using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Foundation.HtmlCache.Models
{
    public class CacheKey
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; }
        public string HtmlCacheKey { get; set; }
        public string SiteName { get; set; }
        public string SiteLang { get; set; }

        public virtual ICollection<CacheItem> CacheItems { get; set; }
    }
}