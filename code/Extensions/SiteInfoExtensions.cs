using System.Collections.Generic;
using Sitecore.Globalization;
using Sitecore.Web;

namespace Foundation.HtmlCache.Extensions
{
    public static class SiteInfoExtensions
    {
        public static List<SiteInfo> GetSites(Sitecore.Data.Items.Item item, Language language = null)
        {
            List<SiteInfo> siteInfos = new List<SiteInfo>();
            if (item != null)
            {
                List<SiteInfo> siteInfoList = Sitecore.Configuration.Factory.GetSiteInfoList();

                foreach (SiteInfo siteInfo in siteInfoList)
                {
                    if (item.Paths.FullPath.ToLower().Trim().StartsWith(siteInfo.RootPath.ToLower().Trim()) &&
                        siteInfo.Domain != "sitecore" && siteInfo.Domain != "extranet" &&
                        (language == null || siteInfo.Language == language.Name))
                    {
                        siteInfos.Add(siteInfo);
                    }
                }

                
            }

            return siteInfos;
        }
    }
}