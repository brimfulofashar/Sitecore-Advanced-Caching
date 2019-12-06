using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Foundation.HtmlCache.Models
{
    public class CacheKey
    {
        public CacheKey()
        {
            this.CacheItems = new HashSet<CacheItem>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public Guid Id { get; set; }
        [Index("IX_CacheKeys", IsUnique = true)]
        public string HtmlCacheKey { get; set; }
        public string HtmlCacheResult { get; set; }
        public string SiteName { get; set; }
        public string SiteLang { get; set; }

        public virtual ICollection<CacheItem> CacheItems { get; set; }
    }
}