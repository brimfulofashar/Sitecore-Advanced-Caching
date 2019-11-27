using Foundation.HtmlCache.Bus;
using Sitecore.DependencyInjection;
using Sitecore.Pipelines;
using Sitecore.Framework.Messaging;

namespace Foundation.HtmlCache.Pipelines
{
    public class InitializeMessageBus
    {
        public void Initialize(PipelineArgs args)
        {
            ServiceLocator.ServiceProvider.StartMessageBus<HtmlCacheMessageBus>();
        }
    }
}