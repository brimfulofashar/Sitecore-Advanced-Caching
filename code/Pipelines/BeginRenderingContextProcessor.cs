using System.Web;
using Foundation.HtmlCache.Models;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Foundation.HtmlCache.Pipelines
{
    public class BeginRenderingContextProcessor : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            RenderingProcessorArgs dic;
            if (args.Rendering.RenderingType == "r" && !args.UsedCache && args.Cacheable && !string.IsNullOrEmpty(args.CacheKey))
            {
                dic = new RenderingProcessorArgs(args.CacheKey, args.Rendering, args.PageContext.Item, TrackOperation.TrackOperationEnum.Track);
            }
            else
            {
                dic = new RenderingProcessorArgs(TrackOperation.TrackOperationEnum.DoNotTrack);
            }
            HttpContext.Current.Items["RenderingArgs"] = dic;
        }
    }
}