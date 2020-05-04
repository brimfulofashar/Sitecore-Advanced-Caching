using System.Linq;
using Foundation.HtmlCache.DB;
using Foundation.HtmlCache.Extensions;
using Foundation.HtmlCache.Messaging.Message;
using Foundation.HtmlCache.Messaging.Repository;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
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
                var siteInfos = SiteInfoExtensions.GetSites(this.Item, null);
                if (siteInfos != null)
                {
                    BroadcastHtmlCacheRepository broadcastHtmlCacheRepository = new BroadcastHtmlCacheRepository();
                    foreach (var siteInfo in siteInfos)
                    {
                        var results = ctx.UspDeleteCacheDataForSite(siteInfo.Name, siteInfo.Language);
                        foreach (var result in results)
                        {
                            broadcastHtmlCacheRepository.BroadcastMessage(new BroadcastHtmlCacheMessage
                            {
                                ToRemove = true,
                                SiteName = result.SiteName,
                                SiteLang = result.SiteLang
                            });
                        }
                    }

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