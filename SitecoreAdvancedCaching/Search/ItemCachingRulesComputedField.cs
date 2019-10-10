using System.Collections;
using System.Linq;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;
using SitecoreAdvancedCaching.Models;

namespace SitecoreAdvancedCaching.Search
{
    public class ItemCachingRulesComputedField : IComputedIndexField
    {
        private readonly ID _cachingTemplateId = new ID("{E8D2DD19-1347-4562-AE3F-310DC0B21A6C}");
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            Item item = indexable as SitecoreIndexableItem;
            if (item != null && item.Template.BaseTemplates.Any(x => x.ID == _cachingTemplateId))
            {
//                var rule =  item.Fields["Rules"].Value;
//
//                return !string.IsNullOrEmpty(rule) ? rule : null;

                var pageIsPublishedSetting = item.Fields["PageIsPublished"].Value;

                Node<IDModel> node = new Node<IDModel>();

                foreach (var id in pageIsPublishedSetting.Split('|').Where(x => !string.IsNullOrEmpty(x)))
                {
                    Item option = item.Database.GetItem(id);

                    if (option != null)
                    {
                        var selectionString = option.Fields["SelectionString"].Value;

                        var pageReferers = GetReferrers(item.ID.ToString());

                        switch (selectionString)
                        {
                            case "I":
                            {
                                
                                break;
                            }
                            case "C":
                            {
                                break;
                            }
                            case "D":
                            {
                                break;
                            }
                            case "R":
                            {
                                break;
                            }
                        }
                    }
                }

                return null;
            }

            return null;
        }


        public Item[] GetReferrers(string itemId)
        {
            Item item = Sitecore.Data.Database.GetDatabase("web").GetItem(new Sitecore.Data.ID(itemId));
            // getting all linked Items that refer to the Item
            ItemLink[] itemLinks = Globals.LinkDatabase.GetReferrers(item);
            if (itemLinks == null)
            {
                return null;
            }
            else
            {
                ArrayList items = new ArrayList(itemLinks.Length);
                foreach (ItemLink itemLink in itemLinks)
                {
                    // comparing the database name of the linked Item
                    if (itemLink.SourceDatabaseName == "web")
                    {
                        Item linkItem = Sitecore.Data.Database.GetDatabase("web").Items[itemLink.SourceItemID];
                        if (linkItem != null)
                        {
                            items.Add(linkItem);
                        }
                    }
                }
                return (Item[])items.ToArray(typeof(Item));
            }
        }
    }
}