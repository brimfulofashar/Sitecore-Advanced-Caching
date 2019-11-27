using System;
using System.Collections.Generic;
using System.Linq;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Settings;
using Sitecore.Data;
using Sitecore.Web;

namespace Foundation.HtmlCache.Providers
{
    public sealed class ItemTrackingStore
    {
        private static readonly Lazy<ItemTrackingStore>
            lazy =
                new Lazy<ItemTrackingStore>
                    (() => new ItemTrackingStore());

        public Dictionary<string, KeyValuePair<string, string>> PersistedHtmlCache;

        private readonly Dictionary<string, Dictionary<string, HashSet<string>>> RenderingIdKey_ItemIDsValue_Dic;

        private ItemTrackingStore()
        {
            RenderingIdKey_ItemIDsValue_Dic = new Dictionary<string, Dictionary<string, HashSet<string>>>();
            PersistedHtmlCache = new Dictionary<string, KeyValuePair<string, string>>();
        }

        public static ItemTrackingStore Instance => lazy.Value;

        public void Add(string siteInfoName, string siteInfoLanguage, RenderingProcessorArgs renderingProcessorArgs)
        {
            if (renderingProcessorArgs.Cacheable && !string.IsNullOrEmpty(renderingProcessorArgs.CacheKey))
            {
                string siteLangKey = "_#site:" + siteInfoName + "#lang:" + siteInfoLanguage;

                if (!RenderingIdKey_ItemIDsValue_Dic.ContainsKey(siteLangKey))
                {
                    RenderingIdKey_ItemIDsValue_Dic.Add(siteLangKey, new Dictionary<string, HashSet<string>>());
                }

                if (!RenderingIdKey_ItemIDsValue_Dic[siteLangKey].ContainsKey(renderingProcessorArgs.CacheKey))
                {
                    RenderingIdKey_ItemIDsValue_Dic[siteLangKey].Add(renderingProcessorArgs.CacheKey, new HashSet<string>());
                }

                foreach (ItemMetaData item in renderingProcessorArgs.ItemAccessList)
                {
                    bool globalTemplateContainsItem = GlobalCacheTemplateSettings.Instance.GlobalCacheableTemplateIDs.Contains(item.TempalteId);

                    if (renderingProcessorArgs.CacheableTemplates.Contains(item.TempalteId.ToString()) || globalTemplateContainsItem)
                    {
                        RenderingIdKey_ItemIDsValue_Dic[siteLangKey][renderingProcessorArgs.CacheKey].Add(item.Id.ToString());
                    }
                }
            }
        }

        public void Delete(string siteInfoName, string siteInfoLanguage, ID itemId)
        {
            var cacheKeysToRemove = new HashSet<string>();

            string siteLangKey = "_#site:" + siteInfoName + "#lang:" + siteInfoLanguage;

            RenderingIdKey_ItemIDsValue_Dic.TryGetValue(siteLangKey,
                out Dictionary<string, HashSet<string>> renderingItemsList);

            string[] keys = renderingItemsList?.Keys.ToArray();
            if (keys != null)
                foreach (string key in keys)
                    if (renderingItemsList.TryGetValue(key, out HashSet<string> list))
                        if (list.Contains(itemId.Guid.ToString()))
                            cacheKeysToRemove.Add(key);

            List<SiteInfo> siteInfoList = Sitecore.Configuration.Factory.GetSiteInfoList();

            foreach (var siteInfo in siteInfoList)
            {
                foreach (string cacheKey in cacheKeysToRemove)
                {
                    RenderingIdKey_ItemIDsValue_Dic[siteLangKey].Remove(cacheKey);
                    siteInfo.HtmlCache.RemoveKeysContaining(cacheKey);
                }
            }
        }

        public void DeleteSite(string siteInfoName, string siteInfoLanguage)
        {
            string siteLangKey = "_#site:" + siteInfoName + "#lang:" + siteInfoLanguage;

            List<SiteInfo> siteInfoList = Sitecore.Configuration.Factory.GetSiteInfoList();
            foreach (var siteInfo in siteInfoList)
            {
                if (siteInfo.Name == siteInfoName && siteInfo.Language == siteInfoLanguage)
                {
                    siteInfo.HtmlCache.Clear(true);
                }
            }

            if (RenderingIdKey_ItemIDsValue_Dic.ContainsKey(siteLangKey))
                RenderingIdKey_ItemIDsValue_Dic.Remove(siteLangKey);

        }
    }
}