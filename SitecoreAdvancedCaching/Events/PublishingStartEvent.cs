using System;
using System.Linq;
using Sitecore.Abstractions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace SitecoreAdvancedCaching.Events
{
    public class PublishingStartEvent
    {
        public static string PublishingStartTimestamp { get; private set; }

        public void SetPublishingStartTimestamp(object sender, EventArgs args)
        {
            PublishingStartTimestamp = DateTime.UtcNow.ToString("u");
        }
    }
}