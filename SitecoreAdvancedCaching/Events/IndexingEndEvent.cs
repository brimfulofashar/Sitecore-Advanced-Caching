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
                    var renderingsQueryResults = context.GetQueryable<SearchResultItem>().Where(i => !string.IsNullOrEmpty(i["RenderingRules"])).ToList();

                    var pagesQueryResults = context.GetQueryable<SearchResultItem>().Where(i => !string.IsNullOrEmpty(i["ItemRenderings"])).ToList();

                    var publishedItemsQueryResult = context.GetQueryable<SearchResultItem>().Where(i => i["PublishingTimestamp"] == PublishingStartEvent.PublishingStartTimestamp).ToList();

                    foreach (var publishedItem in publishedItemsQueryResult)
                    {
                        // if a page is published then we invalidate all caches for that page
                        var pageRenderings = publishedItem.Fields["itemrenderings"].ToString();
                        if (!string.IsNullOrEmpty(pageRenderings))
                        {                            
                            Sitecore.Events.Event.RaiseEvent("caching", "_#iid:" + publishedItem.ItemId.Guid);
                        }
                        // if a rendering is published then we invalidate all caches where the rendering is used
                        else if (renderingsQueryResults.Any(x => x.ItemId == publishedItem.ItemId))
                        {
                            Sitecore.Events.Event.RaiseEvent("caching", "_#ruid:" + publishedItem.ItemId.Guid);
                        }
                        // else if must be a normal item and we must execute the rules engine.
                        else
                        {
                            foreach (var rendering in renderingsQueryResults)
                            {
                                var xmlRules = rendering.Fields["renderingrules"].ToString();
                                if (!string.IsNullOrEmpty(xmlRules))
                                {
                                    var referencedPageItems = pagesQueryResults.Where(x =>
                                            x.Fields["itemrenderings"].ToString()
                                                .Contains(rendering.ItemId.ToString()))
                                        .ToList();
                                    foreach (var referencedPageItem in referencedPageItems)
                                    {
                                        Item pageItem = referencedPageItem.GetItem();
                                        var rules = RuleFactory.ParseRules<RuleContext>(Database.GetDatabase("web"),
                                            xmlRules);
                                        foreach (var rule in rules.Rules)
                                        {

                                            var result = rule.Evaluate(new ExtendedRuleContext {PageItem = pageItem, ItemToCompare = publishedItem.GetItem()});
                                            if (result)
                                            {
                                                Sitecore.Events.Event.RaiseEvent("caching", "_#rid:" + rendering.ItemId.Guid);
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
}