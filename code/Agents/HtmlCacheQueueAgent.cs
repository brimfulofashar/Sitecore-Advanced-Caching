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
                                        catch (Exception ex)
                                        {
                                            Log.Error(string.Format("Failed to unlock cache queue entry {0}", cacheQueueEntry.Id), ex, this);
                                            processQueue = false;
                                        }
                                    }
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
            ctx.Caches.RemoveRange(ctx.Caches);
            ClearCacheArgs remoteEvent = new ClearCacheArgs("cache:clearCacheAllSites:Remote", null, ClearCacheOperation.ClearCacheOperationEnum.AllSites);
            Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);
        }

        private void DeleteSiteAllLangugageCache(ItemTrackingProvider ctx, CacheQueue cacheQueueEntry)
        {
            var name = cacheQueueEntry.CacheTemps.First().SiteName;
            ctx.CacheQueues.RemoveRange(ctx.CacheQueues.Where(x => x.CacheTemps.Any(y => y.SiteName == name && y.CacheQueueId != cacheQueueEntry.Id)));
            ctx.Caches.RemoveRange(ctx.Caches.Where(x => x.SiteName == name));

            var dic = new Dictionary<string, Dictionary<string, HashSet<string>>>();
            dic.Add(name, null);

            ClearCacheArgs remoteEvent = new ClearCacheArgs("cache:clearCacheSiteAllLanguages:Remote", dic, ClearCacheOperation.ClearCacheOperationEnum.SiteAllLanguages);
            Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);

        }

        private void DeleteSiteCache(ItemTrackingProvider ctx, CacheQueue cacheQueueEntry)
        {
            var name = cacheQueueEntry.CacheTemps.First().SiteName;
            var lang = cacheQueueEntry.CacheTemps.First().SiteLang;
            var dic = new Dictionary<string, Dictionary<string, HashSet<string>>>();
            dic.Add(name, new Dictionary<string, HashSet<string>>());
            dic[name].Add(lang, null);

            ClearCacheArgs remoteEvent = new ClearCacheArgs("cache:clearCacheSite:Remote", dic, ClearCacheOperation.ClearCacheOperationEnum.Site);
            Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);


            var result = ctx.CacheTemps
                .Join(ctx.Caches, x => x.ItemId, y => y.ItemId, (left, right) => new {ct = left, c = right})
                .Where(x => (x.c.SiteName == name && x.c.SiteLang == lang) || (x.ct.SiteName == name && x.ct.SiteLang == lang));


            ctx.CacheQueues.RemoveRange(result.Select(x => x.ct.CacheQueue).Where(x => x.Id != cacheQueueEntry.Id));
            ctx.Caches.RemoveRange(result.Select(x => x.c));
        }

        public void DeleteFromCache(ItemTrackingProvider ctx, CacheQueue cacheQueueEntry)
        {
            var result = ctx.CacheTemps
                .Join(ctx.Caches, x => x.ItemId, y => y.ItemId, (left, right) => new {ct = left, c = right})
                .Where(x => x.ct.CacheQueueId == cacheQueueEntry.Id)
                .Join(ctx.Caches, x => x.c.HtmlCacheKeyHash, y=> y.HtmlCacheKeyHash, (left, right) => new { left.ct, c = right});

            var dic = new Dictionary<string, Dictionary<string, HashSet<string>>>();

            foreach (var cacheItem in result.Select(x => x.c))
            {
                if (!dic.ContainsKey(cacheItem.SiteName))
                {
                    dic.Add(cacheItem.SiteName, new Dictionary<string, HashSet<string>>());
                }

                if (!dic[cacheItem.SiteName].ContainsKey(cacheItem.SiteLang))
                {
                    dic[cacheItem.SiteName].Add(cacheItem.SiteLang, new HashSet<string>());
                }

                dic[cacheItem.SiteName][cacheItem.SiteLang].Add(cacheItem.HtmlCacheKey);
            }

            ClearCacheArgs remoteEvent = new ClearCacheArgs("cache:clearCacheHtml:Remote", dic, ClearCacheOperation.ClearCacheOperationEnum.Site);
            Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);

            ctx.CacheQueues.RemoveRange(result.Select(x => x.ct.CacheQueue)).Where(x => x.Id != cacheQueueEntry.Id);
            ctx.Caches.RemoveRange(result.Select(x => x.c));
        }

        private void AddToCache(ItemTrackingProvider ctx, CacheQueue cacheQueueEntry)
        {
            //            foreach (var cacheTemp in cacheQueueEntry.CacheTemps)
            //            {
            //                var cache = new Cache
            //                {
            //                    SiteName = cacheTemp.SiteName,
            //                    SiteLang = cacheTemp.SiteLang,
            //                    ItemId = cacheTemp.ItemId,
            //                    HtmlCacheKey = cacheTemp.HtmlCacheKey,
            //                    HtmlCacheResult = cacheTemp.HtmlCacheResult
            //                };
            //
            //                ctx.Upsert(cache)
            //                    .Key(x => x.SiteName)
            //                    .Key(x => x.SiteLang)
            //                    .Key(x => x.HtmlCacheKeyHash)
            //                    .Key(x => x.ItemId)
            //                    .ExcludeField(x => x.Id)
            //                    .ExcludeField(x => x.HtmlCacheKeyHash)
            //                    .OutputKey(x => x.Id).Execute();
            //            }

            var sql = string.Format(@"
                MERGE [Cache] WITH (HOLDLOCK) AS t 
                USING [CacheTemp] AS s
            ON (s.SiteName = t.SiteName AND s.SiteLang = t.SiteLang AND s.HtmlCacheKeyHash = t.HtmlCacheKeyHash AND s.ItemId = t.ItemId AND s.CacheQueueId = {0})
            WHEN MATCHED
                THEN UPDATE SET 
                    t.SiteName = s.SiteName,
                    t.SiteLang = s.SiteLang,
                    t.HtmlCacheKey = s.HtmlCacheKey,
                    t.HtmlCacheResult = s.HtmlCacheResult
            WHEN NOT MATCHED BY TARGET 
                THEN INSERT ([SiteName]
                           ,[SiteLang]
                           ,[HtmlCacheKey]
                           ,[HtmlCacheResult]
                           ,[ItemId])
                     VALUES (s.SiteName, s.SiteLang, s.HtmlCacheKey, s.HtmlCacheResult, s.ItemId);",
                cacheQueueEntry.Id);
            ctx.Database.ExecuteSqlCommand(sql);
        }
    }
}