using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Providers;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Publishing;

namespace Foundation.HtmlCache.Events
{
    public class PublishJobEnd
    {
        public void SavePublishedItemsToCacheDb(object sender, EventArgs args)
        {
            var publishItemTracking = Sitecore.Context.Items[PublishItemTracking.Name] as PublishItemTracking;
            if (publishItemTracking != null)
            {
                var cacheQueue = new CacheQueue()
                    {CacheQueueMessageTypeId = (int) CacheQueueMessageType.MessageTypeEnum.DeleteFromCache};

                var publishedItems = publishItemTracking.PublishedItems.Select(x => new PublishedItem()
                    {CacheQueue = cacheQueue, ItemId = x.Key}).ToList();

                cacheQueue.PublishedItems = publishedItems;

                using (var ctx = new ItemTrackingProvider())
                {
                    ctx.CacheQueues.Add(cacheQueue);
                    ctx.SaveChanges();
                }
            }

        }
    }
}