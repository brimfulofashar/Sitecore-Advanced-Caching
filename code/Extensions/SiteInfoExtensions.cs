﻿using System.Collections.Generic;
using Sitecore.Globalization;
using Sitecore.Web;

namespace Foundation.HtmlCache.Extensions
{
    public static class SiteInfoExtensions
    {
        public static List<SiteInfo> GetSites(Sitecore.Data.Items.Item item, Language language)
        {
            List<SiteInfo> siteInfos = new List<SiteInfo>();
            if (item != null)
            {
                List<SiteInfo> siteInfoList = Sitecore.Configuration.Factory.GetSiteInfoList();

                foreach (SiteInfo siteInfo in siteInfoList)
                {
                    if (item.Paths.FullPath.ToLower().Trim().StartsWith(siteInfo.RootPath.ToLower().Trim()) &&
                        !string.IsNullOrEmpty(siteInfo.HostName) && siteInfo.CacheHtml &&
                        (language == null || siteInfo.Language == language.Name || string.IsNullOrEmpty(siteInfo.Language)))
                    {
                        siteInfos.Add(siteInfo);
                    }
                }
            }
            else
            {
                siteInfos = Sitecore.Configuration.Factory.GetSiteInfoList();
            }

            return siteInfos;
        }
    }
}