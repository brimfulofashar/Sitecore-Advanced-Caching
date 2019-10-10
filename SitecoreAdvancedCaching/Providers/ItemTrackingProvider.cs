using System;
using System.Collections.Generic;
using System.Web;
using Sitecore.Abstractions;
using Sitecore.Collections;
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
        public ItemTrackingProvider() : base()
        {

        }

        public ItemTrackingProvider(BaseLanguageManager languageManager) : base(languageManager)
        {
        }

        public override ChildList GetChildren(Item item, SecurityCheck securityCheck, ChildListOptions options)
        {
            var childList = base.GetChildren(item, securityCheck, options);

            foreach (var child in childList.InnerChildren)
            {
                ItemAccessTracker.Instance.Add(child.ID);
            }
            return childList;
        }

        public override Item GetItem(ID itemId, Language language, Version version, Database database,
            SecurityCheck securityCheck)
        {
            var item = base.GetItem(itemId, language, version, database, securityCheck);
            if (item != null)
            {
                ItemAccessTracker.Instance.Add(item.ID);
            }
            return item;
        }

        /// <summary>Gets an item from a database.</summary>
        /// <param name="itemPath">The item path.</param>
        /// <param name="language">The language of the item to get.</param>
        /// <param name="version">The version of the item to get.</param>
        /// <param name="database">The database.</param>
        /// <param name="securityCheck">The security check.</param>
        /// <returns>
        ///     The item. If no item is found, <c>null</c> is returned.
        /// </returns>
        public override Item GetItem(string itemPath, Language language, Version version, Database database,
            SecurityCheck securityCheck)
        {
            var item = base.GetItem(itemPath, language, version, database, securityCheck);
            if (item != null)
            {
                ItemAccessTracker.Instance.Add(item.ID);
            }
            return item;
        }

        /// <summary>Gets the parent of an item.</summary>
        /// <param name="item">The item.</param>
        /// <param name="securityCheck">The security check.</param>
        /// <returns>The parent item.</returns>
        public override Item GetParent(Item item, SecurityCheck securityCheck)
        {
            var parent = base.GetParent(item, securityCheck);
            if (parent != null)
            {
                ItemAccessTracker.Instance.Add(parent.ID);
            }

            return parent;
        }

        protected override Item GetItem(ID itemId, Language language, Version version, Database database)
        {
            var item = base.GetItem(itemId, language, version, database);
            if (item != null)
            {
                ItemAccessTracker.Instance.Add(item.ID);
            }

            return item;
        }

        /// <summary>Gets the parent of an item.</summary>
        /// <param name="item">The item.</param>
        /// <returns>The parent item.</returns>
        protected override Item GetParent(Item item)
        {
            var parent = base.GetParent(item);
            if (parent != null)
            {
                ItemAccessTracker.Instance.Add(parent.ID);
            }
            return parent;
        }
    }

    public sealed class ItemAccessTracker
    {
        private static readonly Lazy<ItemAccessTracker>
            lazy =
                new Lazy<ItemAccessTracker>
                    (() => new ItemAccessTracker());

        public readonly Dictionary<ID, List<string>> ItemIdKey_RenderingIDsValue_Dic;
//        public readonly Dictionary<string, List<ID>> RenderingIdKey_ItemsIDsValue_Dic;

        private ItemAccessTracker()
        {
//            RenderingIdKey_ItemsIDsValue_Dic = new Dictionary<string, List<ID>>();
            ItemIdKey_RenderingIDsValue_Dic = new Dictionary<ID, List<string>>();
        }

        public void Add(ID itemId)
        {
            if (HttpContext.Current != null && HttpContext.Current.Items["Rendering"] != null)
            {
                var rendering = (Rendering)HttpContext.Current.Items["Rendering"];
                if (rendering.Caching.Cacheable)
                {
                    var renderingId = rendering.UniqueId.ToString();
                    if (!string.IsNullOrEmpty(renderingId))
                    {

                        if (!ItemIdKey_RenderingIDsValue_Dic.ContainsKey(itemId))
                        {
                            ItemIdKey_RenderingIDsValue_Dic.Add(itemId, new List<string> {renderingId});
                        }
                        else if (!ItemIdKey_RenderingIDsValue_Dic[itemId].Contains(renderingId))
                        {
                            ItemIdKey_RenderingIDsValue_Dic[itemId].Add(renderingId);
                        }
                    }
                }
            }

//            if (!RenderingIdKey_ItemsIDsValue_Dic.ContainsKey(renderingId))
//            {
//                RenderingIdKey_ItemsIDsValue_Dic.Add(renderingId, new List<ID> { itemId });
//            }
//            else if(!RenderingIdKey_ItemsIDsValue_Dic[renderingId].Contains(renderingId))
//            {
//                RenderingIdKey_ItemsIDsValue_Dic[renderingId].Add(itemId);
//            }
        }

        public void Remove(ID itemId)
        {
            if (HttpContext.Current.Items["RenderingId"] != null)
            {
                var renderingId = HttpContext.Current.Items["RenderingId"].ToString();
                if (!string.IsNullOrEmpty(renderingId))
                    if (!ItemIdKey_RenderingIDsValue_Dic.ContainsKey(itemId))
                    {
                        ItemIdKey_RenderingIDsValue_Dic.Remove(itemId);
                    }
            }
        }

//            if (!RenderingIdKey_ItemsIDsValue_Dic.ContainsKey(renderingId))
//            {
//                RenderingIdKey_ItemsIDsValue_Dic.Add(renderingId, new List<ID> { itemId });
//            }
//            else if (!RenderingIdKey_ItemsIDsValue_Dic[renderingId].Contains(renderingId))
//            {
//                RenderingIdKey_ItemsIDsValue_Dic[renderingId].Add(itemId);
//            }

        public static ItemAccessTracker Instance => lazy.Value;
    }
}