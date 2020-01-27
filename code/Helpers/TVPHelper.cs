using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Foundation.HtmlCache.Arguments;
using Foundation.HtmlCache.DB;

namespace Foundation.HtmlCache.Helpers
{
    public class TVPHelper
    {
        public static string HttpContextKey = "TVPHelper";

        private HashSet<CacheSiteTemp> CacheSiteTempSet { get; set; }
        private HashSet<CacheHtmlTemp> CacheHtmlTempSet { get; set; }
        private HashSet<CacheHtmlTempCacheItemTemp> CacheHtmlTempCacheItemTempSet { get; set; }
        private HashSet<CacheItemTemp> CacheItemTempSet { get; set; }

        public Dictionary<string, RenderingProcessorArgs> Tracker { get; set; }

        private byte[] SHA512(string input)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                return hash.ComputeHash(bytes);
            }
        }

        public TVPHelper()
        {
            TVP = new DataSet("TVP");
            var cacheSiteDt = new DataTable(CacheSite_TVP);
            cacheSiteDt.Columns.Add("Id", typeof(Guid));
            cacheSiteDt.Columns.Add("SiteName", typeof(string));
            cacheSiteDt.Columns.Add("SiteLang", typeof(string));

            var cacheHtmlDt = new DataTable(CacheHtml_TVP);
            cacheHtmlDt.Columns.Add("Id", typeof(Guid));
            cacheHtmlDt.Columns.Add("CacheSideId", typeof(Guid));
            cacheHtmlDt.Columns.Add("HtmlCacheKey", typeof(string));
            cacheHtmlDt.Columns.Add("HtmlCacheResult", typeof(string));
            cacheHtmlDt.Columns.Add("HtmlCacheKeyHash", typeof(byte[]));

            var cacheHtmlCacheItemDt = new DataTable(CacheHtml_CacheItem_TVP);
            cacheHtmlCacheItemDt.Columns.Add("Id", typeof(Guid));
            cacheHtmlCacheItemDt.Columns.Add("CacheHtmlId", typeof(Guid));
            cacheHtmlCacheItemDt.Columns.Add("CacheItemId", typeof(Guid));

            var cacheItemDt = new DataTable(CacheItem_TVP);
            cacheItemDt.Columns.Add("Id", typeof(Guid));
            cacheItemDt.Columns.Add("ItemId", typeof(Guid));
            cacheItemDt.Columns.Add("ItemLang", typeof(string));
            cacheItemDt.Columns.Add("IsDeleted", typeof(bool));

            TVP.Tables.Add(cacheSiteDt);
            TVP.Tables.Add(cacheHtmlDt);
            TVP.Tables.Add(cacheItemDt);
            TVP.Tables.Add(cacheHtmlCacheItemDt);

            Tracker = new Dictionary<string, RenderingProcessorArgs>();
            CacheSiteTempSet = new HashSet<CacheSiteTemp>();
            CacheHtmlTempSet = new HashSet<CacheHtmlTemp>();
            CacheHtmlTempCacheItemTempSet = new HashSet<CacheHtmlTempCacheItemTemp>();
            CacheItemTempSet = new HashSet<CacheItemTemp>();
        }

        public DataSet TVP { get; set; }

        public string CacheSite_TVP = "CacheSite_TVP";
        public string CacheHtml_TVP = "CacheHtml_TVP";
        public string CacheHtml_CacheItem_TVP = "CacheHtml_CacheItem_TVP";
        public string CacheItem_TVP = "CacheItem_TVP";

        public void ProcessPublishData(Guid itemId, string itemLang, bool isDeleted)
        {
            CacheItemTemp cacheItemTemp = new CacheItemTemp
            {
                ItemId = itemId,
                ItemLang = itemLang,
                IsDeleted = isDeleted
            };

            TVP.Tables[CacheItem_TVP].Rows.Add(cacheItemTemp.Id, cacheItemTemp.ItemId,
                cacheItemTemp.ItemLang,
                cacheItemTemp.IsDeleted);
        }

        public void ProcessTrackingData(string siteName, string siteLang, string htmlCacheKey,
            string htmlCacheResult, Guid itemId, string itemLang, bool isDeleted)
        {
            if (TVP != null)
            {
                CacheSiteTemp cacheSiteTemp = new CacheSiteTemp
                {
                    SiteName = siteName,
                    SiteLang = siteLang
                };

                var cacheSiteTempExists = !CacheSiteTempSet.Add(cacheSiteTemp);

                if (cacheSiteTempExists)
                {
                    cacheSiteTemp = CacheSiteTempSet.First(x => Equals(x, cacheSiteTemp));
                }
                else
                {
                    TVP.Tables[CacheSite_TVP].Rows
                        .Add(cacheSiteTemp.Id, cacheSiteTemp.SiteName, cacheSiteTemp.SiteLang);
                }

                CacheHtmlTemp cacheHtmlTemp = new CacheHtmlTemp
                {
                    CacheSiteTempId = cacheSiteTemp.Id,
                    HtmlCacheKey = htmlCacheKey,
                    HtmlCacheResult = htmlCacheResult,
                    HtmlCacheKeyHash = SHA512(htmlCacheKey)
                };

                var cacheHtmlTempExists = !CacheHtmlTempSet.Add(cacheHtmlTemp);

                if (cacheHtmlTempExists)
                {
                    cacheHtmlTemp = CacheHtmlTempSet.First(x => x.Equals(cacheHtmlTemp));
                }
                else
                {
                    TVP.Tables[CacheHtml_TVP].Rows.Add(cacheHtmlTemp.Id, cacheSiteTemp.Id,
                        cacheHtmlTemp.HtmlCacheKey,
                        cacheHtmlTemp.HtmlCacheResult, cacheHtmlTemp.HtmlCacheKeyHash);
                }

                CacheItemTemp cacheItemTemp = new CacheItemTemp
                {
                    ItemId = itemId,
                    ItemLang = itemLang,
                    IsDeleted = isDeleted
                };

                var cacheItemExists = !CacheItemTempSet.Add(cacheItemTemp);

                if (cacheItemExists)
                {
                    cacheItemTemp = CacheItemTempSet.First(x => x.Equals(cacheItemTemp));
                }
                else
                {
                    TVP.Tables[CacheItem_TVP].Rows.Add(cacheItemTemp.Id, cacheItemTemp.ItemId,
                        cacheItemTemp.ItemLang,
                        cacheItemTemp.IsDeleted);
                }

                CacheHtmlTempCacheItemTemp cacheHtmlTempCacheItemTemp = new CacheHtmlTempCacheItemTemp
                {
                    CacheHtmlTempId = cacheHtmlTemp.Id,
                    CacheItemTempId = cacheItemTemp.Id
                };

                var cacheHtmlTempCacheItemTempExists = !CacheHtmlTempCacheItemTempSet.Add(cacheHtmlTempCacheItemTemp);

                if (!cacheHtmlTempCacheItemTempExists)
                {
                    TVP.Tables[CacheHtml_CacheItem_TVP].Rows.Add(cacheHtmlTempCacheItemTemp.Id,
                        cacheHtmlTempCacheItemTemp.CacheHtmlTempId, cacheHtmlTempCacheItemTemp.CacheItemTempId);
                }

                HttpContext.Current.Items["TVPHelper"] = this;
            }
        }
    }
}