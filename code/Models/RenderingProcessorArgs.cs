using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Foundation.HtmlCache.Models
{
    public class RenderingProcessorArgs
    {
        public RenderingProcessorArgs(string cacheKey, Rendering rendering, Item pageItem)
        {
            CacheKey = cacheKey;
            Rendering = rendering;
            PageItem = pageItem;
        }
        public string CacheKey { get; set; }
        public Rendering Rendering { get; set; }
        public Item PageItem { get; set; }
    }
}