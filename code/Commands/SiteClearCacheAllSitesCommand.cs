using System.Collections.Generic;
using Foundation.HtmlCache.Bus;
using Foundation.HtmlCache.Messages;
using Foundation.HtmlCache.Models;
using Sitecore.Configuration;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Framework.Messaging;
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
                ((IMessageBus<HtmlCacheMessageBus>)ServiceLocator.ServiceProvider.GetService(
                    typeof(IMessageBus<HtmlCacheMessageBus>))).Publish(new DeleteSiteFromCache(siteInfo.Name, siteInfo.Language));
            }
        }
    }
}