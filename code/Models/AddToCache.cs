using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Web;

namespace Foundation.HtmlCache.Models
{
    public class AddToCache : CacheJobBase, ICacheJob
    {
        public AddToCache(SiteInfo siteInfo, Item item, RenderingProcessorArgs renderingProcessorArgs) : base(siteInfo)
        {
            RenderingProcessorArgs = renderingProcessorArgs;
            Item = item;
        }

        public RenderingProcessorArgs RenderingProcessorArgs { get; set; }
        public Item Item { get; set; }

        public string CacheKey { get; set; }
    }

    public class CacheJobBase
    {
        public CacheJobBase(SiteInfo siteInfo)
        {
            SiteInfo = siteInfo;
        }

        public SiteInfo SiteInfo { get; set; }
    }
}