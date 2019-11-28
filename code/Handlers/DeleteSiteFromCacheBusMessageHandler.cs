using Foundation.HtmlCache.Messages;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Providers;
using Sitecore.Framework.Messaging;
using Task = System.Threading.Tasks.Task;

namespace Foundation.HtmlCache.Handlers
{
    public class DeleteSiteFromCacheBusMessageHandler : IMessageHandler<DeleteSiteFromCache>
    {
        public Task Handle(DeleteSiteFromCache message, IMessageReceiveContext receiveContext, IMessageReplyContext replyContext)
        {
            ItemTrackingStore.Instance.DeleteSite(message.SiteInfoName, message.SiteInfoLanguage);
            return Task.CompletedTask;
        }
    }
}