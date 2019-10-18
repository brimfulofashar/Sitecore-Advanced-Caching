using System;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Events;
using SitecoreAdvancedCaching.Extensions;

namespace SitecoreAdvancedCaching.Events
{
    public class ClearCacheSiteAllLanguages
    {
        public void Clear(object sender, EventArgs args)
        {
            var clearCacheArgs = args as RemoteEventArgs<ClearCacheArgs>;
            if (clearCacheArgs != null)
            {
                var itemId = clearCacheArgs.Event.ItemId;
                if (itemId != null)
                {
                    var item = Factory.GetDatabase("web").GetItem(ID.Parse(itemId));
                    if (item != null)
                    {
                        var siteInfos = SiteInfoExtensions.GetSites(item);
                        foreach (var siteInfo in siteInfos)
                        {
                            var siteContext = Factory.GetSite(siteInfo.Name);
                            CacheManager.GetHtmlCache(siteContext)?.Clear();
                        }
                    }
                }
            }
        }
    }
}