using System;
using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Diagnostics;
using Sitecore.Rules.Conditions;

namespace SitecoreAdvancedCaching.Rules
{
    public class DatasourcePublishedCondition<T> : WhenCondition<T> where T : ExtendedRuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, nameof(ruleContext));
            var renderingItem = ruleContext.Item;
            if (renderingItem == null)
                return false;

            var publishedItem = ruleContext.SearchResultItem;
            if (publishedItem == null)
                return false;

            using (var context = ContentSearchManager.GetIndex(ruleContext.IndexName).CreateSearchContext())
            {
                var pageItemsWithRenderingQueryResult = context.GetQueryable<SearchResultItem>().Where(i => i["ItemDatasources"].Contains(publishedItem.ItemId.Guid.ToString())).ToList();

                foreach (var pageItem in pageItemsWithRenderingQueryResult)
                {
                    // if a field is published then we don't know by which rendering it's used by, we invalidated the page

                    // else we invalidate the rendering.
                }
            }

            return true;
        }
    }
}