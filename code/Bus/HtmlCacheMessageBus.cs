using Foundation.HtmlCache.Messages;
using Sitecore.DependencyInjection;
using Sitecore.Framework.Messaging;

namespace Foundation.HtmlCache.Bus
{
    public sealed class HtmlCacheMessageBus
    {
        public static void Send(ICacheMessage cacheMessage)
        {
            ((IMessageBus<HtmlCacheMessageBus>)ServiceLocator.ServiceProvider.GetService(typeof(IMessageBus<HtmlCacheMessageBus>))).Send(cacheMessage);
        }

        public static void Publish(ICacheMessage cacheMessage)
        {
            ((IMessageBus<HtmlCacheMessageBus>)ServiceLocator.ServiceProvider.GetService(typeof(IMessageBus<HtmlCacheMessageBus>))).Publish(cacheMessage);
        }

    }
}