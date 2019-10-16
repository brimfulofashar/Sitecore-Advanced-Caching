using System.Linq;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using SitecoreAdvancedCaching.Providers;

namespace SitecoreAdvancedCaching.Cache
{
    public class GenerateCacheKeyWithUIDBefore : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));
            if (args.Rendered)
                return;
            var cacheKey = args.CacheKey;
            if (!args.Cacheable || string.IsNullOrEmpty(cacheKey))
                return;
            if (!ItemAccessTracker.Instance.RenderingIdKey_ItemIDsValue_Dic.ContainsKey(args.Rendering.UniqueId.ToString()))
                return;

            // this inserts the page id so that if the page is rendered, all caches using the page are cleared.
            var usedItemIds = string.Join("|",
                ItemAccessTracker.Instance
                    .RenderingIdKey_ItemIDsValue_Dic[args.Rendering.UniqueId.ToString()]
                    .Select(x => x.ToString()).OrderBy(x => x));

            if (!string.IsNullOrEmpty(usedItemIds))
            {
                var aidsKey = "_#aids:" + usedItemIds;
                cacheKey += aidsKey;
                args.CacheKey = cacheKey;
            }

            cacheKey += "_#pid:" + args.PageContext.Item.ID;
            args.CacheKey = cacheKey;
        }
    }
}