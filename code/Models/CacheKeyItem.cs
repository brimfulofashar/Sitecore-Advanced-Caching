using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foundation.HtmlCache.Models
{
    public class CacheKeyItem
    {
        public Guid Id { get; set; } // Id (Primary key)
        public Guid CacheKeyId { get; set; } // CacheKey_Id
        public Guid CacheItemId { get; set; } // CacheItem_Id

        // Foreign keys

        /// <summary>
        /// Parent CacheItem pointed by [CacheKeysItems].([CacheItemId]) (FK_CacheKeysItems_CacheItems)
        /// </summary>
        public virtual CacheItem CacheItem { get; set; } // FK_CacheKeysItems_CacheItems

        /// <summary>
        /// Parent CacheKey pointed by [CacheKeysItems].([CacheKeyId]) (FK_CacheKeysItems_CacheKeys)
        /// </summary>
        public virtual CacheKey CacheKey { get; set; } // FK_CacheKeysItems_CacheKeys
    }
}