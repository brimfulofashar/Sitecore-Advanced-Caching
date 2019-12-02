using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundation.HtmlCache.Messages;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Providers;
using Sitecore;

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
                IRedisCacheProvider redis = DependencyResolver.Current.GetServices<IRedisCacheProvider>().FirstOrDefault();
                if (redis != null)
                {
                    redis.Set(args.CacheKey, addToCache,
                        args.Rendering.Caching.Timeout.TotalMilliseconds);
                    redis.Publish(Context.Site.Name + "_" + Context.Site.Language, "AddToCache");
                }
            }

            renderingProcessorArgs.TrackOperationEnum = TrackOperation.TrackOperationEnum.DoNotTrack;
            HttpContext.Current.Items["RenderingArgs"] = renderingProcessorArgs;
        }
    }
}