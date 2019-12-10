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
        public AddToCache(string siteInfoName, string siteInfoLanguage, RenderingProcessorArgs renderingProcessorArgs) :
            base(siteInfoName, siteInfoLanguage)
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
                ctx.Database.Log = sql => System.Diagnostics.Debug.Write(sql);
                try
                {
                    var cacheKeyId = Guid.NewGuid();
                    var cacheKey = new CacheKey
                    {
                        Id = cacheKeyId,
                        HtmlCacheKey = this.RenderingProcessorArgs.CacheKey,
                        HtmlCacheResult = this.RenderingProcessorArgs.CacheResult,
                        SiteName = this.SiteInfoName,
                        SiteLang = this.SiteInfoLanguage
                    };

                    var cacheItems = this.RenderingProcessorArgs.ItemAccessList
                        .Select(x => new CacheItem() { Id = Guid.NewGuid(), ItemId = x.Id, CacheKeyId = cacheKey.Id }).ToArray();

                    var cacheKeyItems = cacheItems.Select(x => new CacheKeyItem
                        { Id = Guid.NewGuid(), CacheKeyId = cacheKey.Id, CacheItemId = x.Id }).ToArray();

                    var cacheQueue = new CacheQueue
                    {
                        Suffix = suffix,
                        CacheQueueMessageTypeId = (int) CacheQueueMessageType.MessageTypeEnum.AddToCache
                    };

                    ctx.CacheKeys.Add(cacheKey);
                    ctx.CacheItems.AddRange(cacheItems);
                    ctx.CacheKeysItems.AddRange(cacheKeyItems);
                    ctx.CacheQueues.Add(cacheQueue);

                    ctx.SaveChanges();
                }
                catch (Exception e)
                {
                    ctx.Database.ExecuteSqlCommand(ctx.GetDeleteTempTableStatement(suffix));
                }
            }
        }
    }
}