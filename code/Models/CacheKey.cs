using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foundation.HtmlCache.Models
{
    public class CacheKey
    {
        public Guid Id { get; set; } // Id (Primary key)
        public string SiteName { get; set; } // SiteName (length: 250)
        public string SiteLang { get; set; } // SiteLang (length: 250)
        public string HtmlCacheKey { get; set; } // HtmlCacheKey (length: 500)
        public string HtmlCacheResult { get; set; } // HtmlCacheResult

        // Reverse navigation

        /// <summary>
        /// Child CacheItems where [CacheItems].[CacheKey_Id] point to this entity (FK_CacheItems_CacheKeys)
        /// </summary>
        public virtual ICollection<CacheItem> CacheItems { get; set; } // CacheItems.FK_CacheItems_CacheKeys

        /// <summary>
        /// Child CacheKeysItems where [CacheKeysItems].[CacheKey_Id] point to this entity (FK_CacheKeysItems_CacheKeys)
        /// </summary>
        public virtual ICollection<CacheKeyItem> CacheKeysItems { get; set; } // CacheKeysItems.FK_CacheKeysItems_CacheKeys

        public CacheKey()
        {
            CacheItems = new List<CacheItem>();
            CacheKeysItems = new List<CacheKeyItem>();
        }
    }
}