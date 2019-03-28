using System;
using System.Linq;
using Sitecore.Abstractions;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Managers;
using Sitecore.Events;

namespace SitecoreAdvancedCaching.Events
{
    public class HtmlCacheClearingEvent
    {
        public void ClearHtmlCache(object sender, EventArgs args)
        {
            var eventParam = Event.ExtractParameter(args, 0).ToString();

            var eventTokens = eventParam.Split(':');
            var key = eventTokens[0];
            var value = eventTokens[1];
            switch (key)
            {
                case "_#iid":
                {
                    Factory.GetSiteInfo("website").HtmlCache.RemoveKeysContaining(value);
                    break;
                }
            }
        }
    }
}