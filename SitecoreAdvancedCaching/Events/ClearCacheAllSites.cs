using System;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.Data.Events;

namespace SitecoreAdvancedCaching.Events
{
    public class ClearCacheAllSites
    {
        public void Clear(object sender, EventArgs args)
        {
            var clearCacheArgs = args as RemoteEventArgs<ClearCacheArgs>;
            if (clearCacheArgs != null)
            {
                var siteInfos = Factory.GetSiteInfoList();
                foreach (var siteInfo in siteInfos)
                {
                    var siteContext = Factory.GetSite(siteInfo.Name);
                    CacheManager.GetHtmlCache(siteContext)?.Clear();
                }
            }
        }
    }
}