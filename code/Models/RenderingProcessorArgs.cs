using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Foundation.HtmlCache.Models
{
    [Serializable]
    public class RenderingProcessorArgs
    {
        [JsonConstructor]
        public RenderingProcessorArgs(string cacheKey, string cacheableTemplates, bool cacheable, TrackOperation.TrackOperationEnum trackOperationEnum)
        {
            CacheKey = cacheKey;
            CacheableTemplates = cacheableTemplates;
            Cacheable = cacheable;

            TrackOperationEnum = trackOperationEnum;
            ItemAccessList = new HashSet<ItemMetaData>();
        }

        public RenderingProcessorArgs(TrackOperation.TrackOperationEnum trackOperationEnum)
        {
            TrackOperationEnum = trackOperationEnum;
        }

        [JsonProperty("CacheKey")]
        public string CacheKey { get; set; }
        [JsonProperty("Cacheable")]
        public bool Cacheable { get; set; }
        [JsonProperty("CacheableTemplates")]
        public string CacheableTemplates { get; set; }
        [JsonProperty("TrackOperationEnum")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TrackOperation.TrackOperationEnum? TrackOperationEnum { get; set; }
        [JsonProperty("ItemAccessList")]
        public HashSet<ItemMetaData> ItemAccessList { get; set; }
    }
}