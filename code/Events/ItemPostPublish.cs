using System;
using Foundation.HtmlCache.Models;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;

using Sitecore.Publishing.Pipelines.PublishItem;

namespace Foundation.HtmlCache.Events
{
    public class ItemPostPublish
    {
        public void PostCalculatePublishOperation(object sender, EventArgs args)
        {
            PublishItemContext argContext = ((ItemProcessedEventArgs)args).Context;

            var publishItemTracking = Sitecore.Context.Items[PublishItemTracking.Name] as PublishItemTracking;
            if (publishItemTracking != null)
            {
                ID itemId = argContext.ItemId;
                object operation = argContext.PublishContext.CustomData[itemId.ToString()];
                if (operation != null)
                {
                    var createUpdateOrDeleteOperation = (PublishOperation.PublishOperationEnum) operation;
                    Item item = Factory.GetDatabase(publishItemTracking.DestinationDB).GetItem(itemId);
                    
                    if (createUpdateOrDeleteOperation == PublishOperation.PublishOperationEnum.Create)
                    {
                        Item parent = item?.Parent;
                        if (parent != null)
                        {
                            foreach (Item child in parent.Children)
                            {
                                if (!publishItemTracking.PublishedItems.ContainsKey(child.ID.Guid))
                                {
                                    publishItemTracking.PublishedItems.Add(child.ID.Guid,
                                        createUpdateOrDeleteOperation);
                                }
                                else
                                {
                                    publishItemTracking.PublishedItems[child.ID.Guid] = createUpdateOrDeleteOperation;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!publishItemTracking.PublishedItems.ContainsKey(itemId.Guid))
                        {
                            publishItemTracking.PublishedItems.Add(itemId.Guid, createUpdateOrDeleteOperation);
                        }
                        else
                        {
                            publishItemTracking.PublishedItems[itemId.Guid] = createUpdateOrDeleteOperation;
                        }
                    }
                }
            }
        }
    }
}