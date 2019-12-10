using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foundation.HtmlCache.Models
{
    public class CacheItem
    {
        public Guid Id { get; set; } // Id (Primary key)
        public Guid ItemId { get; set; } // ItemId
        public Guid CacheKeyId { get; set; } // CacheKey_Id

        // Reverse navigation

        /// <summary>
        /// Child CacheKeysItems where [CacheKeysItems].[CacheItem_Id] point to this entity (FK_CacheKeysItems_CacheItems)
        /// </summary>
        public virtual ICollection<CacheKeyItem> CacheKeysItems { get; set; } // CacheKeysItems.FK_CacheKeysItems_CacheItems

        // Foreign keys

        /// <summary>
        /// Parent CacheKey pointed by [CacheItems].([CacheKeyId]) (FK_CacheItems_CacheKeys)
        /// </summary>
        public virtual CacheKey CacheKey { get; set; } // FK_CacheItems_CacheKeys

        public CacheItem()
        {
            CacheKeysItems = new List<CacheKeyItem>();
        }
    }
}