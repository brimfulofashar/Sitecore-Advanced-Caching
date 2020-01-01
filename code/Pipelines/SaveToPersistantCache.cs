using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Foundation.HtmlCache.Arguments;
using Foundation.HtmlCache.DB;
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
                        var cacheQueue = new CacheQueue
                        {
                            CacheQueueMessageTypeId = (int)MessageTypeEnum.AddToCache
                        };

                        var cacheTemps = new List<CacheTemp>();

                        foreach (var trackedItem in renderingProcessorArgs.ItemAccessList)
                        {
                            cacheTemps.Add(new CacheTemp
                            {
                                SiteName = Context.Site.SiteInfo.Name,
                                SiteLang = Context.Site.SiteInfo.Language,
                                CacheQueue = cacheQueue,
                                HtmlCacheKey = args.CacheKey,
                                HtmlCacheResult = renderingProcessorArgs.CacheResult,
                                ItemId = trackedItem.Id
                            });
                        }

                        cacheQueue.CacheTemps = cacheTemps;

                        ctx.CacheQueues.Add(cacheQueue);

                        ctx.SaveChanges();
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