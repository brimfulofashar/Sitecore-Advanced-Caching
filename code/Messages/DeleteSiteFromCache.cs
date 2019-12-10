using Foundation.HtmlCache.Providers;

namespace Foundation.HtmlCache.Messages
{
    public class DeleteSiteFromCache : SiteMetaData, ICacheMessage
    {
        public DeleteSiteFromCache(string siteInfoName, string siteInfoLanguage) : base(siteInfoName, siteInfoLanguage)
        {
        }
    }
}