using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Foundation.HtmlCache.Models
{
    public class PublishedItem
    {
        public long CacheQueueId { get; set; } // CacheQueueId (Primary key)
        public Guid ItemId { get; set; } // ItemId (Primary key)

        // Foreign keys

        /// <summary>
        /// Parent CacheQueue pointed by [PublishedItems].([CacheQueueId]) (FK_PublishedItems_CacheQueue)
        /// </summary>
        public virtual CacheQueue CacheQueue { get; set; } // FK_PublishedItems_CacheQueue
    }
}