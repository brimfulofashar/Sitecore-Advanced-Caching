using System;
using System.Collections.Generic;
using Foundation.HtmlCache.Extensions;
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
            RemoteEventArgs<ClearCacheArgs> clearCacheArgs = args as RemoteEventArgs<ClearCacheArgs>;
            if (clearCacheArgs != null)
            {
                List<SiteInfo> siteInfos = SiteInfoExtensions.GetSites(item);
                foreach (SiteInfo siteInfo in siteInfos)
                {
                    SiteContext siteContext = Factory.GetSite(siteInfo.Name);
                    CacheManager.GetHtmlCache(siteContext)?.Clear();
                }
            }
        }
    }
}