using System.Collections.Generic;
using System.Linq;
using Foundation.HtmlCache.DB;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Pipelines;
using Sitecore.Sites;

namespace Foundation.HtmlCache.Pipelines
{
    public class InitializePersistedHtmlCache
    {
        public void Initialize(PipelineArgs args)
        {
            var siteNames = Factory.GetSiteInfoList().Select(x => x.Name);

            foreach (var siteName in siteNames)
            {
                using (var ctx = new ItemTrackingProvider())
                {
                    var htmlResults = ctx.UspGetCacheForSite(siteName);

                    SiteContext siteContext = Factory.GetSite(siteName);

                    foreach (var htmlResult in htmlResults)
                    {
                        CacheManager.GetHtmlCache(siteContext)
                            .SetHtml(htmlResult.HtmlCacheKey, htmlResult.HtmlCacheResult);
                    }
                }
            }
        }
    }
}