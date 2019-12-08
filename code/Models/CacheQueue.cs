using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Foundation.HtmlCache.Models
{
    [Table("CacheQueue")]
    public class CacheQueue
    {
        public CacheQueue()
        {
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Int64 Id { get; set; }
        public Int32 CacheQueueMessageType_Id { get; set; }
        public string Suffix { get; set; }
    }
}