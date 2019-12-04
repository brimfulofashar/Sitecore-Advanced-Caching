using System;
using System.Data.Entity.Migrations;
using System.Linq;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Providers;
using Newtonsoft.Json;

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
            using (var ctx = new ItemTrackingProvider())
            {
                var existing = ctx.CacheKeys.FirstOrDefault(x =>
                    x.HtmlCacheKey == this.RenderingProcessorArgs.CacheKey && x.SiteName == SiteInfoName &&
                    x.SiteLang == SiteInfoLanguage);
                if (existing != null)
                {
                    ctx.CacheKeys.Remove(existing);
                }
                var cacheKey = new CacheKey { ID = Guid.NewGuid(), HtmlCacheKey = this.RenderingProcessorArgs.CacheKey, SiteName = this.SiteInfoName, SiteLang = this.SiteInfoLanguage, CacheItems = this.RenderingProcessorArgs.ItemAccessList.Select(x => new CacheItem(){ID = Guid.NewGuid(), ItemId = x.Id}).ToList()};
                ctx.CacheKeys.Add(cacheKey);
                ctx.SaveChanges();
            }
        }
    }
}