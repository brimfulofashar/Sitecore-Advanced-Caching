using System;
using Newtonsoft.Json;
using Sitecore.Web;

namespace Foundation.HtmlCache.Models
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