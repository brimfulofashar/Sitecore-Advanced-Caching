using System.Collections.Generic;
using System.Linq;
using Foundation.HtmlCache.DB;
using Foundation.HtmlCache.Extensions;
using Sitecore;
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
            using (var ctx = new ItemTrackingProvider())
            {
                var siteInfo = SiteInfoExtensions.GetSites(this.item).FirstOrDefault();
                if (siteInfo != null)
                {
                    var cacheQueue = new CacheQueue
                    {

                        CacheQueueMessageTypeId = (int) MessageTypeEnum.DeleteSiteFromCacheAllLanguages,
                        CacheTemps = new List<CacheTemp>()
                        {
                            new CacheTemp()
                            {
                                SiteName = siteInfo.Name,
                                SiteLang = language.Name
                            }
                        }
                    };
                    ctx.CacheQueues.Add(cacheQueue);
                    ctx.SaveChanges();
                    SheerResponse.Alert("All caches for all sites have been queued for clearing", true);
                }
                else
                {
                    SheerResponse.Alert("Site could not be determined by the item in context", true);
                }
            }

            args.WaitForPostBack(false);
        }
    }
}