using System;
using System.Linq;
using Foundation.HtmlCache.Arguments;
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
                        foreach (var language in publishItemTracking.Languages)
                        {
//                            var ids = TVPHelper.GetTVPParameter(publishItemTracking.PublishedItems .Select(x => new ItemMetaData(x.Key, language, x.Value == PublishOperation.PublishOperationEnum.Delete)).ToList());
//
//                            ctx.UspQueuePublishData(langugage, ids);
                        }

                        
                    }
                }
            }

        }
    }
}