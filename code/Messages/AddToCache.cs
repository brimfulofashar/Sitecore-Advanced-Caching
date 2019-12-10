using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using Foundation.HtmlCache.Extensions;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Providers;
using Newtonsoft.Json;
using Sitecore.Diagnostics;

namespace Foundation.HtmlCache.Messages
{
    [JsonObject(Title = "AddToCache")]
    public class AddToCache : SiteMetaData, ICacheMessage
    {
        public AddToCache(string siteInfoName, string siteInfoLanguage, RenderingProcessorArgs renderingProcessorArgs) :
            base(siteInfoName, siteInfoLanguage)
        {
            RenderingProcessorArgs = renderingProcessorArgs;
        }

        [JsonProperty("RenderingProcessorArgs")]
        public RenderingProcessorArgs RenderingProcessorArgs { get; set; }
    }
}