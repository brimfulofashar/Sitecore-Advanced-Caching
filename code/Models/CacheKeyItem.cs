using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foundation.HtmlCache.Models
{
    public class CacheKeyItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public Guid Id { get; set; }
        [Index("IX_CacheKeysItems", 0, IsUnique = true)]
        public Guid CacheKey_Id { get; set; }
        [Index("IX_CacheKeysItems", 1, IsUnique = true)]
        public Guid CacheItem_Id { get; set; }
        public virtual CacheItem CacheItem { get; set; }
        public virtual CacheKey CacheKey { get; set; }
    }
}