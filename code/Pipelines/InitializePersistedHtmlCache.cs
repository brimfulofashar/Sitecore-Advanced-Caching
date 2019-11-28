using System.Collections.Generic;
using System.Linq;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Providers;
using Sitecore.ContentSearch.Linq;
using Sitecore.Pipelines;

namespace Foundation.HtmlCache.Pipelines
{
    public class InitializePersistedHtmlCache
    {
        public void Initialize(PipelineArgs args)
        {
            using (var context = Sitecore.ContentSearch.ContentSearchManager.GetIndex("sitecore_cache_index").CreateSearchContext())
            {
                var searchResult = context.GetQueryable<CacheStoreSearchResult>()
                    .Where(item => item.Path.Contains("/sitecore/system/Modules/HtmlCache"))
                    .Where(item => item.TemplateName == "CacheStore")
                    .GetResults();

                foreach (var result in searchResult)
                {
                    CacheStoreSearchResult item = result.Document;
                    ItemTrackingStore.Instance.PersistedHtmlCache.Add(item.CacheKey,
                        new KeyValuePair<string, string>(
                            item.RenderingId,
                            item.CachedHtml));
                }
            }
        }
    }
}