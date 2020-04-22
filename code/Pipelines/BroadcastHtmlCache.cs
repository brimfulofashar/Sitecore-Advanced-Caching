using System;
using System.Data;
using System.Web;
using Foundation.HtmlCache.DB;
using Foundation.HtmlCache.Helpers;
using Foundation.HtmlCache.Messaging.Message;
using Foundation.HtmlCache.Messaging.Repository;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Request.RequestEnd;

namespace Foundation.HtmlCache.Pipelines
{
    public class BroadcastHtmlCache : RequestEndProcessor
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
                        foreach (DataRow cacheSiteDr in dataset.Tables[tvpHelper.CacheSite_TVP].Rows)
                        {
                            foreach (DataRow cacheHtmlDr in dataset.Tables[tvpHelper.CacheHtml_TVP].Rows)
                            {
                                BroadcastHtmlCacheRepository broadcastHtmlCacheRepository =
                                    new BroadcastHtmlCacheRepository();
                                broadcastHtmlCacheRepository.BroadcastMessage(new BroadcastHtmlCacheMessage
                                {
                                    SiteName = cacheSiteDr["SiteName"].ToString(),
                                    SiteLang = cacheSiteDr["SiteLang"].ToString(),
                                    HtmlCacheKey = cacheHtmlDr["HtmlCacheKey"].ToString(),
                                    HtmlCacheResult = cacheHtmlDr["HtmlCacheResult"].ToString(),
                                    ToRemove = false
                                });
                            }
                        }
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