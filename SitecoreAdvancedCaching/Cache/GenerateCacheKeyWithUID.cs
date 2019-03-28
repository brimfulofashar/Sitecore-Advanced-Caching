using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using Sitecore.Mvc.Presentation;

namespace SitecoreAdvancedCaching.Cache
{
    public class GenerateCacheKeyWithUID : GenerateCacheKey
    {
        protected override string GenerateKey(Rendering rendering, RenderRenderingArgs args)
        {
            var key = base.GenerateKey(rendering, args);
            key += "_#iid:" + args.Rendering.Item.ID.Guid;
            key += "_#ruid:" + args.Rendering.UniqueId;
            key += "_#rid:" + args.Rendering.RenderingItem.ID.Guid;
            return key;
        }
    }
}