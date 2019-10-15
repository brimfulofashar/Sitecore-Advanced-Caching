using System;
using Sitecore.Configuration;
using Sitecore.Eventing;
using Sitecore.Publishing;
using Sitecore.Publishing.Pipelines.PublishItem;

namespace SitecoreAdvancedCaching.Events
{
    public class ItemPublishNotifier
    {
        public void NotifyItemPublications(object sender, EventArgs args)
        {
            var argContext = ((ItemProcessedEventArgs) args).Context;
            var itemId = argContext.ItemId;
            var remoteEvent = new ItemPublishedArgs("publish:itemProcessed:Remote", itemId.Guid,
                argContext.Action == PublishAction.DeleteTargetItem);
            Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);
        }
    }
}