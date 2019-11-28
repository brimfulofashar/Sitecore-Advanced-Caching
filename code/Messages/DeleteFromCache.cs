using Foundation.HtmlCache.Models;
using Sitecore.Data;

namespace Foundation.HtmlCache.Messages
{
    public class DeleteFromCache : SiteMetaData, ICacheMessage
    {
        public DeleteFromCache(string siteInfoName, string siteInfoLanguage, ID itemId) : base(siteInfoName, siteInfoLanguage)
        {
            ItemId = itemId;
        }

        public ID ItemId { get; set; }
    }
}