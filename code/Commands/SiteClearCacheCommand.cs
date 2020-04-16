using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            if (item != null && language != null)
            {
                using (var ctx = new ItemTrackingProvider())
                {
                    var siteInfo = SiteInfoExtensions.GetSites(this.item, item.Language).FirstOrDefault();
                    if (siteInfo != null)
                    {
                        ctx.UspDeleteCacheDataForSite(siteInfo.Name, siteInfo.Language);

                        SheerResponse.Alert("All caches for all sites have been queued for clearing", true);
                    }
                    else
                    {
                        SheerResponse.Alert("Site could not be determined by the item in context", true);
                    }
                }
            }
            else
            {
                SheerResponse.Alert("Site could not be determined by the item in context", true);
            }
            args.WaitForPostBack(false);
        }
    }
}