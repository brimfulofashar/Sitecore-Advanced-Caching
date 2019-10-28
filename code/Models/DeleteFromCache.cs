using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Web;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Foundation.HtmlCache.Models
{
    public class DeleteFromCache : CacheJobBase, ICacheJob
    {
        public DeleteFromCache(SiteInfo siteInfo, ID itemId) : base(siteInfo)
        {
            ItemId = itemId;
        }

        public ID ItemId { get; set; }
    }
}