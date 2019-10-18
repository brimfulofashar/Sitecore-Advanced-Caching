using System.Linq;
using System.Text.RegularExpressions;
using Sitecore.Data;
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
            if (!ItemAccessTracker.Instance.RenderingIdKey_ItemIDsValue_Dic.ContainsKey(
                args.Rendering.UniqueId.ToString()))
                return;
            var newCacheKey = "_#aids:" + string.Join("|",
                                  ItemAccessTracker.Instance
                                      .RenderingIdKey_ItemIDsValue_Dic[args.Rendering.UniqueId.ToString()]
                                      .Where(x => x != (ID) null)
                                      .Select(x => x.ToString()).OrderBy(x => x));

            var match = new Regex(
                    "_#aids:(\\{{0,1}[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}\\}{0,1}\\|?)+")
                .Match(cacheKey);
            if (match.Success && match.Groups[0].Value != newCacheKey)
                cacheKey = cacheKey.Replace(match.Groups[0].Value, newCacheKey);
            else if (!match.Success) cacheKey += newCacheKey;

            if (!cacheKey.Contains("_#pid:")) cacheKey += "_#pid:" + args.PageContext.Item.ID;

            args.CacheKey = cacheKey;
        }
    }
}