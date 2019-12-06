using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundation.HtmlCache.Messages;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Providers;
using Newtonsoft.Json;
using Sitecore;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using StackExchange.Redis;

namespace Foundation.HtmlCache.Pipelines
{
    public class EndRenderingContext : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            var renderingProcessorArgs = (RenderingProcessorArgs)HttpContext.Current.Items["RenderingArgs"];
            if (renderingProcessorArgs.TrackOperationEnum == TrackOperation.TrackOperationEnum.Track)
            {
                renderingProcessorArgs.CacheResult = ((RecordingTextWriter) args.Writer).GetRecording();
                var addToCache = new AddToCache(Context.Site.SiteInfo.Name, Context.Site.SiteInfo.Language, renderingProcessorArgs);
                IRedisCacheProvider redis = DependencyResolver.Current.GetServices<IRedisCacheProvider>().FirstOrDefault();

                var json = JsonConvert.SerializeObject(addToCache, Formatting.Indented,
                    new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All});

                redis?.Database.ListLeftPush("AddToCache", json);
                redis?.Publish(addToCache.SiteInfoName + "_" + addToCache.SiteInfoLanguage, "");
            }

            renderingProcessorArgs.TrackOperationEnum = TrackOperation.TrackOperationEnum.DoNotTrack;
            HttpContext.Current.Items["RenderingArgs"] = renderingProcessorArgs;
        }
    }
}