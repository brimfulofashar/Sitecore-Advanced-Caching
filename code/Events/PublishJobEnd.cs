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
                foreach (var langugage in publishItemTracking.Languages)
                {
                    var cacheQueue = new CacheQueue()
                        {CacheQueueMessageTypeId = (int) MessageTypeEnum.DeleteFromCache, Language = langugage};

                    var publishedItems = publishItemTracking.PublishedItems.Select(x => new CacheTemp()
                        {CacheQueue = cacheQueue, ItemId = x.Key}).ToList();

                    cacheQueue.CacheTemps = publishedItems;

                    using (var ctx = new ItemTrackingProvider())
                    {
                        ctx.CacheQueues.Add(cacheQueue);
                        ctx.SaveChanges();
                    }
                }
            }

        }
    }
}