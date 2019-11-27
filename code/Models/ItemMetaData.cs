using System;
using Newtonsoft.Json;

namespace Foundation.HtmlCache.Models
{
    [Serializable]
    public class ItemMetaData
    {
        public ItemMetaData(Guid id, Guid templateId)
        {
            Id = id;
            TempalteId = templateId;
        }

        [JsonProperty("Id")]
        public readonly Guid Id;
        [JsonProperty("TempalteId")]
        public readonly Guid TempalteId;

        public override bool Equals(object obj)
        {
            var itemMetaData = obj as ItemMetaData;
            if (itemMetaData == null)
                return false;
            return itemMetaData.Id == Id && itemMetaData.TempalteId == TempalteId;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ TempalteId.GetHashCode();
        }
    }
}