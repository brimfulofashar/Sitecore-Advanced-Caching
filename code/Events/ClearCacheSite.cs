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
                foreach (var name in clearCacheArgs.Event.NameLangKeys.Keys)
                {
                    foreach (var lang in clearCacheArgs.Event.NameLangKeys[name].Keys)
                    {
                        var stringToMatch = string.Format("#lang:{0}", lang);

                        SiteContext siteContext = Factory.GetSite(name);
                        CacheManager.GetHtmlCache(siteContext)?.RemoveKeysContaining(stringToMatch);
                    }
                }
            }
        }
    }
}