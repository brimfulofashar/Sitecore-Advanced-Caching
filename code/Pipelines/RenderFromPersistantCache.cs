using Foundation.HtmlCache.Providers;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Foundation.HtmlCache.Pipelines
{
    public class RenderFromPersistantCache : RenderRenderingProcessor
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
                    args.CustomData.Add("UsedPersistedCache", true);
                }
            }
        }
    }
}