using System;
using Sitecore.Configuration;
using Sitecore.Publishing.Pipelines.PublishItem;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Settings;

namespace Foundation.HtmlCache.Events
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
            if ((sourceItem != null && GlobalCacheTemplateSettings.Instance.GlobalCacheableTemplateIDs.Contains(sourceItem.TemplateID.Guid)) ||
                (destinationItem != null && GlobalCacheTemplateSettings.Instance.GlobalCacheableTemplateIDs.Contains(destinationItem.TemplateID.Guid)))
            {
                if (argContext.PublishContext.CustomData[itemId.ToString()] == null)
                {
                    argContext.PublishContext.CustomData.Add(itemId.ToString(), operation);
                }
                else
                {
                    argContext.PublishContext.CustomData[itemId.ToString()] = operation;
                }
            }
            else
            {
                if (argContext.PublishContext.CustomData[itemId.ToString()] == null)
                {
                    argContext.PublishContext.CustomData.Add(itemId.ToString(), PublishOperation.PublishOperationEnum.Ignore);
                }
                else
                {
                    argContext.PublishContext.CustomData[itemId.ToString()] = PublishOperation.PublishOperationEnum.Ignore;
                }
            }
        }
    }
}