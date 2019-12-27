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
                foreach (var cacheSiteLang in ctx.Caches)
                {
                    SiteContext siteContext = Factory.GetSite(cacheSiteLang.SiteName);
                    CacheManager.GetHtmlCache(siteContext).SetHtml(cacheSiteLang.HtmlCacheKey, cacheSiteLang.HtmlCacheResult);
                }
            }

        }
    }
}