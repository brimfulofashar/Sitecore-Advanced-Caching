using System;
using System.Linq;
using Sitecore;
using Sitecore.Abstractions;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Rules;
using SitecoreAdvancedCaching.Rules;

namespace SitecoreAdvancedCaching.Events
{
    public class IndexingEndEvent
    {
        private const string _indexName = "sitecore_web_index";

        public void CaclculateRenderingCachesToClear(object sender, EventArgs args)
        {
            if (ContentSearchManager.Locator.GetInstance<IEvent>().ExtractParameter<string>(args, 0) == _indexName)
            {
                using (var context = ContentSearchManager.GetIndex(_indexName).CreateSearchContext())
                {
                    var publishedItemsQueryResult = context.GetQueryable<SearchResultItem>().Where(i => i["PublishingTimestamp"] == PublishingStartEvent.PublishingStartTimestamp).ToList();
                    var rendeingItemsWithCacheRulesQueryResult = context.GetQueryable<SearchResultItem>().Where(i => !string.IsNullOrEmpty(i["CachingRules"])).ToList();

                    foreach (var publishedItem in publishedItemsQueryResult)
                    {
                        // if a rendering is published then we simply invalidate all caches where the id is in cache
                        if (publishedItem.Fields.ContainsKey("cachingrules"))
                        {
                            Sitecore.Events.Event.RaiseEvent("caching", publishedItem.ItemId.Guid);
                        }
                        else
                        {
                            foreach (var renderingItem in rendeingItemsWithCacheRulesQueryResult)
                            {
                                var xmlRules = renderingItem.Fields["cachingrules"].ToString();
                                if (!string.IsNullOrEmpty(xmlRules))
                                {
                                    var rules = RuleFactory.ParseRules<ExtendedRuleContext>(Database.GetDatabase("web"), xmlRules);
                                    foreach (var rule in rules.Rules)
                                    {
                                        rule.Evaluate(new ExtendedRuleContext { SearchResultItem = publishedItem, Item = renderingItem.GetItem(), IndexName = _indexName });
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}