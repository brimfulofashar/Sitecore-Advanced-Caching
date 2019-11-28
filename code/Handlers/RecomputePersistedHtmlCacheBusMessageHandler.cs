using Foundation.HtmlCache.Messages;
using Sitecore.Framework.Messaging;
using Task = System.Threading.Tasks.Task;

namespace Foundation.HtmlCache.Handlers
{
    public class RecomputePersistedHtmlCacheBusMessageHandler : IMessageHandler<RecomputePersistedHtmlCache>
    {
        public Task Handle(RecomputePersistedHtmlCache message, IMessageReceiveContext receiveContext, IMessageReplyContext replyContext)
        {
            
            return Task.CompletedTask;
        }
    }
}