using System;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.Data.Events;
using Sitecore.Sites;

namespace Foundation.HtmlCache.Events
{
    public class ClearCacheHtml
    {
        public void Clear(object sender, EventArgs args)
        {
            RemoteEventArgs<ClearCacheArgs> clearCacheArgs = args as RemoteEventArgs<ClearCacheArgs>;
            if (clearCacheArgs != null)
            {
                SiteContext siteContext = Factory.GetSite(clearCacheArgs.Event.CacheSiteLangKeys.SiteName);
                foreach (var htmlCacheKey in clearCacheArgs.Event.CacheSiteLangKeys.HtmlCacheKeys)
                {
                    CacheManager.GetHtmlCache(siteContext)?.RemoveKeysContaining(htmlCacheKey);
                }
            }
        }
    }
}