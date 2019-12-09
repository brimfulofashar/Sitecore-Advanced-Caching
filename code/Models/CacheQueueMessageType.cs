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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string MessageType { get; set; }

        public enum MessageTypeEnum
        {
            AddToCache = 1,
            DeleteFromCache,
            DeleteSiteFromCache
        }
    }
}