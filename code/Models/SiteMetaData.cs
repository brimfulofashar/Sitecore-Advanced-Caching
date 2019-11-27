using System;
using Newtonsoft.Json;

namespace Foundation.HtmlCache.Models
{
    [Serializable]
    public class SiteMetaData
    {
        public SiteMetaData(string siteInfoName, string siteInfoLanguage)
        {
            SiteInfoName = siteInfoName;
            SiteInfoLanguage = siteInfoLanguage;
        }

        [JsonProperty("SiteInfoName")]
        public string SiteInfoName { get; set; }
        [JsonProperty("SiteInfoLanguage")]
        public string SiteInfoLanguage { get; set; }

        public string GetSiteLangString()
        {
            return "_#site:" + SiteInfoName + "#lang:" + SiteInfoLanguage;
        }
    }
}