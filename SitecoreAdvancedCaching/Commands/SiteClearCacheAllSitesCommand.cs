using System;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using SitecoreAdvancedCaching.Events;
using SitecoreAdvancedCaching.Models;

namespace SitecoreAdvancedCaching.Commands
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
            var remoteEvent = new ClearCacheArgs("cache:clearCacheAllSites:Remote", Guid.Empty, string.Empty,
                ClearCacheOperation.ClearCacheOperationEnum.AllSites);
            Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);

            SheerResponse.Alert("Cache for the Site has been cleared", true);
            args.WaitForPostBack(false);
        }
    }
}