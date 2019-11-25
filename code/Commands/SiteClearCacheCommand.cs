using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Foundation.HtmlCache.Events;
using Foundation.HtmlCache.Models;
using Sitecore;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace Foundation.HtmlCache.Commands
{
    public class SiteClearCacheCommand : Command
    {
        private Item item { get; set; }
        private Language language { get; set; }
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, nameof(context));
            this.item = context.Items.FirstOrDefault();
            this.language = this.item.Language;
            Context.ClientPage.Start(this, "Run");
        }

        protected void Run(ClientPipelineArgs args)
        {
            ClearCacheArgs remoteEvent = new ClearCacheArgs("cache:clearCacheSite:Remote", this.item?.ID.Guid, language.Name, ClearCacheOperation.ClearCacheOperationEnum.Site);
            Factory.GetDatabase("web").RemoteEvents.Queue.QueueEvent(remoteEvent, true, true);

            SheerResponse.Alert("All caches for all sites have been cleared", true);
            args.WaitForPostBack(false);
        }
    }
}