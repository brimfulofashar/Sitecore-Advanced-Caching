using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Foundation.HtmlCache.Providers;
using Sitecore;
using Sitecore.Caching;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using Sitecore.Sites;

namespace Foundation.HtmlCache.Pipelines
{
    public class RenderFromPersistantCache : AddRecordedHtmlToCache
    {
        public override void Process(RenderRenderingArgs args)
        {
            if (args.Rendering.RenderingType == "r" && !args.UsedCache && args.Cacheable && !string.IsNullOrEmpty(args.CacheKey) && !args.Rendering.Caching.VaryByUser)
            {
                ItemTrackingStore.Instance.PersistedHtmlCache.TryGetValue(args.CacheKey, out var renderingItemHtmlValuePair);

                if (!string.IsNullOrEmpty(renderingItemHtmlValuePair.Value))
                {
                    args.Writer.Write(renderingItemHtmlValuePair.Value);
                    args.Rendered = true;
                    args.UsedCache = true;
                    args.Cacheable = false;
                    AddHtmlToCache(args.CacheKey, renderingItemHtmlValuePair.Value, args);
                }
            }
        }
    }
}