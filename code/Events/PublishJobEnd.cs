using System;
using System.Linq;
using Foundation.HtmlCache.DB;
using Foundation.HtmlCache.Helpers;
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
                    using (var ctx = new ItemTrackingProvider())
                    {
                        var ids = GuidTVPHelper.GetTVPParameter(publishItemTracking.PublishedItems.Select(x => x.Key).ToList());

                        ctx.UspQueuePublishData(langugage, ids);
                    }
                }
            }

        }
    }
}