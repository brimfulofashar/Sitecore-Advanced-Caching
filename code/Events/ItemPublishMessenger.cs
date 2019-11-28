using System;
using System.Linq;
using Foundation.HtmlCache.Bus;
using Foundation.HtmlCache.Extensions;
using Foundation.HtmlCache.Messages;
using Foundation.HtmlCache.Models;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Publishing.Pipelines.PublishItem;

namespace Foundation.HtmlCache.Events
{
    public class ItemPublishMessenger
    {
        public void PublishItemPublishMessage(object sender, EventArgs args)
        {
            PublishItemContext argContext = ((ItemProcessedEventArgs)args).Context;
            ID itemId = argContext.ItemId;
            object operation = argContext.PublishContext.CustomData[itemId.ToString()];
            if (operation != null)
            {
                var createUpdateOrDeleteOperation = (PublishOperation.PublishOperationEnum)operation;
                Item item = Factory.GetDatabase("web").GetItem(itemId);
                var site = SiteInfoExtensions.GetSites(item);

                if (createUpdateOrDeleteOperation == PublishOperation.PublishOperationEnum.Create)
                {
                    Item parent = item?.Parent;
                    if (parent != null)
                    {
                        foreach (Item child in parent.Children)
                        {
                            var deleteFromCache =
                                new DeleteFromCache(site.First().Name, site.First().Language, child.ID);
                            HtmlCacheMessageBus.Publish(deleteFromCache);
                        }
                    }
                }
                else
                {
                    var deleteFromCache = new DeleteFromCache(Context.Site.SiteInfo.Name, Context.Site.SiteInfo.Language, itemId);
                    HtmlCacheMessageBus.Publish(deleteFromCache);
                }
            }
        }
    }
}