using System;
using System.Collections.Generic;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.Data.Events;
using Sitecore.Sites;
using Sitecore.Web;

namespace Foundation.HtmlCache.Events
{
    public class ClearCacheAllSites
    {
        public void Clear(object sender, EventArgs args)
        {
            RemoteEventArgs<ClearCacheArgs> clearCacheArgs = args as RemoteEventArgs<ClearCacheArgs>;
            if (clearCacheArgs != null)
            {
                List<SiteInfo> siteInfos = Factory.GetSiteInfoList();
                foreach (SiteInfo siteInfo in siteInfos)
                {
                    SiteContext siteContext = Factory.GetSite(siteInfo.Name);
                    CacheManager.GetHtmlCache(siteContext)?.Clear();
                }
            }
        }
    }
}