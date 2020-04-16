using System.Data;
using System.Data.SqlClient;
using Foundation.HtmlCache.DB;
using Foundation.HtmlCache.Extensions;
using Sitecore;
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
                var siteInfos = SiteInfoExtensions.GetSites(null, null);
                if (siteInfos != null)
                {
                    foreach (var siteInfo in siteInfos)
                    {
                        ctx.UspDeleteCacheDataForSite(siteInfo.Name, siteInfo.Language);
                    }

                    SheerResponse.Alert("Caches for the Site in all languages have been queue to be cleared", true);
                }
            }

            SheerResponse.Alert("Caches for the all sites in all languages have been queue to be cleared", true);
            args.WaitForPostBack(false);
        }
    }
}