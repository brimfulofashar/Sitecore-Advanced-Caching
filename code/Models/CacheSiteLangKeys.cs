using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundation.HtmlCache.Models
{
    public class CacheSiteLangKeys
    {
        public int CacheQueueMessageTypeId { get; set; }
        public string SiteName { get; set; }
        public string SiteLang { get; set; }
        public List<string> HtmlCacheKeys { get; set; }
    }
}