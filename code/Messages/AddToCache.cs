using System;
using Foundation.HtmlCache.Models;
using Newtonsoft.Json;

namespace Foundation.HtmlCache.Messages
{
    [Serializable]
    public class AddToCache : SiteMetaData, ICacheJob
    {
        public AddToCache(string siteInfoName, string siteInfoLanguage, RenderingProcessorArgs renderingProcessorArgs) : base(siteInfoName, siteInfoLanguage)
        {
            RenderingProcessorArgs = renderingProcessorArgs;
        }

        [JsonProperty("RenderingProcessorArgs")]
        public RenderingProcessorArgs RenderingProcessorArgs { get; set; }
    }
}