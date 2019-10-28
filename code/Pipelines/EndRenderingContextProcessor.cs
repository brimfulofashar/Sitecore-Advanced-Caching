using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace ProDirect.Foundation.HtmlCache.Pipelines
{
    public class EndRenderingContextProcessor : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            if (HttpContext.Current.Items["Rendering"] != null)
            {
                HttpContext.Current.Items.Remove("Rendering");
            }
        }
    }
}