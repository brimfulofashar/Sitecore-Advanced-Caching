using System;

namespace Foundation.HtmlCache.Messages
{
    public class DeleteFromCache : SiteMetaData, ICacheMessage
    {
        public DeleteFromCache(string siteInfoName, string siteInfoLanguage, Guid itemId) : base(siteInfoName, siteInfoLanguage)
        {
            ItemId = itemId;
        }

        public Guid ItemId { get; set; }
        public void Handle()
        {
            
        }
    }
}