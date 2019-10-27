using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundation.HtmlCache.Models
{
    public class ClearCacheOperation
    {
        public enum ClearCacheOperationEnum
        {
            Site,
            SiteAllLanguages,
            AllSites
        }
    }
}