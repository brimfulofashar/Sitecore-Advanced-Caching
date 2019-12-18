using System;
using System.Linq;
using Foundation.HtmlCache.DB;
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
                var cacheQueue = new CacheQueue()
                    {CacheQueueMessageTypeId = (int) MessageTypeEnum.DeleteFromCache};

                var publishedItems = publishItemTracking.PublishedItems.Select(x => new CacheItemsTemp()
                    {Id = Guid.NewGuid(), CacheQueue = cacheQueue, ItemId = x.Key}).ToList();

                cacheQueue.CacheItemsTemps = publishedItems;

                using (var ctx = new ItemTrackingProvider())
                {
                    ctx.CacheQueues.Add(cacheQueue);
                    ctx.SaveChanges();
                }
            }

        }
    }
}