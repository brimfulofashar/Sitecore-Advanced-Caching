using System;
using System.Collections.Generic;
using System.Linq;
using Foundation.HtmlCache.DB;
using Foundation.HtmlCache.Events;
using Foundation.HtmlCache.Extensions;
using Foundation.HtmlCache.Models;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace Foundation.HtmlCache.Commands
{
    public class SiteClearCacheAllSitesCommand : Command
    {
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, nameof(context));
            Context.ClientPage.Start(this, "Run");
        }

        protected void Run(ClientPipelineArgs args)
        {
            using (var ctx = new ItemTrackingProvider())
            {
                var cacheQueue = new CacheQueue
                {
                    CacheQueueMessageTypeId = (int)MessageTypeEnum.DeleteSiteFromCacheAllSites,
                };
                ctx.CacheQueues.Add(cacheQueue);
                ctx.SaveChanges();
            }

            SheerResponse.Alert("Cache for the Site has been cleared", true);
            args.WaitForPostBack(false);
        }
    }
}