using System;
using System.Web;
using Foundation.HtmlCache.DB;
using Foundation.HtmlCache.Helpers;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Request.RequestEnd;

namespace Foundation.HtmlCache.Pipelines
{
    public class SaveToPersistantCache : RequestEndProcessor
    {
        public override void Process(RequestEndArgs args)
        {
            using (var ctx = new ItemTrackingProvider())
            {
                try
                {
                    var tvpHelper = HttpContext.Current.Items[TVPHelper.HttpContextKey] as TVPHelper;
                    var dataset = tvpHelper?.TVP;
                    if (dataset != null && dataset.Tables[tvpHelper.CacheSite_TVP].Rows.Count > 0 &&
                    dataset.Tables[tvpHelper.CacheHtml_TVP].Rows.Count > 0 && dataset.Tables[tvpHelper.CacheHtml_CacheItem_TVP].Rows.Count > 0 &&
                    dataset.Tables[tvpHelper.CacheItem_TVP].Rows.Count > 0)
                    {
                        ctx.UspQueueCacheData(dataset.Tables[tvpHelper.CacheSite_TVP],
                            dataset.Tables[tvpHelper.CacheHtml_TVP], dataset.Tables[tvpHelper.CacheHtml_CacheItem_TVP],
                            dataset.Tables[tvpHelper.CacheItem_TVP]);
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Failed to write to cache store", e, this);
                }
            }
        }
    }
}