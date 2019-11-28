using System.Web;
using Foundation.HtmlCache.Messages;
using Foundation.HtmlCache.Models;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Foundation.HtmlCache.Pipelines
{
    public class BeginRenderingContext : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            RenderingProcessorArgs dic;
            if (args.Rendering.RenderingType == "r" && !args.UsedCache && args.Cacheable && !string.IsNullOrEmpty(args.CacheKey))
            {
                dic = new RenderingProcessorArgs(args.CacheKey, 
                    args.Rendering.RenderingItem.InnerItem.Fields["CacheableTemplates"].Value, 
                    args.Rendering.Caching.Cacheable, 
                    TrackOperation.TrackOperationEnum.Track);
                dic.ItemAccessList.Add(new ItemMetaData(args.PageContext.Item.ID.Guid,
                    args.PageContext.Item.TemplateID.Guid));
            }
            else
            {
                dic = new RenderingProcessorArgs(TrackOperation.TrackOperationEnum.DoNotTrack);
            }
            HttpContext.Current.Items["RenderingArgs"] = dic;
        }
    }
}