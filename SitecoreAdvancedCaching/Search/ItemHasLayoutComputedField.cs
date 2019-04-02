using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Layouts;

namespace SitecoreAdvancedCaching.Search
{
    public class ItemHasLayoutComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            Item item = indexable as SitecoreIndexableItem;
            if (item != null)
            {
                var layout = new LayoutField(item).Value;
                if (!string.IsNullOrEmpty(layout))
                {
//                    var layoutField = new LayoutField(item.Fields[FieldIDs.FinalLayoutField]);
//
//                    LayoutDefinition layout = LayoutDefinition.Parse(layoutField.Value);

                    return true;
                }
            }

            return false;
        }
    }
}