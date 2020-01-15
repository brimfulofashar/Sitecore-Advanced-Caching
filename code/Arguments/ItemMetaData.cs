using System;
using Newtonsoft.Json;

namespace Foundation.HtmlCache.Arguments
{
    [Serializable]
    public class ItemMetaData
    {
        public ItemMetaData(Guid id, string language, bool isDeleted)
        {
            Id = id;
            Language = language;
            IsDeleted = isDeleted;
        }

        [JsonProperty("Id")]
        public readonly Guid Id;
        
        [JsonProperty("Language")]
        public readonly string Language;

        [JsonProperty("IsDeleted")]
        public readonly bool IsDeleted;

        public override bool Equals(object obj)
        {
            var itemMetaData = obj as ItemMetaData;
            if (itemMetaData == null)
                return false;
            return itemMetaData.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}