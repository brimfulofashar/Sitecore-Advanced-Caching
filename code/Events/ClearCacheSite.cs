using System;
using System.Linq;
using Foundation.HtmlCache.Extensions;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Providers;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Sites;
using Sitecore.Web;

namespace Foundation.HtmlCache.Events
{
    public class ClearCacheSite
    {
        public void Clear(object sender, EventArgs args)
        {
            RemoteEventArgs<ClearCacheArgs> clearCacheArgs = args as RemoteEventArgs<ClearCacheArgs>;
            if (clearCacheArgs != null)
            {
                Guid? itemId = clearCacheArgs.Event.ItemId;
                string languageStr = clearCacheArgs.Event.Language;
                if (itemId != null && !string.IsNullOrEmpty(languageStr))
                {
                    Language language = Language.Parse(languageStr);
                    Item item = Factory.GetDatabase("web").GetItem(ID.Parse(itemId), language);
                    if (item != null)
                    {
                        SiteInfo siteInfo = SiteInfoExtensions.GetSites(item, language).FirstOrDefault();
                        if (siteInfo != null)
                        {
                            ItemTrackingStore.Instance.Enqueue(new DeleteSiteFromCache(siteInfo));
                            SiteContext siteContext = Factory.GetSite(siteInfo.Name);
                            CacheManager.GetHtmlCache(siteContext)?.Clear();
                        }
                    }
                }
            }
        }
    }
}