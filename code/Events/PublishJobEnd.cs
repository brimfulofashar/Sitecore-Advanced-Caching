﻿using System;
using Foundation.HtmlCache.DB;
using Foundation.HtmlCache.Helpers;
using Foundation.HtmlCache.Messaging.Message;
using Foundation.HtmlCache.Messaging.Repository;
using Foundation.HtmlCache.Models;

namespace Foundation.HtmlCache.Events
{
    public class PublishJobEnd
    {
        public void SavePublishedItemsToCacheDb(object sender, EventArgs args)
        {
            var publishItemTracking = Sitecore.Context.Items[PublishItemTracking.Name] as PublishItemTracking;
            if (publishItemTracking != null)
            {
                foreach (var language in publishItemTracking.Languages)
                {
                    using (var ctx = new ItemTrackingProvider())
                    {
                        var tvpHelper = new TVPHelper();
                        foreach (var publishedItem in publishItemTracking.PublishedItems)
                        {
                            tvpHelper.ProcessPublishData(publishedItem.Key, language, publishedItem.Value == PublishOperation.PublishOperationEnum.Delete);
                        }
                        var tvp = tvpHelper.TVP.Tables[tvpHelper.CacheItem_TVP];
                        var cacheEntriesToClear = ctx.UspDeleteCacheData(tvp);

                        BroadcastHtmlCacheRepository broadcastHtmlCacheRepository = new BroadcastHtmlCacheRepository();

                        foreach (var cacheEntryToClear in cacheEntriesToClear)
                        {
                            broadcastHtmlCacheRepository.BroadcastMessage(new BroadcastHtmlCacheMessage
                            {
                                SiteName = cacheEntryToClear.SiteName,
                                SiteLang = cacheEntryToClear.SiteLang,
                                HtmlCacheKey = cacheEntryToClear.HtmlCacheKey,
                                ToRemove = true
                            });
                        }
                    }
                }
            }
        }
    }
}