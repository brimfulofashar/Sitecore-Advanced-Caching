using System.Linq;
using Foundation.HtmlCache.Providers;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Foundation.HtmlCache.Cache
{
    public class GenerateCacheKeyWithUIDAfter : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));

            if (!args.Cacheable || string.IsNullOrEmpty(args.CacheKey) || args.UsedCache)
                return;

            string cacheKey = args.CacheKey;

            string preGeneratedAidsPidKey = args.CustomData["preGeneratedAidsPidKey"]?.ToString();

            string[] list = ItemAccessTracker.Instance.GetByKey(args.Rendering.UniqueId.ToString());

            string aidsKey = string.Empty;

            if (list != null)
            {
                string usedItemIds = string.Join("|", list.OrderBy(x => x));

                if (!string.IsNullOrEmpty(usedItemIds)) aidsKey = "_#aids:" + usedItemIds;
            }

            string pidKey = "_#pid:" + args.PageContext.Item.ID;

            string postGeneratedAidsPidKey = aidsKey + pidKey;

            if (!string.IsNullOrEmpty(preGeneratedAidsPidKey) && preGeneratedAidsPidKey != postGeneratedAidsPidKey)
                cacheKey = cacheKey.Replace(preGeneratedAidsPidKey, postGeneratedAidsPidKey);
            else if (string.IsNullOrEmpty(preGeneratedAidsPidKey))
                cacheKey += postGeneratedAidsPidKey;

            args.CacheKey = cacheKey;
        }
    }
}