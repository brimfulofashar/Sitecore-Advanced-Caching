using Sitecore.Framework.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Foundation.HtmlCache.Messaging.Handlers;
using Foundation.HtmlCache.Messaging.Message;

namespace Foundation.HtmlCache.Messaging.Subscription
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IMessageHandler<BroadcastHtmlCacheMessage>, BroadcastHtmlCacheHandler>();
        }
    }
}