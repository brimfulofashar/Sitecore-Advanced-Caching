using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Foundation.HtmlCache.Models
{
    [Table("CacheQueueMessageType")]
    public class CacheQueueMessageType
    {
        public int Id { get; set; } // Id (Primary key)
        public string MessageType { get; set; } // MessageType (length: 100)

        // Reverse navigation

        /// <summary>
        /// Child CacheQueues where [CacheQueue].[CacheQueueMessageType_Id] point to this entity (FK_CacheQueue_CacheQueueMessageType)
        /// </summary>
        public virtual ICollection<CacheQueue> CacheQueues { get; set; } // CacheQueue.FK_CacheQueue_CacheQueueMessageType

        public CacheQueueMessageType()
        {
            CacheQueues = new List<CacheQueue>();
        }

        public enum MessageTypeEnum
        {
            AddToCache = 1,
            DeleteFromCache,
            DeleteSiteFromCache
        }
    }
}