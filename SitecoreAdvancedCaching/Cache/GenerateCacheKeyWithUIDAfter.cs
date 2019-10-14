using System.Linq;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using SitecoreAdvancedCaching.Providers;

namespace SitecoreAdvancedCaching.Cache
{
    public class GenerateCacheKeyWithUIDAfter : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));
            if (!args.Rendered)
                return;
            var cacheKey = args.CacheKey;
            if (!args.Cacheable || string.IsNullOrEmpty(cacheKey))
                return;
            if (cacheKey.Contains("_#aids:"))
                return;
            if (!ItemAccessTracker.Instance.ItemIdKey_RenderingIDsValue_Dic.ContainsKey(args.Rendering.UniqueId.ToString()))
                return;
            cacheKey += "_#aids:" + string.Join("|",
                            ItemAccessTracker.Instance
                                .ItemIdKey_RenderingIDsValue_Dic[args.Rendering.UniqueId.ToString()]
                                .Select(x => x.ToString()).OrderBy(x => x));
            args.CacheKey = cacheKey;
        }
    }
}