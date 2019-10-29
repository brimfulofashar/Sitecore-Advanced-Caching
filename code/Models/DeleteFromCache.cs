using Sitecore.Data;
using Sitecore.Web;

namespace Foundation.HtmlCache.Models
{
    public class DeleteFromCache : CacheJobBase, ICacheJob
    {
        public DeleteFromCache(SiteInfo siteInfo, ID itemId) : base(siteInfo)
        {
            ItemId = itemId;
        }

        public ID ItemId { get; set; }
    }
}