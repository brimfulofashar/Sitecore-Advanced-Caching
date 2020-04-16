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
                    var result = ctx.UspLockAndProcessCacheQueueEntry(Sitecore.Configuration.Settings.GetSetting("InstanceName"), out var cacheQueueCount);
                    hasQueueItems = cacheQueueCount > 0;

                    if (result.CacheQueueMessageTypeId > (int)MessageTypeEnum.AddToCache)
                    {
                        CacheSiteLangKeys cacheSiteLangKeys = new CacheSiteLangKeys
                        {
                            SiteName = result.SiteName,
                            SiteLang = result.SiteLang,
                            CacheQueueMessageTypeId = result.CacheQueueMessageTypeId,
                            HtmlCacheKeys = result.HtmlCacheKey.Split('|').ToList()
                        };

                        ClearCacheArgs remoteEvent = null;
                        switch (result.CacheQueueMessageTypeId)
                        {
                            case (int)MessageTypeEnum.DeleteHtmlFromCache:
                            {
                                remoteEvent = new ClearCacheArgs("cache:clearCacheHtml:Remote", cacheSiteLangKeys,
                                    ClearCacheOperation.ClearCacheOperationEnum.Site);
                                break;
                            }
                            case (int)MessageTypeEnum.DeleteSiteFromCache:
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