using System;
using System.Collections.Generic;
using System.Linq;
using Foundation.HtmlCache.Bus;
using Foundation.HtmlCache.Extensions;
using Foundation.HtmlCache.Messages;
using Foundation.HtmlCache.Models;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Framework.Messaging;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web;

namespace Foundation.HtmlCache.Commands
{
    public class SiteClearCacheAllLanguagesCommand : Command
    {
        private Item Item { get; set; }
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, nameof(context));
            this.Item = context.Items.FirstOrDefault();

            Guid? itemId = this.Item?.ID.Guid;
            if (itemId != null)
            {
                Item item = Factory.GetDatabase("web").GetItem(ID.Parse(itemId));
                if (item != null)
                {
                    List<SiteInfo> siteInfos = SiteInfoExtensions.GetSites(item);
                    foreach (SiteInfo siteInfo in siteInfos)
                    {
                        ((IMessageBus<HtmlCacheMessageBus>) ServiceLocator.ServiceProvider.GetService(typeof(IMessageBus<HtmlCacheMessageBus>))).Publish(new DeleteSiteFromCache(siteInfo.Name, siteInfo.Language));
                    }
                }
            }

        }
    }
}