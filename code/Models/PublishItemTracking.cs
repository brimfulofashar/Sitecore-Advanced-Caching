using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundation.HtmlCache.Models
{
    public class PublishItemTracking
    {
        public static string Name = "PublishItemTracking";

        public PublishItemTracking(string sourceDb, string destinationDb)
        {
            SourceDB = sourceDb;
            DestinationDB = destinationDb;
            PublishedItems = new Dictionary<Guid, PublishOperation.PublishOperationEnum>();
        }

        public Dictionary<Guid, PublishOperation.PublishOperationEnum> PublishedItems { get; set; }

        public string SourceDB { get; set; }
        public string DestinationDB { get; set; }
    }
}