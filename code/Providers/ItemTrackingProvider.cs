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
using Foundation.HtmlCache.Settings;
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
            if (item != null && HttpContext.Current != null && HttpContext.Current.Items.Contains("RenderingArgs") && Context.Database.Name == "web")
            {
                var renderingProcessorArgs = (RenderingProcessorArgs) HttpContext.Current.Items["RenderingArgs"];

                ItemAccessTracker.Instance.Enqueue(new AddToCache(Context.Site.SiteInfo, item, renderingProcessorArgs));
            }
            return item;
        }

        public override Item GetItem(string itemPath, Language language, Version version, Database database,
            SecurityCheck securityCheck)
        {
            Item item = base.GetItem(itemPath, language, version, database, securityCheck);
            if (item != null && HttpContext.Current != null && HttpContext.Current.Items.Contains("RenderingArgs") && Context.Database.Name == "web")
            {
                var renderingProcessorArgs = (RenderingProcessorArgs)HttpContext.Current.Items["RenderingArgs"];

                ItemAccessTracker.Instance.Enqueue(new AddToCache(Context.Site.SiteInfo, item, renderingProcessorArgs));
            }
            return item;
        }

        protected override Item GetItem(ID itemId, Language language, Version version, Database database)
        {
            Item item = base.GetItem(itemId, language, version, database);
            if (item != null && HttpContext.Current != null && HttpContext.Current.Items.Contains("RenderingArgs") && Context.Database.Name == "web")
            {
                var renderingProcessorArgs = (RenderingProcessorArgs)HttpContext.Current.Items["RenderingArgs"];

                ItemAccessTracker.Instance.Enqueue(new AddToCache(Context.Site.SiteInfo, item, renderingProcessorArgs));
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

        // runningEN / <cachekey> / {ItemIDS}
        private readonly Dictionary<string, Dictionary<string, HashSet<string>>> RenderingIdKey_ItemIDsValue_Dic;
        
        private ChannelWriter<ICacheJob> _writer;

        private ItemAccessTracker()
        {
            RenderingIdKey_ItemIDsValue_Dic = new Dictionary<string, Dictionary<string, HashSet<string>>>();
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
                Add(r.SiteInfo, r.RenderingProcessorArgs, r.Item);
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
        
        private void Add(SiteInfo siteInfo, RenderingProcessorArgs renderingProcessorArgs, Item item)
        {
            if (renderingProcessorArgs.Rendering.Caching.Cacheable && !string.IsNullOrEmpty(renderingProcessorArgs.CacheKey))
            {
                string renderingId = renderingProcessorArgs.Rendering.UniqueId.ToString();
                string cacheableTemplates = renderingProcessorArgs.Rendering.RenderingItem.InnerItem.Fields["CacheableTemplates"].Value;

                bool globalTemplateContainsItem = GlobalCacheTemplateSettings.Instance.GlobalCacheableTemplateIDs.Contains(item.TemplateID.Guid);

                var siteLangKey = "_#site:" + siteInfo.Name + "#lang:" + siteInfo.Language;

                if (!string.IsNullOrEmpty(renderingId) && (cacheableTemplates.Contains(item.TemplateID.ToString()) || globalTemplateContainsItem))
                {
                    if (!RenderingIdKey_ItemIDsValue_Dic.ContainsKey(siteLangKey))
                    {
                        RenderingIdKey_ItemIDsValue_Dic.Add(siteLangKey, new Dictionary<string, HashSet<string>>());
                    }

                    if (!RenderingIdKey_ItemIDsValue_Dic[siteLangKey].ContainsKey(renderingProcessorArgs.CacheKey))
                    {
                        RenderingIdKey_ItemIDsValue_Dic[siteLangKey].Add(renderingProcessorArgs.CacheKey, new HashSet<string>());
                    }

                    RenderingIdKey_ItemIDsValue_Dic[siteLangKey][renderingProcessorArgs.CacheKey].Add(item.ID.ToString());
                    RenderingIdKey_ItemIDsValue_Dic[siteLangKey][renderingProcessorArgs.CacheKey]
                        .Add(renderingProcessorArgs.PageItem.ID.ToString());
                }
            }
        }

        private void Delete(SiteInfo siteInfo, ID itemId)
        {
            var cacheKeysToRemove = new HashSet<string>();

            var siteLangKey = "_#site:" + siteInfo.Name + "#lang:" + siteInfo.Language;

            RenderingIdKey_ItemIDsValue_Dic.TryGetValue(siteLangKey, out var renderingItemsList);

            string[] keys = renderingItemsList?.Keys.ToArray();
            if (keys != null)
            {
                foreach (string key in keys)
                    if (renderingItemsList.TryGetValue(key, out HashSet<string> list))
                        if (list.Contains(itemId.ToString()))
                            cacheKeysToRemove.Add(key);
            }



            foreach (string cacheKey in cacheKeysToRemove)
            {
                RenderingIdKey_ItemIDsValue_Dic[siteLangKey].Remove(cacheKey);
                siteInfo.HtmlCache.RemoveKeysContaining(cacheKey);
            }
            

        }

        private void DeleteSite(SiteInfo siteInfo)
        {
            var siteLangKey = "_#site:" + siteInfo.Name + "#lang:" + siteInfo.Language;
            if (RenderingIdKey_ItemIDsValue_Dic.ContainsKey(siteLangKey))
            {
                RenderingIdKey_ItemIDsValue_Dic.Remove(siteLangKey);
            }
        }
    }
}