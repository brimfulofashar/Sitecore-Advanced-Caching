using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Events;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Providers;
using Sitecore;
using Sitecore.Web;

namespace Foundation.HtmlCache.Events
{
    public class ItemPublished
    {
        public void Clear(object sender, EventArgs args)
        {
            RemoteEventArgs<ItemPublishedArgs> itemPublishedEvent = args as RemoteEventArgs<ItemPublishedArgs>;
            if (itemPublishedEvent != null && itemPublishedEvent.Event.PublishOperationEnum != PublishOperation.PublishOperationEnum.Ignore)
            {
                ID itemId = ID.Parse(itemPublishedEvent.Event.ItemId);
                List<SiteInfo> siteInfoList = Factory.GetSiteInfoList();
                foreach (var siteInfo in siteInfoList)
                    if (itemPublishedEvent.Event.PublishOperationEnum == PublishOperation.PublishOperationEnum.Delete ||
                        itemPublishedEvent.Event.PublishOperationEnum == PublishOperation.PublishOperationEnum.Update)
                    {
                        ItemTrackingStore.Instance.Enqueue(new DeleteFromCache(siteInfo, itemId));
                    }
                    else if (itemPublishedEvent.Event.PublishOperationEnum == PublishOperation.PublishOperationEnum.Create)
                    {
                        var publishedItem = Factory.GetDatabase("web").GetItem(itemId);
                        if (publishedItem != null)
                        {
                            var siblingItems = publishedItem.Parent.Children.Where(x => x.ID != itemId);
                            foreach (var sibling in siblingItems)
                            {
                                ItemTrackingStore.Instance.Enqueue(new DeleteFromCache(siteInfo, sibling.ID));
                            }
                        }
                    }
            }
        }
    }
}