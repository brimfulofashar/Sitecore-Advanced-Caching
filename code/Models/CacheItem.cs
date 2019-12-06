using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Foundation.HtmlCache.Models
{
    [Table("CacheItems")]
    public class CacheItem
    {
        public CacheItem()
        {
            this.CacheKeys = new HashSet<CacheKey>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public Guid Id { get; set; }
        [Index("IX_CacheItems", IsUnique = true)]
        public Guid ItemId { get; set; }

        public virtual ICollection<CacheKey> CacheKeys { get; set; }
    }
}