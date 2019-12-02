﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Foundation.HtmlCache.Messages;
using Foundation.HtmlCache.Providers;
using Sitecore.Configuration;
using Sitecore.Diagnostics;

using Sitecore.Shell.Framework.Commands;
using Sitecore.Web;

namespace Foundation.HtmlCache.Commands
{
    public class SiteClearCacheAllSitesCommand : Command
    {
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, nameof(context));
            List<SiteInfo> siteInfos = Factory.GetSiteInfoList();
            foreach (SiteInfo siteInfo in siteInfos)
            {
                IRedisCacheProvider redis = DependencyResolver.Current.GetServices<IRedisCacheProvider>().FirstOrDefault();
                redis?.Set("SiteClearCache", new DeleteSiteFromCache(siteInfo.Name, siteInfo.Language), 0);
                redis?.Publish(siteInfo.Name + "_" + siteInfo.Language, "SiteClearCache");
            }
        }
    }
}