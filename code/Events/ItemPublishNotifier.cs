using System;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Eventing;
using Sitecore.Publishing;
using Sitecore.Publishing.Pipelines.PublishItem;

namespace Foundation.HtmlCache.Events
{
    public class ItemPublishNotifier
    {
        public void NotifyItemPublications(object sender, EventArgs args)
        {
            PublishItemContext argContext = ((ItemProcessedEventArgs) args).Context;
            ID itemId = argContext.ItemId;
            object operation = argContext.PublishContext.CustomData[itemId.ToString()];
            if (operation != null)
            {
                var createUpdateOrDeleteOperation =(Foundation.HtmlCache.Models.PublishOperation.PublishOperationEnum)operation;
                var remoteEvent = new ItemPublishedArgs("publish:itemProcessed:Remote", itemId.Guid,
                    createUpdateOrDeleteOperation);
                Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);
            }
        }
    }
}