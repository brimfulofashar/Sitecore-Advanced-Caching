using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Web;

namespace Foundation.HtmlCache.Models
{
    public class AddToCache : CacheJobBase, ICacheJob
    {
        public AddToCache(SiteInfo siteInfo, Rendering rendering, Item item) : base(siteInfo)
        {
            Rendering = rendering;
            Item = item;
        }

        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
    }

    public class CacheJobBase
    {
        public CacheJobBase(SiteInfo siteInfo)
        {
            SiteInfo = siteInfo;
        }

        public SiteInfo SiteInfo { get; set; }
    }
}