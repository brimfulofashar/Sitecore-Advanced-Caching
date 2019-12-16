using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundation.HtmlCache.Models
{
    public class ProcessCacheQueueResult
    {
        public long PendingQueueLength { get; set; }
        public string SiteName { get; set; }
        public string SiteLang { get; set; }
        public string DeletedCacheKeys { get; set; }
    }
}