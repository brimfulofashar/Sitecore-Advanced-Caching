using System;
using System.Collections.Generic;
using Foundation.HtmlCache.Extensions;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Providers;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Items;
using Sitecore.Sites;
using Sitecore.Web;

namespace Foundation.HtmlCache.Events
{
    public class ClearCacheSiteAllLanguages
    {
        public void Clear(object sender, EventArgs args)
        {
            var clearCacheArgs = args as RemoteEventArgs<ClearCacheArgs>;
            if (clearCacheArgs != null)
            {
                Guid? itemId = clearCacheArgs.Event.ItemId;
                if (itemId != null)
                {
                    Item item = Factory.GetDatabase("web").GetItem(ID.Parse(itemId));
                    if (item != null)
                    {
                        List<SiteInfo> siteInfos = SiteInfoExtensions.GetSites(item);
                        foreach (SiteInfo siteInfo in siteInfos)
                        {
                            ItemAccessTracker.Instance.Enqueue(new DeleteSiteFromCache(siteInfo));
                            SiteContext siteContext = Factory.GetSite(siteInfo.Name);
                            CacheManager.GetHtmlCache(siteContext)?.Clear();
                        }
                    }
                }
            }
        }
    }
}