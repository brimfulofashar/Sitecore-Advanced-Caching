using System;
using System.Linq;
using System.Web;
using Foundation.HtmlCache.Messages;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Providers;
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
                    var suffix = Guid.NewGuid().ToString().Replace("-", string.Empty);
                    using (var ctx = ItemTrackingProvider.CreateDummyContext(suffix))
                    {
                        try
                        {

                            var cacheKeyId = Guid.NewGuid();
                            var cacheKey = new CacheKey
                            {
                                Id = cacheKeyId,
                                HtmlCacheKey = args.CacheKey,
                                HtmlCacheResult = renderingProcessorArgs.CacheResult,
                                SiteName = Context.Site.SiteInfo.Name,
                                SiteLang = Context.Site.SiteInfo.Language
                            };

                            var cacheItems = trackedItems.Select(x => new CacheItem
                                {Id = Guid.NewGuid(), ItemId = x.Id, CacheKeyId = cacheKey.Id}).ToArray();

                            var cacheKeysItems = cacheItems.Select(x => new CacheKeyItem
                                {Id = Guid.NewGuid(), CacheKeyId = cacheKey.Id, CacheItemId = x.Id}).ToArray();

                            var cacheQueue = new CacheQueue
                            {
                                Suffix = suffix,
                                CacheQueueMessageTypeId = (int) CacheQueueMessageType.MessageTypeEnum.AddToCache
                            };

                            ctx.CacheKeys.Add(cacheKey);
                            ctx.CacheItems.AddRange(cacheItems);
                            ctx.CacheKeysItems.AddRange(cacheKeysItems);
                            ctx.CacheQueues.Add(cacheQueue);

                            ctx.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Log.Error("Failed to write to cache store", e, this);
                            ctx.Database.ExecuteSqlCommand(ctx.GetDeleteTempTableStatement(suffix));
                        }
                    }
                }
            }
            renderingProcessorArgs.TrackOperationEnum = TrackOperation.TrackOperationEnum.DoNotTrack;
            HttpContext.Current.Items["RenderingArgs"] = renderingProcessorArgs;
        }
    }
}