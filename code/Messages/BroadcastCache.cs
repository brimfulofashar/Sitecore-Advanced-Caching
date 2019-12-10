using Newtonsoft.Json;

namespace Foundation.HtmlCache.Messages
{
    public class BroadcastCache : SiteMetaData, ICacheMessage
    {
        [JsonProperty("CacheKey")]
        public string CacheKey { get; set; }
        [JsonProperty("CachedHtml")]
        public string CachedHtml { get; set; }
        public BroadcastCache(string siteInfoName, string siteInfoLanguage, string cacheKey, string cachedHtml) : base(siteInfoName, siteInfoLanguage)
        {
            CacheKey = cacheKey;
            CachedHtml = cachedHtml;
        }
    }
}