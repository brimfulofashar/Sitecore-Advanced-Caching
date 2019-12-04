using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Foundation.HtmlCache.Models
{
    public class CacheKeysItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; }
        public Guid CacheKeyId { get; set; }
        public Guid CacheItemId { get; set; }
        public virtual CacheItem CacheItem { get; set; }
        public virtual CacheKey CacheKey { get; set; }
    }
}