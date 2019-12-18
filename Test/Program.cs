using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.HtmlCache.DB;
using Foundation.HtmlCache.Extensions;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new ItemTrackingProvider())
            {
                var cacheQueueEntry = ctx.CacheQueues
                    .OrderByDescending(x => x.CacheQueueMessageType.Id)
                    .ThenBy(x => x.Id)
                    .FirstOrDefault(x => !x.Processing);

                var processQueue = true;
                if (cacheQueueEntry != null)
                {
                    using (var ctxProcessing = new ItemTrackingProvider())
                    {
                        cacheQueueEntry.Processing = true;
                        ctxProcessing.Upsert(cacheQueueEntry).ExcludeField(x => x.CacheItemsTemps).ExcludeField(x => x.CacheSiteLangTemps).ExcludeField(x => x.CacheKeysTemps).Key(x => x.Id).OutputKey(x => x.Id);
                        ctxProcessing.SaveChanges();
                    }

                    if (cacheQueueEntry.CacheQueueMessageTypeId > (int) MessageTypeEnum.AddToCache)
                    {
                        var cacheQueueBlocker = ctx.CacheQueueBlockers.First();
                        if (!cacheQueueBlocker.BlockingMode)
                        {
                            using (var ctxBlocking = new ItemTrackingProvider())
                            {
                                ctxBlocking.Database.BeginTransaction();
                                ctxBlocking.CacheQueueBlockers
                                    .First(x => x.UpdateVersion == cacheQueueBlocker.UpdateVersion).BlockingMode = true;
                                processQueue = ctxBlocking.SaveChanges() > 0;
                                ctxBlocking.Database.CurrentTransaction.Commit();
                            }
                        }
                    }

                    if (processQueue)
                    {
                        try
                        {
                            ctx.Database.BeginTransaction();
                            switch (cacheQueueEntry.CacheQueueMessageType.Id)
                            {

                                case (int) MessageTypeEnum.DeleteSiteFromCache:
                                {
                                    DeleteSiteCache(ctx, cacheQueueEntry);
                                    break;
                                }
                                case (int) MessageTypeEnum.DeleteFromCache:
                                {
                                    DeleteFromCache(ctx, cacheQueueEntry);
                                    break;
                                }
                                case (int) MessageTypeEnum.AddToCache:
                                {
                                    AddToCache(ctx, cacheQueueEntry);
                                    break;
                                }
                            }

                            ctx.SaveChanges();
                            ctx.Database.CurrentTransaction.Commit();

                            using (var ctxDeleteQueue = new ItemTrackingProvider())
                            {
                                cacheQueueEntry = ctxDeleteQueue.CacheQueues.First(x => x.Id == cacheQueueEntry.Id);
                                ctxDeleteQueue.CacheQueues.Remove(cacheQueueEntry);
                                ctxDeleteQueue.SaveChanges();
                            }
                        }
                        catch (Exception e)
                        {
                            ctx.Database.CurrentTransaction.Rollback();
                        }
                    }
                }
            }
        }

        private static void DeleteSiteCache(ItemTrackingProvider ctx, CacheQueue cacheQueueEntry)
        {
            var result = ctx.CacheSiteLangTemps.Where(x =>
                    x.Name == cacheQueueEntry.CacheSiteLangTemps.First().Name &&
                    x.Lang == cacheQueueEntry.CacheSiteLangTemps.First().Lang)
                .GroupJoin(ctx.CacheSiteLangs, x => x.Name, y => y.Name,
                    (left, right) => new
                    {
                        cq = left.CacheKeysTemps.Select(x => x.CacheQueue),
                        csl = right.Select(x => x)
                    });

            ctx.CacheQueues.RemoveRange(result.SelectMany(x => x.cq));
            ctx.CacheSiteLangs.RemoveRange(result.SelectMany(x => x.csl));

            // raise event
        }

        public static void DeleteFromCache(ItemTrackingProvider ctx, CacheQueue cacheQueueEntry)
        {
            var result = ctx.CacheItemsTemps
                .Join(ctx.CacheItemsTemps, x => x.ItemId, y => y.ItemId,
                    (left, right) => new { cit1 = left, cit2 = right })
                .Where(x => x.cit1.CacheKeyId == null)
                .Where(x => x.cit1.CacheQueueId == cacheQueueEntry.Id)
                .Join(ctx.CacheKeysTemps, x => x.cit2.CacheKeyId, y => y.Id,
                    (left, right) => new { cit2 = left, ckt = right })
                .GroupJoin(ctx.CacheKeys, x => x.ckt.HtmlCacheKey, y => y.HtmlCacheKey,
                    (left, right) => new
                    {
                        cq = left.ckt.CacheQueue,
                        ck = right.Select(x => x).Where(x => x != null)
                    });

            ctx.CacheQueues.RemoveRange(result.Select(x => x.cq));
            ctx.CacheKeys.RemoveRange(result.SelectMany(x => x.ck));
        }

        private static void AddToCache(ItemTrackingProvider ctx, CacheQueue cacheQueueEntry)
        {
            foreach (var cacheSiteLangTemp in cacheQueueEntry.CacheSiteLangTemps)
            {
                var cacheSiteLang = new CacheSiteLang
                {
                    Lang = cacheSiteLangTemp.Lang,
                    Name = cacheSiteLangTemp.Name,
                };

                var cacheSiteLangId = ctx.Upsert(cacheSiteLang)
                    .Key(x => x.Name)
                    .Key(x => x.Lang)
                    .ExcludeField(x => x.Id)
                    .ExcludeField(x => x.CacheKeys)
                    .OutputKey(x => x.Id).Execute();

                foreach (var cachekeyTemp in cacheSiteLangTemp.CacheKeysTemps)
                {
                    var cacheKey = new CacheKey
                    {
                        HtmlCacheKey = cachekeyTemp.HtmlCacheKey,
                        HtmlCacheResult = cachekeyTemp.HtmlCacheResult,
                        CacheSiteLangId = cacheSiteLangId
                    };

                    var cacheKeyId = ctx.Upsert(cacheKey).Key(x => x.HtmlCacheKey)
                        .Key(x => x.CacheSiteLangId)
                        .ExcludeField(x => x.Id)
                        .ExcludeField(x => x.CacheKeysItems)
                        .ExcludeField(x => x.CacheSiteLang)
                        .ExcludeField(x => x.CacheItems)
                        .OutputKey(x => x.Id).Execute();

                    foreach (var cacheKeyItemTemp in cachekeyTemp.CacheItemsTemps)
                    {
                        var cacheItem = new CacheItem
                        {
                            ItemId = cacheKeyItemTemp.ItemId,
                            CacheKeyId = cacheKeyId
                        };

                        var cacheItemId = ctx.Upsert(cacheItem).Key(x => x.ItemId)
                            .Key(x => x.CacheKeyId)
                            .ExcludeField(x => x.Id)
                            .ExcludeField(x => x.CacheKey)
                            .ExcludeField(x => x.CacheKeysItems)
                            .OutputKey(x => x.Id).Execute();

                        var cacheKeyItem = new CacheKeysItem()
                        {
                            CacheKeyId = cacheKeyId,
                            CacheItemId = cacheItemId
                        };

                        ctx.Upsert(cacheKeyItem)
                            .Key(x => x.CacheKeyId)
                            .Key(x => x.CacheItemId)
                            .ExcludeField(x => x.Id)
                            .ExcludeField(x => x.CacheItem)
                            .ExcludeField(x => x.CacheKey)
                            .OutputKey(x => x.Id).Execute();
                    }
                }
            }
        }
    }
}