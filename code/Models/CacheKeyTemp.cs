using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundation.HtmlCache.Models
{
    public class CacheKeyTemp
    {
        public Guid Id { get; set; } // Id (Primary key)
        public long CacheQueueId { get; set; } // CacheQueue_Id
        public string SiteName { get; set; } // SiteName (length: 250)
        public string SiteLang { get; set; } // SiteLang (length: 250)
        public string HtmlCacheKey { get; set; } // HtmlCacheKey (length: 500)
        public string HtmlCacheResult { get; set; } // HtmlCacheResult

        // Reverse navigation

        /// <summary>
        /// Child CacheItemsTemps where [CacheItemsTemp].[CacheKey_Id] point to this entity (FK_CacheItemsTemp_CacheKeysTemp)
        /// </summary>
        public virtual ICollection<CacheItemTemp> CacheItemTemp { get; set; } // CacheItemsTemp.FK_CacheItemsTemp_CacheKeysTemp

        /// <summary>
        /// Child CacheKeysItemsTemps where [CacheKeysItemsTemp].[CacheKey_Id] point to this entity (FK_CacheKeysItemsTemp_CacheKeysTemp)
        /// </summary>
        public virtual ICollection<CacheKeyItemTemp> CacheKeyItemTemp { get; set; } // CacheKeysItemsTemp.FK_CacheKeysItemsTemp_CacheKeysTemp

        // Foreign keys

        /// <summary>
        /// Parent CacheQueue pointed by [CacheKeysTemp].([CacheQueueId]) (FK_CacheKeysTemp_CacheQueue)
        /// </summary>
        public virtual CacheQueue CacheQueue { get; set; } // FK_CacheKeysTemp_CacheQueue

        public CacheKeyTemp()
        {
            CacheItemTemp = new List<CacheItemTemp>();
            CacheKeyItemTemp = new List<CacheKeyItemTemp>();
        }
    }
}