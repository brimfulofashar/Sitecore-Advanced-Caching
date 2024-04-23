using System.Web;
using Foundation.HtmlCache.Arguments;
using Foundation.HtmlCache.Helpers;
using Foundation.HtmlCache.Models;
using Sitecore.Caching;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Foundation.HtmlCache.Pipelines
{
    public class EndRenderingContext : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            var renderingProcessorArgs = (RenderingProcessorArgs)HttpContext.Current.Items[RenderingProcessorArgs.Key];
            if (renderingProcessorArgs.TrackOperationEnum == TrackOperation.TrackOperationEnum.Track)
            {
                renderingProcessorArgs.CacheResult = ((RecordingTextWriter) args.Writer).GetRecording();

                var tvpHelper = HttpContext.Current.Items[TVPHelper.HttpContextKey] as TVPHelper;
                tvpHelper?.Tracker.Add(args.CacheKey, renderingProcessorArgs);
            }

            renderingProcessorArgs.TrackOperationEnum = TrackOperation.TrackOperationEnum.DoNotTrack;
            HttpContext.Current.Items[RenderingProcessorArgs.Key] = renderingProcessorArgs;
        }
    }
}