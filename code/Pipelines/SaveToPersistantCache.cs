using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Foundation.HtmlCache.Arguments;
using Foundation.HtmlCache.DB;
using Foundation.HtmlCache.Helpers;
using Foundation.HtmlCache.Models;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Foundation.HtmlCache.Pipelines
{
    public class SaveToPersistantCache : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            var renderingProcessorArgs = (RenderingProcessorArgs) HttpContext.Current.Items["RenderingArgs"];
            if (renderingProcessorArgs.TrackOperationEnum == TrackOperation.TrackOperationEnum.Track && renderingProcessorArgs.ItemAccessList.Any())
            {
                using (var ctx = new ItemTrackingProvider())
                {
                    try
                    {
                        var ids = TVPHelper.GetTVPParameter(renderingProcessorArgs.ItemAccessList.ToList());

                        ctx.UspQueueCacheData(Context.Site.SiteInfo.Name, Context.Site.SiteInfo.Language,
                            renderingProcessorArgs.CacheKey, renderingProcessorArgs.CacheResult, ids);
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to write to cache store", e, this);
                    }
                }
            }
            renderingProcessorArgs.TrackOperationEnum = TrackOperation.TrackOperationEnum.DoNotTrack;
            HttpContext.Current.Items["RenderingArgs"] = renderingProcessorArgs;
        }
    }
}