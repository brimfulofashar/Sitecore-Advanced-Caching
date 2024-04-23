using System.Web;
using Foundation.HtmlCache.Arguments;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Pipelines;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Data.Templates;
using Sitecore.Pipelines.ItemProvider.GetItem;

namespace Foundation.HtmlCache.Providers
{
    public class TrackItemAccess : GetItemProcessor
    {
        public override void Process(GetItemArgs args)
        {
            if (args.Result != null && HttpContext.Current != null &&
                HttpContext.Current.Items.Contains(RenderingProcessorArgs.Key))
            {
                Item item = args.Result;
                if (item != null)
                {
                    // var templateRootPath = Context.Database.GetItem(ItemIDs.TemplateRoot).Paths.FullPath;

                    var renderingProcessorArgs = (RenderingProcessorArgs) HttpContext.Current.Items[RenderingProcessorArgs.Key];
                    // if (renderingProcessorArgs.TrackOperationEnum == TrackOperation.TrackOperationEnum.Track &&
                    //     renderingProcessorArgs.CacheableTemplates.Contains(item.TemplateID.ToString()) &&
                    //     !item.Paths.FullPath.Contains(templateRootPath) &&
                    //     ((item.Language.Name == Context.Language.Name || item.IsFallback) && item.Language != null && !string.IsNullOrEmpty(item.Language.Name)))
                    if (renderingProcessorArgs.TrackOperationEnum == TrackOperation.TrackOperationEnum.Track &&
                        InitialiseGlobalTemplateList.TemplateIds.ContainsKey(item.TemplateID))
                    {
                        renderingProcessorArgs.ItemAccessList.Add(new ItemMetaData(item.ID.Guid, item.Language.Name, false));
                        HttpContext.Current.Items[RenderingProcessorArgs.Key] = renderingProcessorArgs;
                    }
                }
            }
        }
    }
}