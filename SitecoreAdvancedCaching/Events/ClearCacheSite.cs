using System;
using System.Linq;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Globalization;
using SitecoreAdvancedCaching.Extensions;

namespace SitecoreAdvancedCaching.Events
{
    public class ClearCacheSite
    {
        public void Clear(object sender, EventArgs args)
        {
            var clearCacheArgs = args as RemoteEventArgs<ClearCacheArgs>;
            if (clearCacheArgs != null)
            {
                var itemId = clearCacheArgs.Event.ItemId;
                var languageStr = clearCacheArgs.Event.Language;
                if (itemId != null && !string.IsNullOrEmpty(languageStr))
                {
                    var language = Language.Parse(languageStr);
                    var item = Factory.GetDatabase("web").GetItem(ID.Parse(itemId), language);
                    if (item != null)
                    {
                        var siteInfo = SiteInfoExtensions.GetSites(item, language).FirstOrDefault();
                        if (siteInfo != null)
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