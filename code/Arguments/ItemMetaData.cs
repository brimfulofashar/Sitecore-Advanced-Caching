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
            //TempalteId = templateId;
            Language = language;
        }

        [JsonProperty("Id")]
        public readonly Guid Id;
        //[JsonProperty("TempalteId")]
        //public readonly Guid TempalteId;
        [JsonProperty("Language")]
        public readonly string Language;
    }
}