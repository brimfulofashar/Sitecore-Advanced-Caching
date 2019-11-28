using Foundation.HtmlCache.Messages;
using Foundation.HtmlCache.Providers;
using Sitecore.Framework.Messaging;
using Task = System.Threading.Tasks.Task;

namespace Foundation.HtmlCache.Handlers
{
    public class DeleteFromCacheBusMessageHandler : IMessageHandler<DeleteFromCache>
    {
        public Task Handle(DeleteFromCache message, IMessageReceiveContext receiveContext, IMessageReplyContext replyContext)
        {
            ItemTrackingStore.Instance.Delete(message.SiteInfoName, message.SiteInfoLanguage, message.ItemId);
            return Task.CompletedTask;
        }
    }
}