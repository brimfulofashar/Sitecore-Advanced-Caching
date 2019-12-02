using System;
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
    public class ItemPublishMessenger
    {
        public void PublishItemPublishMessage(object sender, EventArgs args)
        {
            PublishItemContext argContext = ((ItemProcessedEventArgs)args).Context;
            ID itemId = argContext.ItemId;
            object operation = argContext.PublishContext.CustomData[itemId.ToString()];
            if (operation != null)
            {
                var createUpdateOrDeleteOperation = (PublishOperation.PublishOperationEnum)operation;
                Item item = Factory.GetDatabase("web").GetItem(itemId);
                var siteInfos = SiteInfoExtensions.GetSites(item);

                foreach (var siteInfo in siteInfos)
                {
                    IRedisCacheProvider redis = DependencyResolver.Current.GetServices<IRedisCacheProvider>()
                        .FirstOrDefault();
                    if (createUpdateOrDeleteOperation == PublishOperation.PublishOperationEnum.Create)
                    {
                        Item parent = item?.Parent;
                        if (parent != null)
                        {
                            foreach (Item child in parent.Children)
                            {
                                var deleteFromCache =
                                    new DeleteFromCache(siteInfo.Name, siteInfo.Language, child.ID.Guid);
                                redis?.Set(child.ID.ToString(), deleteFromCache, 0);
                                redis?.Publish(siteInfo.Name + "_" + siteInfo.Language, "DeleteFromCache");
                            }
                        }
                    }
                    else
                    {
                        var deleteFromCache = new DeleteFromCache(Context.Site.SiteInfo.Name,
                            Context.Site.SiteInfo.Language, itemId.Guid);
                        redis?.Set(itemId.ToString(), deleteFromCache, 0);
                        redis?.Publish(siteInfo.Name + "_" + siteInfo.Language, "DeleteFromCache");
                    }
                }
            }
        }
    }
}