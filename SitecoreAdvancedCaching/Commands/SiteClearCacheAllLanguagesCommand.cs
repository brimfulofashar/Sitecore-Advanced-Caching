using System.Linq;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using SitecoreAdvancedCaching.Events;
using SitecoreAdvancedCaching.Models;

namespace SitecoreAdvancedCaching.Commands
{
    public class SiteClearCacheAllLanguagesCommand : Command
    {
        private Item Item { get; set; }

        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, nameof(context));
            Item = context.Items.FirstOrDefault();
            Context.ClientPage.Start(this, "Run");
        }

        protected void Run(ClientPipelineArgs args)
        {
            var remoteEvent = new ClearCacheArgs("cache:clearCacheSiteAllLanguages:Remote", Item?.ID.Guid, string.Empty,
                ClearCacheOperation.ClearCacheOperationEnum.SiteAllLanguages);
            Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);

            SheerResponse.Alert("Caches for the Site in all languages have been cleared", true);
            args.WaitForPostBack(false);
        }
    }
}