using System.Linq;
using Foundation.HtmlCache.Providers;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Foundation.HtmlCache.Cache
{
    public class GenerateCacheKeyWithUIDBefore : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));
            if (!args.Cacheable || string.IsNullOrEmpty(args.CacheKey))
                return;

            string cacheKey = args.CacheKey;
            string[] list = ItemAccessTracker.Instance.GetByKey(args.Rendering.UniqueId.ToString());

            if (list != null)
            {
                string usedItemIds = string.Join("|", list.OrderBy(x => x));

                if (!string.IsNullOrEmpty(usedItemIds))
                {
                    string aidsKey = "_#aids:" + usedItemIds;
                    cacheKey += aidsKey;

                    string pidKey = "_#pid:" + args.PageContext.Item.ID;
                    cacheKey += pidKey;

                    args.CustomData.Add("preGeneratedAidsPidKey", aidsKey + pidKey);

                    args.CacheKey = cacheKey;
                }
            }
        }
    }
}