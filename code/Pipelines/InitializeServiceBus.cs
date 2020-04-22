using System;
using Foundation.HtmlCache.Messaging.Bus;
using Sitecore.Framework.Messaging;
using Sitecore.Pipelines;

namespace Foundation.HtmlCache.Pipelines
{
    public class InitializeServiceBus
    {
        private readonly IServiceProvider serviceProvider;

        public InitializeServiceBus(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Process(PipelineArgs args)
        {
            this.serviceProvider.StartMessageBus<BroadcastHtmlCacheBus>();
        }
    }
}