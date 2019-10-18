using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Abstractions;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.Mvc.Presentation;
using Sitecore.SecurityModel;
using Version = Sitecore.Data.Version;

namespace SitecoreAdvancedCaching.Providers
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
            var item = base.GetItem(itemId, language, version, database, securityCheck);
            if (item != null && HttpContext.Current != null && HttpContext.Current.Items["Rendering"] != null &&
                Context.Database.Name == "web") ItemAccessTracker.Instance.Add(item);
            return item;
        }

        public override Item GetItem(string itemPath, Language language, Version version, Database database,
            SecurityCheck securityCheck)
        {
            var item = base.GetItem(itemPath, language, version, database, securityCheck);
            if (item != null && HttpContext.Current != null && HttpContext.Current.Items["Rendering"] != null &&
                Context.Database.Name == "web") ItemAccessTracker.Instance.Add(item);
            return item;
        }

        protected override Item GetItem(ID itemId, Language language, Version version, Database database)
        {
            var item = base.GetItem(itemId, language, version, database);
            if (item != null && HttpContext.Current != null && HttpContext.Current.Items["Rendering"] != null &&
                Context.Database.Name == "web") ItemAccessTracker.Instance.Add(item);

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

        public readonly Dictionary<string, List<ID>> RenderingIdKey_ItemIDsValue_Dic;

        private ItemAccessTracker()
        {
            RenderingIdKey_ItemIDsValue_Dic = new Dictionary<string, List<ID>>();
            var globalCacheableTemplateIDsTemp = Settings.GetSetting("GlobalCacheableTemplateIDs").Split('|')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => ID.Parse(x)).ToList();
            globalCacheableTemplateIDs = new HashSet<Guid>();
            foreach (var g in globalCacheableTemplateIDsTemp) globalCacheableTemplateIDs.Add(g.Guid);
        }

        public static ItemAccessTracker Instance => lazy.Value;

        public void Add(Item item)
        {
            var rendering = (Rendering) HttpContext.Current.Items["Rendering"];
            if (rendering.Caching.Cacheable)
            {
                var renderingId = rendering.UniqueId.ToString();
                var cacheableTemplates = rendering.RenderingItem.InnerItem.Fields["CacheableTemplates"].Value;

                var globalTemplateContainsItem = globalCacheableTemplateIDs.Contains(item.TemplateID.Guid);

                if (!string.IsNullOrEmpty(renderingId) &&
                    (cacheableTemplates.Contains(item.TemplateID.ToString()) || globalTemplateContainsItem))
                {
                    if (!RenderingIdKey_ItemIDsValue_Dic.ContainsKey(renderingId))
                        RenderingIdKey_ItemIDsValue_Dic.Add(renderingId, new List<ID> {item.ID});
                    else if (!RenderingIdKey_ItemIDsValue_Dic[renderingId].Contains(item.ID))
                        RenderingIdKey_ItemIDsValue_Dic[renderingId].Add(item.ID);
                }
            }
        }

        public void Remove(ID itemId)
        {
            var keysToRemove = new List<string>();

            foreach (var key in RenderingIdKey_ItemIDsValue_Dic.Keys)
                if (RenderingIdKey_ItemIDsValue_Dic[key].Contains(itemId))
                    keysToRemove.Add(key);

            foreach (var key in keysToRemove) RenderingIdKey_ItemIDsValue_Dic.Remove(key);
        }
    }
}