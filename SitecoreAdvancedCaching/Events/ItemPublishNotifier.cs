using System;
using Sitecore.Configuration;
using Sitecore.Publishing.Pipelines.PublishItem;
using SitecoreAdvancedCaching.Models;

namespace SitecoreAdvancedCaching.Events
{
    public class ItemPublishNotifier
    {
        public void NotifyItemPublications(object sender, EventArgs args)
        {
            var argContext = ((ItemProcessedEventArgs) args).Context;
            var itemId = argContext.ItemId;
            var operation = argContext.PublishContext.CustomData[itemId.ToString()];
            if (operation != null)
            {
                var createUpdateOrDeleteOperation = (PublishOperation.PublishOperationEnum) operation;
                var remoteEvent = new ItemPublishedArgs("publish:itemProcessed:Remote", itemId.Guid,
                    createUpdateOrDeleteOperation);
                Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);
            }
        }
    }
}