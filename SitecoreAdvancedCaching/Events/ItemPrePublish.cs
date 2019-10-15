using System;
using Sitecore.Configuration;
using Sitecore.Publishing.Pipelines.PublishItem;
using SitecoreAdvancedCaching.Models;

namespace SitecoreAdvancedCaching.Events
{
    public class ItemPrePublish
    {
        public void CalculatePublishOperation(object sender, EventArgs args)
        {
            var argContext = ((ItemProcessingEventArgs) args).Context;
            var itemId = argContext.ItemId;
            var sourceItem = Factory.GetDatabase("master").GetItem(itemId);
            var destinationItem = Factory.GetDatabase("web").GetItem(itemId);
            var operation = destinationItem == null ? PublishOperation.PublishOperationEnum.Create :
                sourceItem == null ? PublishOperation.PublishOperationEnum.Delete : PublishOperation.PublishOperationEnum.Update;
            if (argContext.PublishContext.CustomData[itemId.ToString()] == null)
            {
                argContext.PublishContext.CustomData.Add(itemId.ToString(), operation);
            }
            else
            {
                argContext.PublishContext.CustomData[itemId.ToString()] = operation;
            }
        }
    }
}