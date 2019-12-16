using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundation.HtmlCache.Models
{
    public class CacheKeyItemTemp
    {
        public Guid Id { get; set; } // Id (Primary key)
        public Guid CacheKeyId { get; set; } // CacheKey_Id
        public Guid CacheItemId { get; set; } // CacheItem_Id

        // Foreign keys

        /// <summary>
        /// Parent CacheItemsTemp pointed by [CacheKeysItemsTemp].([CacheItemId]) (FK_CacheKeysItemsTemp_CacheItemsTemp)
        /// </summary>
        public virtual CacheItemTemp CacheItemTemp { get; set; } // FK_CacheKeysItemsTemp_CacheItemsTemp

        /// <summary>
        /// Parent CacheKeysTemp pointed by [CacheKeysItemsTemp].([CacheKeyId]) (FK_CacheKeysItemsTemp_CacheKeysTemp)
        /// </summary>
        public virtual CacheKeyTemp CacheKeyTemp { get; set; } // FK_CacheKeysItemsTemp_CacheKeysTemp
    }
}