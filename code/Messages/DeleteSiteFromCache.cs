using Foundation.HtmlCache.Models;

namespace Foundation.HtmlCache.Messages
{
    public class DeleteSiteFromCache : SiteMetaData, ICacheJob
    {
        public DeleteSiteFromCache(string siteInfoName, string siteInfoLanguage) : base(siteInfoName, siteInfoLanguage)
        {
        }
    }
}