using Foundation.HtmlCache.Messaging.Bus;
using Foundation.HtmlCache.Messaging.Message;
using Sitecore.DependencyInjection;
using Sitecore.Framework.Messaging;

namespace Foundation.HtmlCache.Messaging.Repository
{
    public class BroadcastHtmlCacheRepository
    {
        private readonly IMessageBus<BroadcastHtmlCacheBus> _messageBus;
        public BroadcastHtmlCacheRepository()
        {
            this._messageBus =
                (IMessageBus<BroadcastHtmlCacheBus>) ServiceLocator.ServiceProvider.GetService(
                    typeof(IMessageBus<BroadcastHtmlCacheBus>));
        }

        public void BroadcastMessage(BroadcastHtmlCacheMessage message)
        {
            _messageBus.PublishAsync(message);
        }
    }
}