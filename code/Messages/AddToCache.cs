using System;
using Foundation.HtmlCache.Providers;
using Newtonsoft.Json;

namespace Foundation.HtmlCache.Messages
{
    [Serializable]
    public class AddToCache : SiteMetaData, ICacheMessage
    {
        public AddToCache(string siteInfoName, string siteInfoLanguage, RenderingProcessorArgs renderingProcessorArgs) : base(siteInfoName, siteInfoLanguage)
        {
            RenderingProcessorArgs = renderingProcessorArgs;
        }

        [JsonProperty("RenderingProcessorArgs")]
        public RenderingProcessorArgs RenderingProcessorArgs { get; set; }

        public void Handle()
        {
            ItemTrackingStore.Instance.Add(this.SiteInfoName, this.SiteInfoLanguage, this.RenderingProcessorArgs);
        }
    }
}