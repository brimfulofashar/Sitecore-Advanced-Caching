using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Foundation.HtmlCache.Models
{
    public class DeleteFromCache : ICacheJob 
    {
        public DeleteFromCache(ID itemId)
        {
            ItemId = itemId;

        }
        public ID ItemId { get; set; }
    }
}