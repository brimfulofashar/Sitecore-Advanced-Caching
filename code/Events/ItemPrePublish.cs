using System;
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

            var publishItemTracking = Sitecore.Context.Items[PublishItemTracking.Name] as PublishItemTracking;
            if (publishItemTracking != null)
            {
                Item sourceItem = Factory.GetDatabase(publishItemTracking.SourceDB).GetItem(itemId);
                Item destinationItem = Factory.GetDatabase(publishItemTracking.DestinationDB).GetItem(itemId);
                PublishOperation.PublishOperationEnum operation =
                    destinationItem == null
                        ? PublishOperation.PublishOperationEnum.Create
                        :
                        (sourceItem == null || sourceItem.Publishing.NeverPublish)
                            ?
                            PublishOperation.PublishOperationEnum.Delete
                            :
                            PublishOperation.PublishOperationEnum.Update;

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
}