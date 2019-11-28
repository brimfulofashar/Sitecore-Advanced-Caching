using Foundation.HtmlCache.Messages;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Providers;
using Sitecore.Framework.Messaging;
using Task = System.Threading.Tasks.Task;

namespace Foundation.HtmlCache.Handlers
{
    public class AddToCacheBusMessageHandler : IMessageHandler<AddToCache>
    {
        public Task Handle(AddToCache message, IMessageReceiveContext receiveContext, IMessageReplyContext replyContext)
        {
            ItemTrackingStore.Instance.Add(message.SiteInfoName, message.SiteInfoLanguage, message.RenderingProcessorArgs);
            return Task.CompletedTask;
        }
    }
}