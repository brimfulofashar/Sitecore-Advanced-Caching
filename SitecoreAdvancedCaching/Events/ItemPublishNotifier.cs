using System;
using System.Linq;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Eventing;
using Sitecore.Publishing;
using Sitecore.Publishing.Pipelines.PublishItem;
using SitecoreAdvancedCaching.Providers;

namespace SitecoreAdvancedCaching.Events
{
    public class ItemPublishNotifier
    {
        public void NotifyItemPublications(object sender, EventArgs args)
        {
            var argContext = ((ItemProcessedEventArgs) args).Context;
            var itemId = argContext.ItemId;
            var remoteEvent = new ItemPublishedEvent(Settings.InstanceName, "publish:itemProcessed:Remote", itemId.Guid,
                argContext.Action == PublishAction.DeleteTargetItem);
            //            EventManager.QueueEvent(remoteEvent);

            var siteInfoList = Factory.GetSiteInfoList();
            foreach (var siteInfo in siteInfoList)
                if (siteInfo.HtmlCache.InnerCache.GetCacheKeys().Any(x => x.Contains(itemId.ToString())))
                {
                    siteInfo.HtmlCache.RemoveKeysContaining(itemId.ToString());
                    if (argContext.Action == PublishAction.DeleteTargetItem) ItemAccessTracker.Instance.Remove(itemId);
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