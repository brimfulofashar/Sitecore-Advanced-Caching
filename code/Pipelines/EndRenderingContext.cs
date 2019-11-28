using System.Web;
using Foundation.HtmlCache.Bus;
using Foundation.HtmlCache.Messages;
using Foundation.HtmlCache.Models;
using Sitecore;
using Sitecore.DependencyInjection;
using Sitecore.Framework.Messaging;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Foundation.HtmlCache.Pipelines
{
    public class EndRenderingContext : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            var renderingProcessorArgs = (RenderingProcessorArgs)HttpContext.Current.Items["RenderingArgs"];
            if (renderingProcessorArgs.TrackOperationEnum == TrackOperation.TrackOperationEnum.Track)
            {
                var addToCache = new AddToCache(Context.Site.SiteInfo.Name, Context.Site.SiteInfo.Language, renderingProcessorArgs);
                ((IMessageBus<HtmlCacheMessageBusSend>)ServiceLocator.ServiceProvider.GetService(typeof(IMessageBus<HtmlCacheMessageBusSend>))).Send(addToCache);
            }

            renderingProcessorArgs.TrackOperationEnum = TrackOperation.TrackOperationEnum.DoNotTrack;
            HttpContext.Current.Items["RenderingArgs"] = renderingProcessorArgs;
        }
    }
}