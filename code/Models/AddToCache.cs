using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Foundation.HtmlCache.Models
{
    public class AddToCache : ICacheJob 
    {
        public AddToCache(Rendering rendering, Item item)
        {
            Rendering = rendering;
            Item = item;

        }
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
    }
}