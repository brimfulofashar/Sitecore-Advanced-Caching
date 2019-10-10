using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using SitecoreAdvancedCaching.Helpers;
using SitecoreAdvancedCaching.Models;

namespace SitecoreAdvancedCaching.Search
{
    public class ItemDatasourcesComputedField : IComputedIndexField
    {
        private readonly string DatasourceRootsToIndex = Settings.GetSetting("DatasourceRootsToIndex");

        private readonly Regex IDRegex =
            new Regex(
                @"(\{)[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\})");

        private Item indexableItem;
        private readonly string IndexName = Settings.GetSetting("IndexName");
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var idMap = new Dictionary<ID, bool>();

            indexableItem = indexable as SitecoreIndexableItem;
            if (indexableItem != null && IsIndexable(indexableItem))
            {
                var renderings = ItemLayoutHelper.GetDeviceDefinition(indexableItem);

                var sb = new StringBuilder();

                if (renderings.Any())
                {
                    foreach (var rendering in renderings)
                    {
                        if (!string.IsNullOrEmpty(rendering.Datasource))
                            foreach (var match in IDRegex.Matches(rendering.Datasource))
                            {
                                var ID = new ID(Guid.Parse(match.ToString()));
                                var currentItem = indexableItem.Database.GetItem(ID);
                                if ((!idMap.ContainsKey(ID) || !idMap[ID]) && IsIndexable(currentItem) &&
                                    currentItem.ID != indexableItem.ID)
                                    idMap.Add(ID, false);
                            }

                        if (!string.IsNullOrEmpty(rendering.Parameters))
                            foreach (var match in IDRegex.Matches(rendering.Parameters))
                            {
                                var ID = new ID(Guid.Parse(match.ToString()));
                                var currentItem = indexableItem.Database.GetItem(ID);
                                if ((!idMap.ContainsKey(ID) || !idMap[ID]) && IsIndexable(currentItem) &&
                                    currentItem.ID != indexableItem.ID)
                                    idMap.Add(ID, false);
                            }
                    }

                    foreach (Field field in indexableItem.Fields)
                        if (!IsStandardField(field))
                            if (IDRegex.IsMatch(field.Value))
                                foreach (var match in IDRegex.Matches(field.Value))
                                {
                                    var ID = new ID(Guid.Parse(match.ToString()));
                                    var currentItem = indexableItem.Database.GetItem(ID);
                                    if ((!idMap.ContainsKey(ID) || !idMap[ID]) && IsIndexable(currentItem) &&
                                        currentItem.ID != indexableItem.ID)
                                        idMap.Add(ID, false);
                                }

                    var computedIdMap = ItemWalker(idMap, indexableItem.Database);

                    foreach (var key in computedIdMap.Keys) sb.Append("{" + key.Guid + "}|");

                    return sb.ToString().Trim('|');
                }

                var tempItem = (SitecoreIndexableItem) indexableItem;
                if (tempItem.Item != null)
                    using (var context = ContentSearchManager.GetIndex(IndexName).CreateSearchContext())
                    {
                        var baseQuery = PredicateBuilder.True<CacheSearchResultItem>();
                        var containsQuery = PredicateBuilder.True<CacheSearchResultItem>();

                        foreach (var id in tempItem.Item.Paths.LongID.Split('/')
                            .Where(x => !string.IsNullOrEmpty(x)))
                            containsQuery = containsQuery.Or(x =>
                                x.ItemDatasources.Contains(id.ToLower()) && x.ItemHasLayout);

                        baseQuery = baseQuery.And(containsQuery);
                        var queryRunner = context.GetQueryable<CacheSearchResultItem>().Where(baseQuery).ToList();
                    }
            }

            return null;
        }

        private bool IsIndexable(Item item)
        {
            return item != null && DatasourceRootsToIndex.Split('|').Where(x => !string.IsNullOrEmpty(x))
                       .Any(x => item.Paths.FullPath.Contains(x));
        }

        private Dictionary<ID, bool> ItemWalker(Dictionary<ID, bool> idMap, Database database)
        {
            if (idMap.All(x => x.Value)) return idMap;
            var temp = idMap.ToDictionary(entry => entry.Key, entry => entry.Value);
            foreach (var id in temp.Keys.Where(x => !temp[x]))
            {
                var item = database.GetItem(id);
                idMap[id] = true;
                foreach (Field field in item.Fields)
                    if (!IsStandardField(field))
                        if (IDRegex.IsMatch(field.Value))
                            foreach (var match in IDRegex.Matches(field.Value))
                            {
                                var ID = new ID(Guid.Parse(match.ToString()));
                                var currentItem = database.GetItem(ID);
                                if ((!idMap.ContainsKey(ID) || !idMap[ID]) && IsIndexable(currentItem) &&
                                    currentItem.ID != indexableItem.ID) idMap.Add(ID, false);
                            }

                foreach (Item child in item.Children)
                    if (!ItemLayoutHelper.ItemHasLayout(child))
                        if ((!idMap.ContainsKey(child.ID) || !idMap[child.ID]) && IsIndexable(child) &&
                            child.ID != indexableItem.ID)
                            idMap.Add(child.ID, false);
            }

            return ItemWalker(idMap, database);
        }

        private bool IsStandardField(Field field)
        {
            var template = TemplateManager.GetTemplate(
                Settings.DefaultBaseTemplate,
                field.Database);
            Assert.IsNotNull(template, "template");
            return template.ContainsField(field.ID);
        }
    }
}