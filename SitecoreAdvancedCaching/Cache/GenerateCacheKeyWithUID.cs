using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using Sitecore.Mvc.Presentation;

namespace SitecoreAdvancedCaching.Cache
{
    public class GenerateCacheKeyWithUID : GenerateCacheKey
    {
        protected override string GenerateKey(Rendering rendering, RenderRenderingArgs args)
        {
            var key = base.GenerateKey(rendering, args);
            // stores the id of the item that refers to the rendering
            key += "_#iid:" + args.Rendering.Item.ID.Guid;
            // stores the unique id of the rendering on the page item
            key += "_#ruid:" + args.Rendering.UniqueId;
            // stores the item id of the rendering
            key += "_#rid:" + args.Rendering.RenderingItem.ID.Guid;
            return key;
        }
    }
}