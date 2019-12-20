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
                foreach (var nameLangKey in clearCacheArgs.Event.NameLangKeys)
                {
                    SiteContext siteContext = Factory.GetSite(nameLangKey.Key);
                    CacheManager.GetHtmlCache(siteContext)?.RemoveKeysContaining(htmlCacheKey);
                }
            }
        }
    }
}