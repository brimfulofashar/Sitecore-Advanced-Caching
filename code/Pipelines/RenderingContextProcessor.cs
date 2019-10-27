using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Foundation.HtmlCache.Pipelines
{
    public class RenderingContextProcessor : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            if (HttpContext.Current.Items["Rendering"] == null)
            {
                HttpContext.Current.Items.Add("Rendering", args.Rendering);
            }
            else
            {
                HttpContext.Current.Items["Rendering"] = args.Rendering;
            }
        }
    }
}