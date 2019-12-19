using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Foundation.HtmlCache.DB;
using Foundation.HtmlCache.Extensions;

namespace Foundation.HtmlCache.Agents
{
    public class HtmlCacheQueueAgent
    {
        public void ProcessCacheQueue()
        {
            bool hasQueueItems = true;
            using (var ctx = new ItemTrackingProvider())
            {
                while (hasQueueItems)
                {
                    var cacheQueueEntry = ctx.CacheQueues
                        .OrderByDescending(x => x.CacheQueueMessageType.Id)
                        .ThenBy(x => x.Id).AsNoTracking()
                        .FirstOrDefault(x => !x.Processing);

                    var processQueue = cacheQueueEntry != null;
                    hasQueueItems = cacheQueueEntry != null;
                    if (processQueue)
                    {
                        if (cacheQueueEntry.CacheQueueMessageTypeId > (int)MessageTypeEnum.AddToCache)
                        {
                            var cacheQueueBlocker = ctx.CacheQueueBlockers.First();
                            if (!cacheQueueBlocker.BlockingMode)
                            {
                                using (var ctxBlocking = new ItemTrackingProvider())
                                {
                                    ctxBlocking.Database.BeginTransaction();
                                    ctxBlocking.CacheQueueBlockers
                                            .First(x => x.UpdateVersion == cacheQueueBlocker.UpdateVersion)
                                            .BlockingMode =
                                        true;
                                    processQueue = ctxBlocking.SaveChanges() > 0;
                                    ctxBlocking.Database.CurrentTransaction.Commit();
                                }
                            }
                        }

                        if (processQueue)
                        {
                            using (var ctxProcessing = new ItemTrackingProvider())
                            {
                                try
                                {
                                    cacheQueueEntry.Processing = true;
                                    ctxProcessing.CacheQueues.AddOrUpdate(cacheQueueEntry);
                                    var saved = ctxProcessing.SaveChanges() > 0;
                                    if (!saved)
                                    {
                                        processQueue = false;
                                    }
                                }
                                catch (Exception e)
                                {

                                }
                            }

                            if (processQueue)
                            {
                                try
                                {
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

                                    using (var ctxDeleteQueue = new ItemTrackingProvider())
                                    {
                                        cacheQueueEntry =
                                            ctxDeleteQueue.CacheQueues.First(x => x.Id == cacheQueueEntry.Id);
                                        ctxDeleteQueue.CacheQueues.Remove(cacheQueueEntry);
                                        ctxDeleteQueue.SaveChanges();
                                    }
                                }
                                catch (Exception e)
                                {

                                }
                            }
                        }
                    }
                }
            }
        }

        private void DeleteSiteCache(ItemTrackingProvider ctx, CacheQueue cacheQueueEntry)
        {
            var result = ctx.CacheSiteLangTemps.Where(x =>
                    x.Name == cacheQueueEntry.CacheSiteLangTemps.First().Name &&
                    x.Lang == cacheQueueEntry.CacheSiteLangTemps.First().Lang)
                .GroupJoin(ctx.CacheSiteLangs, x => x.Name, y => y.Name,
                    (left, right) => new
                    {
                        cq = left.CacheKeyTemps.Select(x => x.CacheQueue),
                        csl = right.Select(x => x)
                    });

            ctx.CacheQueues.RemoveRange(result.SelectMany(x => x.cq));
            ctx.CacheSiteLangs.RemoveRange(result.SelectMany(x => x.csl));

            // raise event
        }

        public void DeleteFromCache(ItemTrackingProvider ctx, CacheQueue cacheQueueEntry)
        {
            var result = ctx.CacheItemTemps
                .Join(ctx.CacheItemTemps, x => x.ItemId, y => y.ItemId,
                    (left, right) => new { cit1 = left, cit2 = right })
                .Where(x => x.cit1.CacheKeyId == null)
                .Where(x => x.cit1.CacheQueueId == cacheQueueEntry.Id)
                .Join(ctx.CacheKeyTemps, x => x.cit2.CacheKeyId, y => y.Id,
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

        private void AddToCache(ItemTrackingProvider ctx, CacheQueue cacheQueueEntry)
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

                foreach (var cachekeyTemp in cacheSiteLangTemp.CacheKeyTemps)
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
                        .ExcludeField(x => x.CacheKeyItems)
                        .ExcludeField(x => x.CacheSiteLang)
                        .ExcludeField(x => x.CacheItems)
                        .OutputKey(x => x.Id).Execute();

                    foreach (var cacheKeyItemTemp in cachekeyTemp.CacheItemTemps)
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
                            .ExcludeField(x => x.CacheKeyItems)
                            .OutputKey(x => x.Id).Execute();

                        var cacheKeyItem = new CacheKeyItem()
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