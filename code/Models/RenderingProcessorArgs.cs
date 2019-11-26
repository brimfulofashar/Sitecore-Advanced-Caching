using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Foundation.HtmlCache.Models
{
    public class RenderingProcessorArgs
    {
        public RenderingProcessorArgs(string cacheKey, Rendering rendering, Item pageItem, TrackOperation.TrackOperationEnum trackOperationEnum)
        {
            CacheKey = cacheKey;
            CacheableTemplates = rendering.RenderingItem.InnerItem.Fields["CacheableTemplates"].Value;
            Cacheable = rendering.Caching.Cacheable;
            TrackOperationEnum = trackOperationEnum;
            ItemAccessList = new HashSet<ItemMetaData>
            {
                new ItemMetaData(pageItem.ID.Guid, pageItem.TemplateID.Guid)
            };
        }

        public RenderingProcessorArgs(TrackOperation.TrackOperationEnum trackOperationEnum)
        {
            TrackOperationEnum = trackOperationEnum;
        }

        public string CacheKey { get; set; }
        public bool Cacheable { get; set; }
        public string CacheableTemplates { get; set; }
        public TrackOperation.TrackOperationEnum? TrackOperationEnum { get; set; }
        public HashSet<ItemMetaData> ItemAccessList { get; set; }
    }
}