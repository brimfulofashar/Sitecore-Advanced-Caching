using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace Foundation.HtmlCache.Models
{
    public class CacheStoreSearchResult : SearchResultItem
    {
        [IndexField("cachekey_t")] public virtual string CacheKey { get; set; }

        [IndexField("renderingid_t")] public virtual string RenderingId { get; set; }

        [IndexField("cachedhtml_t")] public virtual string CachedHtml { get; set; }
    }
}