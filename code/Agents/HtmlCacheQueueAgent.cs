using System.Data.Entity;
using System.Linq;
using Foundation.HtmlCache.DB;
using Foundation.HtmlCache.Events;
using Foundation.HtmlCache.Models;
using Sitecore.Configuration;

namespace Foundation.HtmlCache.Agents
{
    public class HtmlCacheQueueAgent
    {
        public void ProcessCacheQueue()
        {
            using (var ctx = new ItemTrackingProvider())
            {
                var hasQueueItems = ctx.CacheQueues.AsNoTracking().Any();
                while (hasQueueItems)
                {
                    var results = ctx.UspLockAndProcessCacheQueueEntry(Sitecore.Configuration.Settings.GetSetting("InstanceName"), out var cacheQueueCount);
                    hasQueueItems = cacheQueueCount > 0;

                    foreach (var result in results.GroupBy(x => new { x.SiteName, x.SiteLang, x.CacheQueueMessageTypeId}))
                    {
                        if (result.Key.CacheQueueMessageTypeId > (int) MessageTypeEnum.AddToCache)
                        {
                            CacheSiteLangKeys cacheSiteLangKeys = new CacheSiteLangKeys
                            {
                                SiteName = result.Key.SiteName,
                                SiteLang = result.Key.SiteLang,
                                CacheQueueMessageTypeId = result.Key.CacheQueueMessageTypeId,
                                HtmlCacheKeys = result.Select(x => x.HtmlCacheKey).ToList()
                            };

                            ClearCacheArgs remoteEvent = null;
                            switch (result.Key.CacheQueueMessageTypeId)
                            {
                                case (int) MessageTypeEnum.DeleteHtmlFromCache:
                                {
                                    remoteEvent = new ClearCacheArgs("cache:clearCacheHtml:Remote", cacheSiteLangKeys,
                                        ClearCacheOperation.ClearCacheOperationEnum.Site);
                                    break;
                                }
                                case (int) MessageTypeEnum.DeleteSiteFromCache:
                                {
                                    remoteEvent = new ClearCacheArgs("cache:clearCacheSite:Remote", cacheSiteLangKeys,
                                        ClearCacheOperation.ClearCacheOperationEnum.Site);
                                    break;
                                }
                            }

                            Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);
                        }
                    }
                }
            }
        }
    }
}