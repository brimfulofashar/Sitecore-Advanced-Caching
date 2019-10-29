using Sitecore.Web;

namespace Foundation.HtmlCache.Models
{
    public class DeleteSiteFromCache : CacheJobBase, ICacheJob
    {
        public DeleteSiteFromCache(SiteInfo siteInfo) : base(siteInfo)
        {
        }
    }
}