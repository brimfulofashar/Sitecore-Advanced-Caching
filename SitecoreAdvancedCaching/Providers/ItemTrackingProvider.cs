using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Abstractions;
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

        public readonly Dictionary<string, List<ID>> ItemIdKey_RenderingIDsValue_Dic;

        private ItemAccessTracker()
        {
            ItemIdKey_RenderingIDsValue_Dic = new Dictionary<string, List<ID>>();
            _globalCacheableTemplateIDs = Sitecore.Configuration.Settings.GetSetting("GlobalCacheableTemplateIDs").Split('|').Where(x => !string.IsNullOrEmpty(x)).Select(x => ID.Parse(x)).ToList();
        }

        private readonly List<ID> _globalCacheableTemplateIDs;
        
        public static ItemAccessTracker Instance => lazy.Value;

        public void Add(Item item)
        {
            var rendering = (Rendering) HttpContext.Current.Items["Rendering"];
            if (rendering.Caching.Cacheable)
            {
                var renderingId = rendering.UniqueId.ToString();
                var cacheableTemplates = rendering.RenderingItem.InnerItem.Fields["CacheableTemplates"].Value;
                if (!string.IsNullOrEmpty(renderingId) && (cacheableTemplates.Contains(item.TemplateID.ToString()) || _globalCacheableTemplateIDs.Any(x => x == item.TemplateID)))
                {
                    if (!ItemIdKey_RenderingIDsValue_Dic.ContainsKey(renderingId))
                        ItemIdKey_RenderingIDsValue_Dic.Add(renderingId, new List<ID> {item.ID});
                    else if (!ItemIdKey_RenderingIDsValue_Dic[renderingId].Contains(item.ID))
                        ItemIdKey_RenderingIDsValue_Dic[renderingId].Add(item.ID);
                }
            }
        }

        public void Remove(ID itemId)
        {
            if (HttpContext.Current.Items["RenderingId"] != null)
            {
                var renderingId = HttpContext.Current.Items["RenderingId"].ToString();
                if (!string.IsNullOrEmpty(renderingId))
                    if (!ItemIdKey_RenderingIDsValue_Dic.ContainsKey(renderingId))
                        ItemIdKey_RenderingIDsValue_Dic.Remove(renderingId);
            }
        }
    }
}