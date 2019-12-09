using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using Foundation.HtmlCache.Extensions;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Providers;
using Newtonsoft.Json;
using Sitecore.Diagnostics;

namespace Foundation.HtmlCache.Messages
{
    [JsonObject(Title = "AddToCache")]
    public class AddToCache : SiteMetaData, ICacheMessage
    {
        public AddToCache(string siteInfoName, string siteInfoLanguage, RenderingProcessorArgs renderingProcessorArgs) : base(siteInfoName, siteInfoLanguage)
        {
            RenderingProcessorArgs = renderingProcessorArgs;
        }

        [JsonProperty("RenderingProcessorArgs")]
        public RenderingProcessorArgs RenderingProcessorArgs { get; set; }

        public void Handle()
        {
            var suffix = Guid.NewGuid().ToString().Replace("-", string.Empty);
            using (var ctx = ItemTrackingProvider.CreateDummyContext(suffix))
            {
                using (var dbContextTransaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var cacheKey = new CacheKey
                        {
                            Id = Guid.NewGuid(),
                            HtmlCacheKey = this.RenderingProcessorArgs.CacheKey,
                            HtmlCacheResult = this.RenderingProcessorArgs.CacheResult,
                            SiteName = this.SiteInfoName,
                            SiteLang = this.SiteInfoLanguage
                        };

                        ctx.CacheKeys.AddOrUpdate(cacheKey);

                        var cacheItems = this.RenderingProcessorArgs.ItemAccessList
                            .Select(x => new CacheItem() {Id = Guid.NewGuid(), ItemId = x.Id, CacheKey_Id = cacheKey.Id}).ToArray();
                        ctx.CacheItems.AddOrUpdate(cacheItems);

                        var cacheKeyItems = cacheItems.Select(x => new CacheKeyItem()
                            {Id = Guid.NewGuid(), CacheKey_Id = cacheKey.Id, CacheItem_Id = x.Id}).ToArray();
                        ctx.CacheKeyItems.AddOrUpdate(cacheKeyItems);

                        ctx.CacheQueues.Add(new CacheQueue() {Suffix = suffix, CacheQueueMessageType_Id = (int)CacheQueueMessageType.MessageTypeEnum.AddToCache});

                        ctx.SaveChanges();

                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Log.Error("Can't persist cache", e, this);
                    }
                }
            }
        }
    }
}