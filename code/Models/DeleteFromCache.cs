using Sitecore.Data;

namespace Foundation.HtmlCache.Models
{
    public class DeleteFromCache : SiteMetaData, ICacheJob
    {
        public DeleteFromCache(string siteInfoName, string siteInfoLanguage, ID itemId) : base(siteInfoName, siteInfoLanguage)
        {
            ItemId = itemId;
        }

        public ID ItemId { get; set; }
    }
}