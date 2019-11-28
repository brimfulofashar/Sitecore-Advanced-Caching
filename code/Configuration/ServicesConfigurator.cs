using Foundation.HtmlCache.Handlers;
using Foundation.HtmlCache.Messages;
using Foundation.HtmlCache.Models;
using Sitecore.Framework.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Foundation.HtmlCache.Configuration
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IMessageHandler<AddToCache>, AddToCacheBusMessageHandler>();
            serviceCollection.AddTransient<IMessageHandler<DeleteFromCache>, DeleteFromCacheBusMessageHandler>();
            serviceCollection.AddTransient<IMessageHandler<DeleteSiteFromCache>, DeleteSiteFromCacheBusMessageHandler>();
            serviceCollection.AddTransient<IMessageHandler<BroadcastCache>, BroadcastCacheBusMessageHandler>();
        }
    }
}