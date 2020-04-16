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

        private HashSet<CacheSite> CacheSiteSet { get; set; }
        private HashSet<CacheHtml> CacheHtmlSet { get; set; }
        private HashSet<CacheHtmlCacheItem> CacheHtmlCacheItemSet { get; set; }
        private HashSet<CacheItem> CacheItemSet { get; set; }

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
            cacheSiteDt.Columns.Add("Id", typeof(long));
            cacheSiteDt.Columns.Add("SiteName", typeof(string));
            cacheSiteDt.Columns.Add("SiteLang", typeof(string));
            cacheSiteDt.Columns["Id"].AutoIncrement = true;
            cacheSiteDt.Columns["Id"].AutoIncrementSeed = 1;
            cacheSiteDt.Columns["Id"].AutoIncrementStep = 1;

            var cacheHtmlDt = new DataTable(CacheHtml_TVP);
            cacheHtmlDt.Columns.Add("Id", typeof(long));
            cacheHtmlDt.Columns.Add("CacheSideId", typeof(long));
            cacheHtmlDt.Columns.Add("HtmlCacheKey", typeof(string));
            cacheHtmlDt.Columns.Add("HtmlCacheResult", typeof(string));
            cacheHtmlDt.Columns.Add("HtmlCacheKeyHash", typeof(byte[]));
            cacheHtmlDt.Columns["Id"].AutoIncrement = true;
            cacheHtmlDt.Columns["Id"].AutoIncrementSeed = 1;
            cacheHtmlDt.Columns["Id"].AutoIncrementStep = 1;

            var cacheHtmlCacheItemDt = new DataTable(CacheHtml_CacheItem_TVP);
            cacheHtmlCacheItemDt.Columns.Add("Id", typeof(long));
            cacheHtmlCacheItemDt.Columns.Add("CacheHtmlId", typeof(long));
            cacheHtmlCacheItemDt.Columns.Add("CacheItemId", typeof(long));
            cacheHtmlCacheItemDt.Columns["Id"].AutoIncrement = true;
            cacheHtmlCacheItemDt.Columns["Id"].AutoIncrementSeed = 1;
            cacheHtmlCacheItemDt.Columns["Id"].AutoIncrementStep = 1;

            var cacheItemDt = new DataTable(CacheItem_TVP);
            cacheItemDt.Columns.Add("Id", typeof(long));
            cacheItemDt.Columns.Add("ItemId", typeof(Guid));
            cacheItemDt.Columns.Add("ItemLang", typeof(string));
            cacheItemDt.Columns.Add("IsDeleted", typeof(bool));
            cacheItemDt.Columns["Id"].AutoIncrement = true;
            cacheItemDt.Columns["Id"].AutoIncrementSeed = 1;
            cacheItemDt.Columns["Id"].AutoIncrementStep = 1;

            TVP.Tables.Add(cacheSiteDt);
            TVP.Tables.Add(cacheHtmlDt);
            TVP.Tables.Add(cacheItemDt);
            TVP.Tables.Add(cacheHtmlCacheItemDt);

            Tracker = new Dictionary<string, RenderingProcessorArgs>();
            CacheSiteSet = new HashSet<CacheSite>();
            CacheHtmlSet = new HashSet<CacheHtml>();
            CacheHtmlCacheItemSet = new HashSet<CacheHtmlCacheItem>();
            CacheItemSet = new HashSet<CacheItem>();
        }

        public DataSet TVP { get; set; }

        public string CacheSite_TVP = "CacheSite_TVP";
        public string CacheHtml_TVP = "CacheHtml_TVP";
        public string CacheHtml_CacheItem_TVP = "CacheHtml_CacheItem_TVP";
        public string CacheItem_TVP = "CacheItem_TVP";

        public void ProcessPublishData(Guid itemId, string itemLang, bool isDeleted)
        {
            CacheItem cacheItem = new CacheItem
            {
                ItemId = itemId,
                ItemLang = itemLang
            };

            TVP.Tables[CacheItem_TVP].Rows.Add(cacheItem.Id, cacheItem.ItemId,
                cacheItem.ItemLang,
                isDeleted);
        }

        public TVPHelper ProcessTrackingData(string siteName, string siteLang, string htmlCacheKey,
            string htmlCacheResult, Guid itemId, string itemLang)
        {
            if (TVP != null)
            {
                CacheSite cacheSite = new CacheSite
                {
                    SiteName = siteName,
                    SiteLang = siteLang
                };

                var cacheSiteExists = !CacheSiteSet.Add(cacheSite);

                if (cacheSiteExists)
                {
                    cacheSite = CacheSiteSet.First(x => Equals(x, cacheSite));
                }
                else
                {
                    TVP.Tables[CacheSite_TVP].Rows
                        .Add(cacheSite.Id, cacheSite.SiteName, cacheSite.SiteLang);
                }

                CacheHtml cacheHtml = new CacheHtml
                {
                    CacheSiteId = cacheSite.Id,
                    HtmlCacheKey = htmlCacheKey,
                    HtmlCacheResult = htmlCacheResult,
                    HtmlCacheKeyHash = SHA512(htmlCacheKey)
                };

                var cacheHtmlExists = !CacheHtmlSet.Add(cacheHtml);

                if (cacheHtmlExists)
                {
                    cacheHtml = CacheHtmlSet.First(x => x.Equals(cacheHtml));
                }
                else
                {
                    TVP.Tables[CacheHtml_TVP].Rows.Add(cacheHtml.Id, cacheSite.Id,
                        cacheHtml.HtmlCacheKey,
                        cacheHtml.HtmlCacheResult, cacheHtml.HtmlCacheKeyHash);
                }

                CacheItem cacheItem = new CacheItem
                {
                    ItemId = itemId,
                    ItemLang = itemLang
                };

                var cacheItemExists = !CacheItemSet.Add(cacheItem);

                if (cacheItemExists)
                {
                    cacheItem = CacheItemSet.First(x => x.Equals(cacheItem));
                }
                else
                {
                    TVP.Tables[CacheItem_TVP].Rows.Add(cacheItem.Id, cacheItem.ItemId,
                        cacheItem.ItemLang, false);
                }

                CacheHtmlCacheItem cacheHtmlCacheItem = new CacheHtmlCacheItem
                {
                    CacheHtmlId = cacheHtml.Id,
                    CacheItemId = cacheItem.Id
                };

                var cacheHtmlCacheItemExists = !CacheHtmlCacheItemSet.Add(cacheHtmlCacheItem);

                if (!cacheHtmlCacheItemExists)
                {
                    TVP.Tables[CacheHtml_CacheItem_TVP].Rows.Add(cacheHtmlCacheItem.Id,
                        cacheHtmlCacheItem.CacheHtmlId, cacheHtmlCacheItem.CacheItemId);
                }

                return this;
            }
            return null;
        }
    }
}