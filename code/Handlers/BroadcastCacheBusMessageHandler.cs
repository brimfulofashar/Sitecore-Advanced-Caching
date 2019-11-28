using System.Collections.Generic;
using System.Linq;
using Foundation.HtmlCache.Messages;
using Sitecore.Framework.Messaging;
using Sitecore.Web;
using Task = System.Threading.Tasks.Task;

namespace Foundation.HtmlCache.Handlers
{
    public class BroadcastCacheBusMessageHandler : IMessageHandler<BroadcastCache>
    {
        public Task Handle(BroadcastCache message, IMessageReceiveContext receiveContext, IMessageReplyContext replyContext)
        {

            List<SiteInfo> siteInfoList = Sitecore.Configuration.Factory.GetSiteInfoList();

            var matchedSiteInfo =
                siteInfoList.FirstOrDefault(x => x.Name == message.SiteInfoName && x.Language == message.SiteInfoLanguage);

            if (matchedSiteInfo != null)
            {
                matchedSiteInfo.HtmlCache.Remove(message.CacheKey);
                matchedSiteInfo.HtmlCache.SetHtml(message.CacheKey, message.CachedHtml);
            }


            return Task.CompletedTask;
        }
    }
}