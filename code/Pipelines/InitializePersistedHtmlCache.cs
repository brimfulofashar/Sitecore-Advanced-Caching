using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Foundation.HtmlCache.Providers;
using Sitecore.ContentSearch.Linq;
using Sitecore.Data.Items;
using Sitecore.Pipelines;

namespace Foundation.HtmlCache.Pipelines
{
    public class InitializePersistedHtmlCache
    {
        public void Initialize(PipelineArgs args)
        {
            using (var context = Sitecore.ContentSearch.ContentSearchManager.GetIndex("sitecore_cache_index").CreateSearchContext())
            {
                var searchResult = context.GetQueryable<Item>()
                    .Where(item => item["_path"].Contains("/sitecore/system/Modules/HtmlCache"))
                    .Where(item => item["_templatename"] == "CacheStore")
                    .GetResults();

                foreach (var result in searchResult)
                {
                    Item item = result.Document;
                    ItemTrackingStore.Instance.PersistedHtmlCache.Add(item.Fields["CacheKey"].Value,
                        new KeyValuePair<string, string>(
                            item.Fields["RenderingId"].Value,
                            item.Fields["CachedHtml"].Value));
                }
            }
        }
    }
}