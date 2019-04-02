using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Items;

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
                var rule =  item.Fields["Rules"].Value;

                return !string.IsNullOrEmpty(rule) ? rule : null;
            }

            return null;
        }
    }
}