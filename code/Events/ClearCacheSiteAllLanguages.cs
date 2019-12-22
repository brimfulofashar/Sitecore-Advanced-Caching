using System;
using System.Linq;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.Data.Events;
using Sitecore.Sites;

namespace Foundation.HtmlCache.Events
{
    public class ClearCacheSiteAllLanguages
    {
        public void Clear(object sender, EventArgs args)
        {
            RemoteEventArgs<ClearCacheArgs> clearCacheArgs = args as RemoteEventArgs<ClearCacheArgs>;
            if (clearCacheArgs != null)
            {
                var siteInfo = Factory.GetSiteInfo(clearCacheArgs.Event.NameLangKeys.FirstOrDefault().Key);
                SiteContext siteContext = Factory.GetSite(siteInfo.Name);
                CacheManager.GetHtmlCache(siteContext)?.Clear();
            }
        }
    }
}