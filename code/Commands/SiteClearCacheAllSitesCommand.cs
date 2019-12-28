using Foundation.HtmlCache.DB;
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
                var cacheQueue = new CacheQueue
                {
                    CacheQueueMessageTypeId = (int)MessageTypeEnum.DeleteSiteFromCacheAllSites,
                };
                ctx.CacheQueues.Add(cacheQueue);
                ctx.SaveChanges();
            }

            SheerResponse.Alert("Caches for the all sites in all languages have been queue to be cleared", true);
            args.WaitForPostBack(false);
        }
    }
}