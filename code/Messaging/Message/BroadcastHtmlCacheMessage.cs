using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundation.HtmlCache.Messaging.Message
{
    public class BroadcastHtmlCacheMessage
    {
        public bool ToRemove { get; set; }
        public string SiteName { get; set; }
        public string SiteLang { get; set; }
        public string HtmlCacheKey { get; set; }
        public string HtmlCacheResult { get; set; }
    }
}