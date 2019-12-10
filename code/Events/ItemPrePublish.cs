using System;
using System.Collections.Generic;
using Sitecore.Configuration;
using Sitecore.Publishing.Pipelines.PublishItem;
using Foundation.HtmlCache.Models;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Foundation.HtmlCache.Events
{
    public class ItemPrePublish
    {
        public void PreCalculatePublishOperation(object sender, EventArgs args)
        {
            PublishItemContext argContext = ((ItemProcessingEventArgs) args).Context;
            ID itemId = argContext.ItemId;
            Item sourceItem = Factory.GetDatabase("master").GetItem(itemId);
            Item destinationItem = Factory.GetDatabase("web").GetItem(itemId);
            PublishOperation.PublishOperationEnum operation = 
                destinationItem == null ? PublishOperation.PublishOperationEnum.Create :
                (sourceItem == null || sourceItem.Publishing.NeverPublish)? PublishOperation.PublishOperationEnum.Delete : 
                PublishOperation.PublishOperationEnum.Update;

            if (argContext.PublishContext.CustomData[itemId.ToString()] == null)
            {
                argContext.PublishContext.CustomData.Add(itemId.ToString(), operation);
            }
            else
            {
                argContext.PublishContext.CustomData[itemId.ToString()] = operation;
            }

            HashSet<string> dic;

            if (argContext.PublishContext.CustomData["Publishing"] == null)
            {
                dic = new HashSet<string>();
                argContext.PublishContext.CustomData["Publishing"] = dic;
            }
            
        }
    }
}