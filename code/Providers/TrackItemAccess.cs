﻿using System.Web;
using Foundation.HtmlCache.Arguments;
using Foundation.HtmlCache.Models;
using Sitecore.Data.Items;
using Sitecore.Pipelines.ItemProvider.GetItem;

namespace Foundation.HtmlCache.Providers
{
    public class TrackItemAccess : GetItemProcessor
    {
        public override void Process(GetItemArgs args)
        {
            if (args.Result != null && HttpContext.Current != null &&
                HttpContext.Current.Items.Contains("RenderingArgs"))
            {
                Item item = args.Result;
                if (item != null)
                {
                    var renderingProcessorArgs = (RenderingProcessorArgs) HttpContext.Current.Items["RenderingArgs"];
                    if (renderingProcessorArgs.TrackOperationEnum == TrackOperation.TrackOperationEnum.Track &&
                        renderingProcessorArgs.CacheableTemplates.Contains(item.TemplateID.ToString()) &&
                        !item.Paths.FullPath.Contains("/sitecore/templates"))
                    {
                        renderingProcessorArgs.ItemAccessList.Add(new ItemMetaData(item.ID.Guid, item.Language.Name));
                        HttpContext.Current.Items["RenderingArgs"] = renderingProcessorArgs;
                    }
                }
            }
        }
    }
}