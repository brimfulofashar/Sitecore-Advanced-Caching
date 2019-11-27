namespace Foundation.HtmlCache.Models
{
    public class DeleteSiteFromCache : SiteMetaData, ICacheJob
    {
        public DeleteSiteFromCache(string siteInfoName, string siteInfoLanguage) : base(siteInfoName, siteInfoLanguage)
        {
        }
    }
}