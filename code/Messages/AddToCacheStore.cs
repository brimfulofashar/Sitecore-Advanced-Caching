using System;
using Foundation.HtmlCache.Models;
using Newtonsoft.Json;

namespace Foundation.HtmlCache.Messages
{
    [Serializable]
    public class AddToCacheStore : SiteMetaData, ICacheMessage
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("CacheKey")]
        public string CacheKey { get; set; }
        [JsonProperty("RenderingId")]
        public string RenderingId { get; set; }
        [JsonProperty("CachedHtml")]
        public string CachedHtml { get; set; }

        public AddToCacheStore(string siteInfoName, string siteInfoLanguage, string cacheKey, string renderingId, string cachedHtml, string name) : base(siteInfoName, siteInfoLanguage)
        {
            CacheKey = cacheKey;
            RenderingId = renderingId;
            CachedHtml = cachedHtml;
            Name = name;
        }


    }
}