﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Globalization;
using Sitecore.Web;

namespace Foundation.HtmlCache.Extensions
{
    public static class SiteInfoExtensions
    {
        public static List<Sitecore.Web.SiteInfo> GetSites(Sitecore.Data.Items.Item item, Language language = null)
        {
            var siteInfoList = Sitecore.Configuration.Factory.GetSiteInfoList();

            List<Sitecore.Web.SiteInfo> siteInfos = new List<SiteInfo>();

            foreach (Sitecore.Web.SiteInfo siteInfo in siteInfoList)
            {
                if (item.Paths.FullPath.ToLower().Trim().StartsWith(siteInfo.RootPath.ToLower().Trim()) && siteInfo.Domain != "sitecore" && siteInfo.Domain != "extranet" && (language == null || siteInfo.Language == language.Name))
                {
                    siteInfos.Add(siteInfo);
                }
            }

            return siteInfos;
        }
    }
}