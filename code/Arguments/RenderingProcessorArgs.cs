using System.Collections.Generic;
using Foundation.HtmlCache.Models;

namespace Foundation.HtmlCache.Arguments
{
    public class RenderingProcessorArgs
    {
        public static string Key = "RenderingArgs";

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

        public string CacheKey { get; set; }

        public string CacheResult { get; set; }
        public bool Cacheable { get; set; }
        public string CacheableTemplates { get; set; }
        public TrackOperation.TrackOperationEnum? TrackOperationEnum { get; set; }
        public HashSet<ItemMetaData> ItemAccessList { get; set; }
    }
}