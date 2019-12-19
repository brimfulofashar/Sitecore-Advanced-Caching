using System;
using System.Linq;
using System.Web;
using Foundation.HtmlCache.DB;
using Foundation.HtmlCache.Messages;
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
            if (renderingProcessorArgs.TrackOperationEnum == TrackOperation.TrackOperationEnum.Track)
            {
                var trackedItems = renderingProcessorArgs.ItemAccessList.Where(x =>
                    renderingProcessorArgs.CacheableTemplates.ToLower()
                        .Contains(x.TempalteId.ToString().ToLower())).ToList();
                if (trackedItems.Any())
                {
                    using (var ctx = new ItemTrackingProvider())
                    {
                        try
                        {
                            var cacheQueue = new CacheQueue
                            {
                                CacheQueueMessageTypeId = (int)MessageTypeEnum.AddToCache
                            };

                            var cacheSiteLang = new CacheSiteLangTemp
                                {Name = Context.Site.SiteInfo.Name, Lang = Context.Site.SiteInfo.Language, CacheQueue = cacheQueue};

                            var cacheKey = new CacheKeyTemp
                            {
                                HtmlCacheKey = args.CacheKey,
                                HtmlCacheResult = renderingProcessorArgs.CacheResult,
                                CacheSiteLangTemp = cacheSiteLang,
                                CacheQueue = cacheQueue,
                            };

                            var cacheItems = trackedItems.Select(x => new CacheItemTemp{ItemId = x.Id, CacheKeyId = cacheKey.Id, CacheQueue = cacheQueue }).ToArray();

                            cacheKey.CacheItemTemps = cacheItems;
                            cacheSiteLang.CacheKeyTemps.Add(cacheKey);
                            cacheQueue.CacheSiteLangTemps.Add(cacheSiteLang);

                            ctx.CacheQueues.Add(cacheQueue);

                            ctx.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Log.Error("Failed to write to cache store", e, this);
                        }
                    }
                }
            }
            renderingProcessorArgs.TrackOperationEnum = TrackOperation.TrackOperationEnum.DoNotTrack;
            HttpContext.Current.Items["RenderingArgs"] = renderingProcessorArgs;
        }
    }
}