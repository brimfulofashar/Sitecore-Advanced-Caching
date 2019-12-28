using System.Web;
using Foundation.HtmlCache.Arguments;
using Foundation.HtmlCache.Models;
using Sitecore.Mvc.Common;
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
                renderingProcessorArgs.CacheResult = ((RecordingTextWriter) args.Writer).GetRecording();
            }
        }
    }
}