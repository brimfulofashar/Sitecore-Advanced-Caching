using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
                ItemAccessTracker.Instance.Enqueue(new AddToCache(rendering, item));
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
                ItemAccessTracker.Instance.Enqueue(new AddToCache(rendering, item));
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
                ItemAccessTracker.Instance.Enqueue(new AddToCache(rendering, item));
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

        private readonly SafeDictionary<string, HashSet<string>> RenderingIdKey_ItemIDsValue_Dic;

        private ConcurrentQueue<ICacheJob> _cacheQueue = new ConcurrentQueue<ICacheJob>();

        private ItemAccessTracker()
        {
            RenderingIdKey_ItemIDsValue_Dic = new SafeDictionary<string, HashSet<string>>();
            List<ID> globalCacheableTemplateIDsTemp = Settings.GetSetting("GlobalCacheableTemplateIDs").Split('|')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => ID.Parse(x)).ToList();
            globalCacheableTemplateIDs = new HashSet<Guid>();
            foreach (ID g in globalCacheableTemplateIDsTemp) globalCacheableTemplateIDs.Add(g.Guid);

            var thread = new Thread(new ThreadStart(OnStart));
            thread.IsBackground = true;
            thread.Start();
        }

        public void Enqueue(ICacheJob job)
        {
            _cacheQueue.Enqueue(job);
        }

        private void OnStart()
        {
            while (true)
            {
                if (_cacheQueue.TryDequeue(out var result))
                {
                    if (result.GetType() == typeof(AddToCache))
                    {
                        var r = result as AddToCache;
                        Add(r.Rendering,r.Item);
                    }
                    else if (result.GetType() == typeof(DeleteFromCache))
                    {
                        var r = result as DeleteFromCache;
                        Delete(r.ItemId);
                    }
                }
            }
        }

        public static ItemAccessTracker Instance => lazy.Value;

        
        private void Add(Rendering rendering, Item item)
        {
            if (rendering.Caching.Cacheable)
            {
                string renderingId = rendering.UniqueId.ToString();
                string cacheableTemplates = rendering.RenderingItem.InnerItem.Fields["CacheableTemplates"].Value;

                bool globalTemplateContainsItem = globalCacheableTemplateIDs.Contains(item.TemplateID.Guid);

                if (!string.IsNullOrEmpty(renderingId) && (cacheableTemplates.Contains(item.TemplateID.ToString()) || globalTemplateContainsItem))
                {
                    if (!RenderingIdKey_ItemIDsValue_Dic.ContainsKey(renderingId))
                    {
                        RenderingIdKey_ItemIDsValue_Dic.Add(renderingId, new HashSet<string>());
                    }
                    RenderingIdKey_ItemIDsValue_Dic[renderingId].Add(item.ID.ToString());
                }
            }
        }

        private void Delete(ID itemId)
        {
            var keysToRemove = new List<string>();

            string[] keys = RenderingIdKey_ItemIDsValue_Dic.Keys.ToArray();
            foreach (string key in keys)
                if (RenderingIdKey_ItemIDsValue_Dic.TryGetValue(key, out HashSet<string> list))
                    if (list.Contains(itemId.ToString()))
                        keysToRemove.Add(key);

            foreach (string key in keysToRemove)
                RenderingIdKey_ItemIDsValue_Dic.Remove(key);
        }

        public string[] GetByKey(string key)
        {
            RenderingIdKey_ItemIDsValue_Dic.TryGetValue(key, out var list);

            return list?.ToArray();
        }
    }
}