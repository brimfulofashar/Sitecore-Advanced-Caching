using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foundation.HtmlCache.Models
{
    [Table("CacheKeysItems")]
    public class CacheKeyItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public Guid Id { get; set; }

        [Index("IX_CacheKeysItems", 0, IsUnique = true)]
        [Column("CacheKey_Id")]
        public Guid CacheKey_Id { get; set; }

        [Index("IX_CacheKeysItems", 1, IsUnique = true)]
        [Column("CacheItem_Id")]
        public Guid CacheItem_Id { get; set; }

        [NotMapped]
        public virtual CacheItem CacheItem { get; set; }

        [NotMapped]
        public virtual CacheKey CacheKey { get; set; }
    }
}