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
                    var cacheQueueEntry = ctx.CacheQueues.AsNoTracking()
                        .OrderByDescending(x => x.CacheQueueMessageType.Id)
                        .ThenBy(x => x.Id)
                        .FirstOrDefault();

                    var processQueue = cacheQueueEntry != null && !cacheQueueEntry.Processing;
                    hasQueueItems = cacheQueueEntry != null;
                    if (processQueue)
                    {
                        if (cacheQueueEntry.CacheQueueMessageTypeId > (int)MessageTypeEnum.AddToCache)
                        {
                            if (!cacheQueueEntry.Processing)
                            {
                                try
                                {
                                    using (var ctxBlocking = new ItemTrackingProvider())
                                    {
                                        ctxBlocking.CacheQueues.First(x =>
                                                x.UpdateVersion == cacheQueueEntry.UpdateVersion)
                                            .Processing = true;
                                        processQueue = ctxBlocking.SaveChanges() > 0;
                                        ctxBlocking.SaveChanges();
                                    }
                                }
                                catch (Exception e)
                                {
                                    processQueue = false;
                                }
                            }
                            else
                            {
                                processQueue = false;
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
                                            DeleteAllSiteCache(ctx, cacheQueueEntry);
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
                                            var cacheQueueEntryToLock = ctxProcessing.CacheQueues.AsNoTracking().FirstOrDefault(x => x.Id == cacheQueueEntry.Id);
                                            if (cacheQueueEntryToLock != null)
                                            {
                                                cacheQueueEntryToLock.Processing = false;
                                                ctxProcessing.CacheQueues.AddOrUpdate(cacheQueueEntryToLock);
                                                ctxProcessing.SaveChanges();
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

        private void DeleteAllSiteCache(ItemTrackingProvider ctx, CacheQueue cacheQueueEntry)
        {
            ctx.CacheQueues.RemoveRange(ctx.CacheQueues.Where(x => x.Id != cacheQueueEntry.Id));
            ctx.Caches.RemoveRange(ctx.Caches);
            ClearCacheArgs remoteEvent = new ClearCacheArgs("cache:clearCacheAllSites:Remote", null, ClearCacheOperation.ClearCacheOperationEnum.AllSites);
            Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);
        }

        private void DeleteSiteAllLangugageCache(ItemTrackingProvider ctx, CacheQueue cacheQueueEntry)
        {
            var name = cacheQueueEntry.Site;
            ctx.CacheQueues.RemoveRange(ctx.CacheQueues.Where(x => x.CacheTemps.Any(y => y.SiteName == name && y.CacheQueueId != cacheQueueEntry.Id)));
            ctx.Caches.RemoveRange(ctx.Caches.Where(x => x.SiteName == name));

            var dic = new Dictionary<string, Dictionary<string, HashSet<string>>>();
            dic.Add(name, null);

            ClearCacheArgs remoteEvent = new ClearCacheArgs("cache:clearCacheSiteAllLanguages:Remote", dic, ClearCacheOperation.ClearCacheOperationEnum.SiteAllLanguages);
            Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);

        }

        private void DeleteSiteCache(ItemTrackingProvider ctx, CacheQueue cacheQueueEntry)
        {
            var name = cacheQueueEntry.Site;
            var lang = cacheQueueEntry.Language;
            var dic = new Dictionary<string, Dictionary<string, HashSet<string>>>();
            dic.Add(name, new Dictionary<string, HashSet<string>>());
            dic[name].Add(lang, null);

            ctx.CacheQueues.RemoveRange(ctx.CacheQueues.Where(x => x.CacheTemps.Any(y => y.SiteName == name && y.SiteLang == lang && y.CacheQueueId != cacheQueueEntry.Id)));
            ctx.Caches.RemoveRange(ctx.Caches.Where(x => x.SiteName == name && x.SiteLang == lang));

            ClearCacheArgs remoteEvent = new ClearCacheArgs("cache:clearCacheSite:Remote", dic, ClearCacheOperation.ClearCacheOperationEnum.Site);
            Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);
        }

        public void DeleteFromCache(ItemTrackingProvider ctx, CacheQueue cacheQueueEntry)
        {
            var result = ctx.CacheTemps
                .Join(ctx.Caches, x => x.ItemId, y => y.ItemId, (left, right) => new { ct = left, c = right })
                .Where(x => x.ct.CacheQueueId == cacheQueueEntry.Id && x.c.SiteLang == cacheQueueEntry.Language)
                .Join(ctx.Caches, x => x.c.HtmlCacheKeyHash, y => y.HtmlCacheKeyHash, (left, right) => new { left.ct, c = right });

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

            ctx.CacheQueues.RemoveRange(result.Select(x => x.ct.CacheQueue).Where(x => x.Id != cacheQueueEntry.Id));
            ctx.Caches.RemoveRange(result.Select(x => x.c));

            ClearCacheArgs remoteEvent = new ClearCacheArgs("cache:clearCacheHtml:Remote", dic, ClearCacheOperation.ClearCacheOperationEnum.Site);
            Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);
        }

        private void AddToCache(ItemTrackingProvider ctx, CacheQueue cacheQueueEntry)
        {
            var sql = string.Format(@"
                WITH CTE (SiteName, SiteLang, HtmlCacheKey, HtmlCacheKeyHash, HtmlCacheResult, ItemId) AS 
                (
                   SELECT SiteName, SiteLang, HtmlCacheKey, HtmlCacheKeyHash, HtmlCacheResult, ItemId 
                   FROM CacheTemp 
                   WHERE CacheQueueId = {0} 
                )
                MERGE [Cache] WITH (HOLDLOCK) AS t 
                USING CTE AS s 
                ON 
                (
                    s.SiteName = t.SiteName 
                    AND s.SiteLang = t.SiteLang 
                    AND s.HtmlCacheKeyHash = t.HtmlCacheKeyHash 
                    AND s.ItemId = t.ItemId
                ) 
                WHEN MATCHED THEN UPDATE
                   SET t.SiteName = s.SiteName, t.SiteLang = s.SiteLang, t.HtmlCacheKey = s.HtmlCacheKey, t.HtmlCacheResult = s.HtmlCacheResult, t.ItemId = s.ItemId 
                WHEN NOT MATCHED THEN
                   INSERT ([SiteName] , [SiteLang] , [HtmlCacheKey] , [HtmlCacheResult] , [ItemId]) 
                   VALUES (s.SiteName, s.SiteLang, s.HtmlCacheKey, s.HtmlCacheResult, s.ItemId);",
                cacheQueueEntry.Id);
            ctx.Database.ExecuteSqlCommand(sql);
        }
    }
}