using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Foundation.HtmlCache.Extensions;
using Foundation.HtmlCache.Messages;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Providers;
using Sitecore;
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

            HashSet<string> dic = argContext.PublishContext.CustomData["Publishing"] as HashSet<string>;
            if (dic != null)
            {
                ID itemId = argContext.ItemId;
                object operation = argContext.PublishContext.CustomData[itemId.ToString()];
                if (operation != null)
                {
                    var createUpdateOrDeleteOperation = (PublishOperation.PublishOperationEnum) operation;
                    Item item = Factory.GetDatabase("web").GetItem(itemId);
                    var siteInfos = SiteInfoExtensions.GetSites(item);

                    foreach (var siteInfo in siteInfos)
                    {
                        if (createUpdateOrDeleteOperation == PublishOperation.PublishOperationEnum.Create)
                        {
                            Item parent = item?.Parent;
                            if (parent != null)
                            {
                                foreach (Item child in parent.Children)
                                {
                                    dic.Add(child.ID.ToString());
                                }
                            }
                        }
                        else
                        {
                            dic.Add(itemId.ToString());
                        }
                    }
                }
            }
        }
    }
}