using System.Diagnostics;
using System.Web;
using Foundation.HtmlCache.Models;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Foundation.HtmlCache.Pipelines
{
    public class BeginRenderingContextProcessor : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            if (!args.UsedCache && args.Cacheable)
            {
                var dic = new RenderingProcessorArgs(args.CacheKey, args.Rendering, args.PageContext.Item);
                if (!HttpContext.Current.Items.Contains("RenderingArgs"))
                    HttpContext.Current.Items.Add("RenderingArgs", dic);
                else
                    HttpContext.Current.Items["RenderingArgs"] = dic;
            }
            else
            {
                if (HttpContext.Current.Items.Contains("RenderingArgs"))
                    HttpContext.Current.Items.Remove("RenderingArgs");
            }
        }
    }
}