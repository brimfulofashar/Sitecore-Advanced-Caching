using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Foundation.HtmlCache.Models
{
    public class CacheQueue
    {
        public long Id { get; set; } // Id (Primary key)
        public int CacheQueueMessageTypeId { get; set; } // CacheQueueMessageType_Id

        // Reverse navigation

        /// <summary>
        /// Child CacheKeysTemps where [CacheKeysTemp].[CacheQueue_Id] point to this entity (FK_CacheKeysTemp_CacheQueue)
        /// </summary>
        public virtual ICollection<CacheKeyTemp> CacheKeysTemps { get; set; } // CacheKeysTemp.FK_CacheKeysTemp_CacheQueue

        /// <summary>
        /// Child PublishedItems where [PublishedItems].[CacheQueueId] point to this entity (FK_PublishedItems_CacheQueue)
        /// </summary>
        public virtual ICollection<PublishedItem> PublishedItems { get; set; } // PublishedItems.FK_PublishedItems_CacheQueue

        // Foreign keys

        /// <summary>
        /// Parent CacheQueueMessageType pointed by [CacheQueue].([CacheQueueMessageTypeId]) (FK_CacheQueue_CacheQueueMessageType)
        /// </summary>
        public virtual CacheQueueMessageType CacheQueueMessageType { get; set; } // FK_CacheQueue_CacheQueueMessageType

        public CacheQueue()
        {
            CacheKeysTemps = new List<CacheKeyTemp>();
            PublishedItems = new List<PublishedItem>();
        }
    }
}