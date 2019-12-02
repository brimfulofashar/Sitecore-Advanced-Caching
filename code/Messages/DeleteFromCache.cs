using System;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Providers;
using Sitecore.Data;

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
            ItemTrackingStore.Instance.Delete(this.SiteInfoName, this.SiteInfoLanguage, this.ItemId);
        }
    }
}