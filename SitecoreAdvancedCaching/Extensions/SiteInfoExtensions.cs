using System.Collections.Generic;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Web;

namespace SitecoreAdvancedCaching.Extensions
{
    public static class SiteInfoExtensions
    {
        public static List<SiteInfo> GetSites(Item item, Language language = null)
        {
            var siteInfoList = Factory.GetSiteInfoList();

            var siteInfos = new List<SiteInfo>();

            foreach (var siteInfo in siteInfoList)
                if (item.Paths.FullPath.ToLower().Trim().StartsWith(siteInfo.RootPath.ToLower().Trim()) &&
                    siteInfo.Domain != "sitecore" && siteInfo.Domain != "extranet" &&
                    (language == null || siteInfo.Language == language.Name))
                    siteInfos.Add(siteInfo);

            return siteInfos;
        }
    }
}