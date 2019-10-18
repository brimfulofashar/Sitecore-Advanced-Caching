using System.Linq;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using SitecoreAdvancedCaching.Events;
using SitecoreAdvancedCaching.Models;

namespace SitecoreAdvancedCaching.Commands
{
    public class SiteClearCacheCommand : Command
    {
        private Item item { get; set; }
        private Language language { get; set; }

        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, nameof(context));
            item = context.Items.FirstOrDefault();
            language = item.Language;
            Context.ClientPage.Start(this, "Run");
        }

        protected void Run(ClientPipelineArgs args)
        {
            var remoteEvent = new ClearCacheArgs("cache:clearCacheSite:Remote", item?.ID.Guid, language.Name,
                ClearCacheOperation.ClearCacheOperationEnum.Site);
            Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);

            SheerResponse.Alert("All caches for all sites have been cleared", true);
            args.WaitForPostBack(false);
        }
    }
}