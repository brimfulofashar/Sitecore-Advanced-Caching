using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Items;
using SitecoreAdvancedCaching.Events;

namespace SitecoreAdvancedCaching.Search
{
    public class PublishingTimestampComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            return PublishingStartEvent.PublishingStartTimestamp;
        }
    }
}