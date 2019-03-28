using System;
using System.Linq;
using Sitecore.Abstractions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Managers;
using Sitecore.Rules;

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
                    var renderingsQueryResults = context.GetQueryable<SearchResultItem>().Where(i => !string.IsNullOrEmpty(i["RenderingRules"])).ToList();

                    var publishedItemsQueryResult = context.GetQueryable<SearchResultItem>().Where(i => i["PublishingTimestamp"] == PublishingStartEvent.PublishingStartTimestamp).ToList();

                    foreach (var publishedItem in publishedItemsQueryResult)
                    {
                        // if a page is published then we invalidate all caches for that page
                        var pageRenderings = publishedItem.Fields["itemrenderings"].ToString();
                        if (!string.IsNullOrEmpty(pageRenderings))
                        {
                            // cache clear where iid = publishedItem.ID
                            Sitecore.Events.Event.RaiseEvent("caching", "_#iid:" + publishedItem.ItemId.Guid);
                        }
                        else
                        {
                            foreach (var renderings in renderingsQueryResults)
                            {
                                var xmlRules = renderings.Fields["renderingrules"].ToString();
                                if (!string.IsNullOrEmpty(xmlRules))
                                {
                                    var rules = RuleFactory.ParseRules<RuleContext>(Database.GetDatabase("web"), xmlRules);
                                    foreach (var rule in rules.Rules)
                                    {
                                        var result = rule.Evaluate(new RuleContext());
                                        if (result)
                                        {

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
}