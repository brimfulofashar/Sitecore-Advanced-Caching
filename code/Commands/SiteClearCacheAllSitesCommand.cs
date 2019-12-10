using System.Collections.Generic;
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
                
            }
        }
    }
}