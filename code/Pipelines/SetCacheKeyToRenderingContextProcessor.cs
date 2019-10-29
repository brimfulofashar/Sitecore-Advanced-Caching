using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Foundation.HtmlCache.Pipelines
{
    public class SetCacheKeyToRenderingContextProcessor : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            if (!string.IsNullOrEmpty(args.CacheKey))
            {
                if (HttpContext.Current.Items["CacheKey"] == null)
                {
                    HttpContext.Current.Items.Add("CacheKey", args.CacheKey);
                }
                else
                {
                    HttpContext.Current.Items["CacheKey"] = args.CacheKey;
                }
            }
            else
            {
                if (HttpContext.Current.Items["CacheKey"] != null)
                {
                    HttpContext.Current.Items.Remove("CacheKey");
                }
            }
        }
    }
}