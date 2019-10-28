using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Web;
using Foundation.HtmlCache.Models;
using Sitecore;
using Sitecore.Abstractions;
using Sitecore.Collections;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.Mvc.Presentation;
using Sitecore.SecurityModel;
using Sitecore.Web;
using Version = Sitecore.Data.Version;

namespace Foundation.HtmlCache.Providers
{
    public class ItemTrackingProvider : ItemProvider
    {
        public ItemTrackingProvider()
        {
        }

        public ItemTrackingProvider(BaseLanguageManager languageManager) : base(languageManager)
        {
        }


        public override Item GetItem(ID itemId, Language language, Version version, Database database,
            SecurityCheck securityCheck)
        {
            Item item = base.GetItem(itemId, language, version, database, securityCheck);
            if (item != null && HttpContext.Current != null && HttpContext.Current.Items["Rendering"] != null &&
                Context.Database.Name == "web")
            {
                var rendering = (Rendering) HttpContext.Current.Items["Rendering"];
                ItemAccessTracker.Instance.Enqueue(new AddToCache(Context.Site.SiteInfo, rendering, item));
            }
            return item;
        }

        public override Item GetItem(string itemPath, Language language, Version version, Database database,
            SecurityCheck securityCheck)
        {
            Item item = base.GetItem(itemPath, language, version, database, securityCheck);
            if (item != null && HttpContext.Current != null && HttpContext.Current.Items["Rendering"] != null &&
                Context.Database.Name == "web")
            {
                var rendering = (Rendering)HttpContext.Current.Items["Rendering"];
                ItemAccessTracker.Instance.Enqueue(new AddToCache(Context.Site.SiteInfo, rendering, item));
            }
            return item;
        }

        protected override Item GetItem(ID itemId, Language language, Version version, Database database)
        {
            Item item = base.GetItem(itemId, language, version, database);
            if (item != null && HttpContext.Current != null && HttpContext.Current.Items["Rendering"] != null &&
                Context.Database.Name == "web")
            {
                var rendering = (Rendering)HttpContext.Current.Items["Rendering"];
                ItemAccessTracker.Instance.Enqueue(new AddToCache(Context.Site.SiteInfo, rendering, item));
            }

            return item;
        }
    }

    public sealed class ItemAccessTracker
    {
        private static readonly Lazy<ItemAccessTracker>
            lazy =
                new Lazy<ItemAccessTracker>
                    (() => new ItemAccessTracker());

        private readonly HashSet<Guid> globalCacheableTemplateIDs;

        private readonly SafeDictionary<string, SafeDictionary<string, HashSet<string>>> RenderingIdKey_ItemIDsValue_Dic;
        
        private ChannelWriter<ICacheJob> _writer;

        private Thread thread;

        private ItemAccessTracker()
        {
            RenderingIdKey_ItemIDsValue_Dic = new SafeDictionary<string, SafeDictionary<string, HashSet<string>>>();
            List<ID> globalCacheableTemplateIDsTemp = Settings.GetSetting("GlobalCacheableTemplateIDs").Split('|')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => ID.Parse(x)).ToList();
            globalCacheableTemplateIDs = new HashSet<Guid>();
            foreach (ID g in globalCacheableTemplateIDsTemp) globalCacheableTemplateIDs.Add(g.Guid);


            var channel = Channel.CreateUnbounded<ICacheJob>();
            var reader = channel.Reader;
            _writer = channel.Writer;

            Task.Factory.StartNew(async () =>
            {
                // Wait while channel is not empty and still not completed
                while (await reader.WaitToReadAsync())
                {
                    var job = await reader.ReadAsync();
                    ProcessJob(job);
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void Enqueue(ICacheJob job)
        {
            _writer.TryWrite(job);
        }

        private void ProcessJob(ICacheJob result)
        {
            if (result.GetType() == typeof(AddToCache))
            {
                var r = result as AddToCache;
                Add(r.SiteInfo, r.Rendering, r.Item);
            }
            else if (result.GetType() == typeof(DeleteFromCache))
            {
                var r = result as DeleteFromCache;
                Delete(r.SiteInfo, r.ItemId);
            }
            else if (result.GetType() == typeof(DeleteSiteFromCache))
            {
                var r = result as DeleteSiteFromCache;
                DeleteSite(r.SiteInfo);
            }
        }
        public static ItemAccessTracker Instance => lazy.Value;
        
        private void Add(SiteInfo siteInfo, Rendering rendering, Item item)
        {
            if (rendering.Caching.Cacheable)
            {
                string renderingId = rendering.UniqueId.ToString();
                string cacheableTemplates = rendering.RenderingItem.InnerItem.Fields["CacheableTemplates"].Value;

                bool globalTemplateContainsItem = globalCacheableTemplateIDs.Contains(item.TemplateID.Guid);

                var siteLangKey = "_#site:" + siteInfo.Name + "#lang:" + siteInfo.Language;

                if (!string.IsNullOrEmpty(renderingId) && (cacheableTemplates.Contains(item.TemplateID.ToString()) || globalTemplateContainsItem))
                {
                    if (!RenderingIdKey_ItemIDsValue_Dic.ContainsKey(siteLangKey))
                    {
                        RenderingIdKey_ItemIDsValue_Dic.Add(siteLangKey, new SafeDictionary<string, HashSet<string>>());
                    }

                    if (!RenderingIdKey_ItemIDsValue_Dic[siteLangKey].ContainsKey(renderingId))
                    {
                        RenderingIdKey_ItemIDsValue_Dic[siteLangKey].Add(renderingId, new HashSet<string>());
                    }

                    RenderingIdKey_ItemIDsValue_Dic[siteLangKey][renderingId].Add(item.ID.ToString());
                }
            }
        }

        private void Delete(SiteInfo siteInfo, ID itemId)
        {
            var renderingIdKeysToRemove = new List<string>();

            var siteLangKey = "_#site:" + siteInfo.Name + "#lang:" + siteInfo.Language;

            RenderingIdKey_ItemIDsValue_Dic.TryGetValue(siteLangKey, out var renderingItemsList);

            string[] keys = renderingItemsList.ToKeyArray();
            foreach (string key in keys)
                if (renderingItemsList.TryGetValue(key, out HashSet<string> list))
                    if (list.Contains(itemId.ToString()))
                        renderingIdKeysToRemove.Add(key);



            foreach (string renderingId in renderingIdKeysToRemove)
            {
                RenderingIdKey_ItemIDsValue_Dic[siteLangKey].Remove(renderingId);
            }

        }

        private void DeleteSite(SiteInfo siteInfo)
        {
            var siteLangKey = "_#site:" + siteInfo.Name + "#lang:" + siteInfo.Language;
            if (RenderingIdKey_ItemIDsValue_Dic.ContainsKey(siteLangKey))
            {
                var contains = RenderingIdKey_ItemIDsValue_Dic.ContainsKey(siteLangKey);

                if (contains)
                {
                    RenderingIdKey_ItemIDsValue_Dic.Remove(siteLangKey);
                }
            }
        }

        public string[] GetByKey(SiteInfo siteInfo, string key)
        {
            var siteLangKey = "_#site:" + siteInfo.Name + "#lang:" + siteInfo.Language;

            RenderingIdKey_ItemIDsValue_Dic.TryGetValue(siteLangKey, out var renderingItemsList);

            if (renderingItemsList != null)
            {
                renderingItemsList.TryGetValue(key, out var itemList);

                return itemList?.ToArray();
            }

            return null;
        }
    }
}