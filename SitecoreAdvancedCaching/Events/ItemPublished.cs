using System;
using System.Linq;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Events;
using SitecoreAdvancedCaching.Providers;

namespace SitecoreAdvancedCaching.Events
{
    public class ItemPublished
    {
        public void Clear(object sender, EventArgs args)
        {
            var itemPublishedEvent = args as RemoteEventArgs<ItemPublishedArgs>;
            if (itemPublishedEvent != null)
            {
                var itemId = ID.Parse(itemPublishedEvent.Event.ItemId);
                var siteInfoList = Factory.GetSiteInfoList();
                foreach (var siteInfo in siteInfoList)
                    if (siteInfo.HtmlCache.InnerCache.GetCacheKeys().Any(x => x.Contains(itemId.ToString())))
                    {
                        siteInfo.HtmlCache.RemoveKeysContaining(itemId.ToString());
                        if (itemPublishedEvent.Event.ItemIsDeleted) ItemAccessTracker.Instance.Remove(itemId);
                    }
                    else
                    {
                        var publishedItem = Database.GetDatabase("web").GetItem(itemId);
                        if (publishedItem != null)
                        {
                            var siblingItems = publishedItem.Parent.Children
                                .Where(x => x.ID != itemId);
                            foreach (var sibling in siblingItems)
                                siteInfo.HtmlCache.RemoveKeysContaining(sibling.ID.ToString());
                        }
                    }
            }
        }
    }
}