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
                foreach (var cacheSiteLang in ctx.CacheSiteLangs)
                {
                    SiteContext siteContext = Factory.GetSite(cacheSiteLang.Name);
                    foreach (var cacheKey in cacheSiteLang.CacheKeys)
                    {
                        CacheManager.GetHtmlCache(siteContext).SetHtml(cacheKey.HtmlCacheKey, cacheKey.HtmlCacheResult);
                    }
                }
            }

        }
    }
}