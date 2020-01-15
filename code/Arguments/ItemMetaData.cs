using System;
using Newtonsoft.Json;

namespace Foundation.HtmlCache.Arguments
{
    [Serializable]
    public class ItemMetaData
    {
        public ItemMetaData(Guid id, string language)
        {
            Id = id;
            Language = language;
        }

        [JsonProperty("Id")]
        public readonly Guid Id;
        
        [JsonProperty("Language")]
        public readonly string Language;

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