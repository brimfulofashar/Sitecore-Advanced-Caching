using Sitecore.Web;

namespace ProDirect.Foundation.HtmlCache.Models
{
    public class DeleteSiteFromCache : CacheJobBase, ICacheJob
    {
        public DeleteSiteFromCache(SiteInfo siteInfo) : base(siteInfo)
        {
        }
    }
}