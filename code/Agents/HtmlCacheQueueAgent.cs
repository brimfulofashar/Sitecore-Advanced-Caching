using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Foundation.HtmlCache.DB;
using Foundation.HtmlCache.Events;
using Foundation.HtmlCache.Extensions;
using Foundation.HtmlCache.Models;
using Sitecore.Configuration;
using Sitecore.Diagnostics;

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
                        .ThenBy(x => x.Id)
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
                                    ctxBlocking.CacheQueueBlockers
                                            .First(x => x.UpdateVersion == cacheQueueBlocker.UpdateVersion)
                                            .BlockingMode =
                                        true;
                                    processQueue = ctxBlocking.SaveChanges() > 0;
                                    ctxBlocking.SaveChanges();
                                }
                            }
                        }

                        if (processQueue)
                        {
                            using (var ctxProcessing = new ItemTrackingProvider())
                            {
                                try
                                {
                                    var cacheQueueEntryToLock = ctxProcessing.CacheQueues.AsNoTracking().FirstOrDefault(x =>
                                        x.Id == cacheQueueEntry.Id && x.UpdateVersion == cacheQueueEntry.UpdateVersion);
                                    if (cacheQueueEntryToLock != null)
                                    {
                                        cacheQueueEntryToLock.Processing = true;
                                        ctxProcessing.CacheQueues.AddOrUpdate(cacheQueueEntryToLock);
                                        var saved = ctxProcessing.SaveChanges() > 0;
                                        if (!saved)
                                        {
                                            processQueue = false;
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Log.Error(string.Format("Failed to lock cache queue entry {0}", cacheQueueEntry.Id), e, this);
                                    processQueue = false;
                                }
                            }

                            if (processQueue)
                            {
                                try
                                {
                                    switch (cacheQueueEntry.CacheQueueMessageType.Id)
                                    {
                                        case (int) MessageTypeEnum.DeleteSiteFromCacheAllSites:
                                        {
                                            DeleteAllSiteCache(ctx);
                                            break;
                                        }
                                        case (int) MessageTypeEnum.DeleteSiteFromCacheAllLanguages:
                                        {
                                            DeleteSiteAllLangugageCache(ctx, cacheQueueEntry);
                                            break;
                                        }
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
                                    Log.Error(string.Format("Failed to process cache queue entry {0}", cacheQueueEntry.Id), e, this);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DeleteAllSiteCache(ItemTrackingProvider ctx)
        {
            ctx.CacheQueues.RemoveRange(ctx.CacheQueues);
            ctx.CacheSiteLangs.RemoveRange(ctx.CacheSiteLangs);
            ClearCacheArgs remoteEvent = new ClearCacheArgs("cache:clearCacheAllSites:Remote", null, null, null, ClearCacheOperation.ClearCacheOperationEnum.AllSites);
            Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);
        }

        private void DeleteSiteAllLangugageCache(ItemTrackingProvider ctx, CacheQueue cacheQueueEntry)
        {
            var name = cacheQueueEntry.CacheSiteLangTemps.First().Name;
            ctx.CacheQueues.RemoveRange(ctx.CacheQueues.Where(x => x.CacheSiteLangTemps.Any(y => y.Name == name)));
            ctx.CacheSiteLangs.RemoveRange(ctx.CacheSiteLangs.Where(x => x.Name == name));

            ClearCacheArgs remoteEvent = new ClearCacheArgs("cache:clearCacheSiteAllLanguages:Remote", new List<string>(){ name }, null, null, ClearCacheOperation.ClearCacheOperationEnum.SiteAllLanguages);
            Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);

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

            ClearCacheArgs remoteEvent = new ClearCacheArgs("cache:clearCacheSite:Remote", cacheQueueEntry.CacheSiteLangTemps.First().Name, cacheQueueEntry.CacheSiteLangTemps.First().Lang, string.Empty, ClearCacheOperation.ClearCacheOperationEnum.Site);
            Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);

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

            var queuedCacheKeys = result.SelectMany(x => x.cq.CacheKeyTemps.Select(y => y.HtmlCacheKey)).ToList();
            var processedCacheKeys = result.SelectMany(x => x.ck.Select(y => y.HtmlCacheKey).ToList()).ToList();
            

            var finalList = new List<string>();
            finalList.AddRange(processedCacheKeys);
            finalList.AddRange(queuedCacheKeys);

            ctx.CacheQueues.RemoveRange(result.Select(x => x.cq));
            ctx.CacheKeys.RemoveRange(result.SelectMany(x => x.ck));


            ClearCacheArgs remoteEvent = new ClearCacheArgs("cache:clearCacheHtml:Remote", null, null,null, ClearCacheOperation.ClearCacheOperationEnum.Site);
            Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);
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