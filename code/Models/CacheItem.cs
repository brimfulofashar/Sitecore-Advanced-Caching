using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Foundation.HtmlCache.Models
{
    public class CacheItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; }
        public Guid ItemId { get; set; }

        public virtual ICollection<CacheKey> CacheKeys { get; set; }
    }
}