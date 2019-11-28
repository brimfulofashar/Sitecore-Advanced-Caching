using Foundation.HtmlCache.Handlers;
using Foundation.HtmlCache.Messages;
using Foundation.HtmlCache.Models;
using Sitecore.Framework.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Foundation.HtmlCache.Configuration
{
    public class ServicesConfiguratorCm : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IMessageHandler<AddToCacheStore>, AddToCacheStoreBusMessageHandler>();
        }
    }
}