using System;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.Data.Events;
using Sitecore.Sites;

namespace Foundation.HtmlCache.Events
{
    public class ClearCacheSite
    {
        public void Clear(object sender, EventArgs args)
        {
            RemoteEventArgs<ClearCacheArgs> clearCacheArgs = args as RemoteEventArgs<ClearCacheArgs>;
            if (clearCacheArgs != null)
            {
                SiteContext siteContext = Factory.GetSite(clearCacheArgs.Event.CacheSiteLangKeys.SiteName);
                if (siteContext != null)
                {
                    if (!string.IsNullOrEmpty(clearCacheArgs.Event.CacheSiteLangKeys.SiteLang))
                    {
                        var stringToMatch = string.Format("#lang:{0}", clearCacheArgs.Event.CacheSiteLangKeys.SiteLang);

                        CacheManager.GetHtmlCache(siteContext)?.RemoveKeysContaining(stringToMatch);
                    }
                    else
                    {
                        CacheManager.GetHtmlCache(siteContext).Clear();
                    }
                }
            }
        }
    }
}