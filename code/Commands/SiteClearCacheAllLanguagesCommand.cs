using System.Collections.Generic;
using System.Linq;
using Foundation.HtmlCache.DB;
using Foundation.HtmlCache.Events;
using Foundation.HtmlCache.Extensions;
using Foundation.HtmlCache.Models;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;

namespace Foundation.HtmlCache.Commands
{
    public class SiteClearCacheAllLanguagesCommand : Command
    {
        private Item Item { get; set; }

        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, nameof(context));
            this.Item = context.Items.FirstOrDefault();
            Context.ClientPage.Start(this, "Run");
        }

        protected void Run(ClientPipelineArgs args)
        {
            using (var ctx = new ItemTrackingProvider())
            {
                var siteInfos = SiteInfoExtensions.GetSites(this.Item);
                if (siteInfos != null)
                {
                    foreach (var siteInfo in siteInfos)
                    {
                        var cacheQueue = new CacheQueue
                        {
                            CacheQueueMessageTypeId = (int) MessageTypeEnum.DeleteSiteFromCacheAllLanguages,
                            Site = siteInfo.Name,
                            Language = siteInfo.Language

                        };
                        ctx.CacheQueues.Add(cacheQueue);
                    }

                    ctx.SaveChanges();

                    SheerResponse.Alert("Caches for the Site in all languages have been queue to be cleared", true);
                }
                else
                {
                    SheerResponse.Alert("Site could not be determined by the item in context", true);
                }

                args.WaitForPostBack(false);
            }

        }
    }
}