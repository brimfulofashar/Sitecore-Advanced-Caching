using System;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Web;

namespace Foundation.HtmlCache.Models
{
    public class AddToCache : CacheJobBase, ICacheJob
    {
        public AddToCache(SiteInfo siteInfo, RenderingProcessorArgs renderingProcessorArgs) : base(siteInfo)
        {
            RenderingProcessorArgs = renderingProcessorArgs;
        }

        public RenderingProcessorArgs RenderingProcessorArgs { get; set; }
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