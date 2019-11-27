using System.Web;
using Foundation.HtmlCache.Bus;
using Foundation.HtmlCache.Models;
using Sitecore;
using Sitecore.DependencyInjection;
using Sitecore.Framework.Messaging;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Foundation.HtmlCache.Pipelines
{
    public class EndRenderingContextProcessor : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            var renderingProcessorArgs = (RenderingProcessorArgs)HttpContext.Current.Items["RenderingArgs"];
            if (renderingProcessorArgs.TrackOperationEnum == TrackOperation.TrackOperationEnum.Track)
            {
                var addToCache = new AddToCache(Context.Site.SiteInfo.Name, Context.Site.SiteInfo.Language, renderingProcessorArgs);
                ((IMessageBus<HtmlCacheMessageBus>)ServiceLocator.ServiceProvider.GetService(typeof(IMessageBus<HtmlCacheMessageBus>))).Send(addToCache);
            }

            renderingProcessorArgs.TrackOperationEnum = TrackOperation.TrackOperationEnum.DoNotTrack;
            HttpContext.Current.Items["RenderingArgs"] = renderingProcessorArgs;
        }
    }
}