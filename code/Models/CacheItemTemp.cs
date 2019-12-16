using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundation.HtmlCache.Models
{
    public class CacheItemTemp
    {
        public Guid Id { get; set; } // Id (Primary key)
        public Guid ItemId { get; set; } // ItemId
        public Guid CacheKeyId { get; set; } // CacheKey_Id

        // Reverse navigation

        /// <summary>
        /// Child CacheKeysItemsTemps where [CacheKeysItemsTemp].[CacheItem_Id] point to this entity (FK_CacheKeysItemsTemp_CacheItemsTemp)
        /// </summary>
        public virtual ICollection<CacheKeyItemTemp> CacheKeyItemTemp { get; set; } // CacheKeysItemsTemp.FK_CacheKeysItemsTemp_CacheItemsTemp

        // Foreign keys

        /// <summary>
        /// Parent CacheKeysTemp pointed by [CacheItemsTemp].([CacheKeyId]) (FK_CacheItemsTemp_CacheKeysTemp)
        /// </summary>
        public virtual CacheKeyTemp CacheKeysTemp { get; set; } // FK_CacheItemsTemp_CacheKeysTemp

        public CacheItemTemp()
        {
            CacheKeyItemTemp = new List<CacheKeyItemTemp>();
        }
    }
}