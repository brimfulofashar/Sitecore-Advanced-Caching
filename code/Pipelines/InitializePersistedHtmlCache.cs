using Foundation.HtmlCache.DB;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.Pipelines;
using Sitecore.Sites;

namespace Foundation.HtmlCache.Pipelines
{
    public class InitializePersistedHtmlCache
    {
        public void Initialize(PipelineArgs args)
        {
            using (var ctx = new ItemTrackingProvider())
            {
                foreach (var cacheSiteLang in ctx.CacheSites)
                {
                    SiteContext siteContext = Factory.GetSite(cacheSiteLang.SiteName);
                    foreach (var cacheHtml in cacheSiteLang.CacheHtmls)
                    {
                        CacheManager.GetHtmlCache(siteContext).SetHtml(cacheHtml.HtmlCacheKey, cacheHtml.HtmlCacheResult);
                    }
                }
            }

        }
    }
}