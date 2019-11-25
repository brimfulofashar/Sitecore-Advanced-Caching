using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Settings;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Web;

namespace Foundation.HtmlCache.Providers
{
    public sealed class ItemTrackingStore
    {
        private static readonly Lazy<ItemTrackingStore>
            lazy =
                new Lazy<ItemTrackingStore>
                    (() => new ItemTrackingStore());

        private readonly object lockObj = new object();

        // runningEN / <cachekey> / {ItemIDS}
        private readonly Dictionary<string, Dictionary<string, HashSet<string>>> RenderingIdKey_ItemIDsValue_Dic;

        private readonly ConcurrentQueue<ICacheJob> _queue;

        private readonly Timer _timer;

        private readonly int _timerInterval =
            int.Parse(Sitecore.Configuration.Settings.GetSetting("ItemTrackingQueueProcessInterval"));

        private ItemTrackingStore()
        {
            RenderingIdKey_ItemIDsValue_Dic = new Dictionary<string, Dictionary<string, HashSet<string>>>();
            _queue = new ConcurrentQueue<ICacheJob>();
            _timer = new Timer(_timerInterval);
            _timer.Elapsed += _timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Start();
        }

        public static ItemTrackingStore Instance => lazy.Value;

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ProcessCacheQueue();
        }

        public void Enqueue(ICacheJob job)
        {
            _queue.Enqueue(job);
        }


        private void ProcessCacheQueue()
        {
            _timer.Stop();
            Log.Info(string.Format("Tracking Queue contains {0} entries", _queue.Count), this);
            while (_queue.Count > 0)
            {
                _queue.TryDequeue(out ICacheJob job);
                if (job != null)
                    lock (lockObj)
                    {
                        ProcessJob(job);
                    }
            }

            _timer.Start();
        }

        private void ProcessJob(ICacheJob result)
        {
            if (result.GetType() == typeof(AddToCache))
            {
                var r = result as AddToCache;
                Add(r.SiteInfo, r.RenderingProcessorArgs);
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

        private void Add(SiteInfo siteInfo, RenderingProcessorArgs renderingProcessorArgs)
        {
            if (renderingProcessorArgs.Cacheable && !string.IsNullOrEmpty(renderingProcessorArgs.CacheKey))
            {
                string siteLangKey = "_#site:" + siteInfo.Name + "#lang:" + siteInfo.Language;

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

        private void Delete(SiteInfo siteInfo, ID itemId)
        {
            var cacheKeysToRemove = new HashSet<string>();

            string siteLangKey = "_#site:" + siteInfo.Name + "#lang:" + siteInfo.Language;

            RenderingIdKey_ItemIDsValue_Dic.TryGetValue(siteLangKey,
                out Dictionary<string, HashSet<string>> renderingItemsList);

            string[] keys = renderingItemsList?.Keys.ToArray();
            if (keys != null)
                foreach (string key in keys)
                    if (renderingItemsList.TryGetValue(key, out HashSet<string> list))
                        if (list.Contains(itemId.Guid.ToString()))
                            cacheKeysToRemove.Add(key);


            foreach (string cacheKey in cacheKeysToRemove)
            {
                RenderingIdKey_ItemIDsValue_Dic[siteLangKey].Remove(cacheKey);
                siteInfo.HtmlCache.RemoveKeysContaining(cacheKey);
            }
        }

        private void DeleteSite(SiteInfo siteInfo)
        {
            string siteLangKey = "_#site:" + siteInfo.Name + "#lang:" + siteInfo.Language;
            if (RenderingIdKey_ItemIDsValue_Dic.ContainsKey(siteLangKey))
                RenderingIdKey_ItemIDsValue_Dic.Remove(siteLangKey);
        }
    }
}