using System.Linq;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using Sitecore.Mvc.Presentation;
using SitecoreAdvancedCaching.Providers;

namespace SitecoreAdvancedCaching.Cache
{
    public class GenerateCacheKeyWithUID : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            Assert.ArgumentNotNull((object)args, nameof(args));
            if (!args.Rendered)
                return;
            string cacheKey = args.CacheKey;
            if (!args.Cacheable || string.IsNullOrEmpty(cacheKey))
                return;
            cacheKey += "_#aids:" + string.Join("|", ItemAccessTracker.Instance.ItemIdKey_RenderingIDsValue_Dic.Where(x => x.Value.Contains(args.Rendering.UniqueId.ToString())).Select(x => x.Key));
            args.CacheKey = cacheKey;
        }
    }
}