using System;
using Newtonsoft.Json;

namespace Foundation.HtmlCache.Messages
{
    [Serializable]
    public class AddToCacheStore : BroadcastCache, ICacheMessage
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("RenderingId")]
        public string RenderingId { get; set; }

        public AddToCacheStore(string siteInfoName, string siteInfoLanguage, string cacheKey, string renderingId, string cachedHtml, string name) : base(siteInfoName, siteInfoLanguage, cacheKey, cachedHtml)
        {
            CacheKey = cacheKey;
            RenderingId = renderingId;
            CachedHtml = cachedHtml;
            Name = name;
        }


    }
}