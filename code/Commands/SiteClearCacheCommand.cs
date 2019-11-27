using System;
using System.Linq;
using Foundation.HtmlCache.Bus;
using Foundation.HtmlCache.Extensions;
using Foundation.HtmlCache.Models;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Framework.Messaging;
using Sitecore.Globalization;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web;

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

            Guid? itemId = this.item?.ID.Guid;
            string languageStr = language.Name;
            if (itemId != null && !string.IsNullOrEmpty(languageStr))
            {
                Language language = Language.Parse(languageStr);
                Item item = Factory.GetDatabase("web").GetItem(ID.Parse(itemId), language);
                if (item != null)
                {
                    SiteInfo siteInfo = SiteInfoExtensions.GetSites(item, language).FirstOrDefault();
                    if (siteInfo != null)
                    {
                        ((IMessageBus<HtmlCacheMessageBus>)ServiceLocator.ServiceProvider.GetService(
                            typeof(IMessageBus<HtmlCacheMessageBus>))).Send(new DeleteSiteFromCache(siteInfo.Name, siteInfo.Language));
                    }
                }
            }

        }
    }
}