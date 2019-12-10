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
        public string Suffix { get; set; } // Suffix (length: 32)

        // Reverse navigation

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
            PublishedItems = new List<PublishedItem>();
        }
    }
}