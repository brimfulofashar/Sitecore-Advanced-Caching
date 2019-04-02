using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Rules.Conditions;

namespace SitecoreAdvancedCaching.Rules
{
    public class RenderingPublishedCondition<T> : WhenCondition<T> where T : ExtendedRuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, nameof(ruleContext));
            using (var context = ContentSearchManager.GetIndex(ruleContext.IndexName).CreateSearchContext())
            {
                var publishedItem = ruleContext.SearchResultItem;
                if (publishedItem == null)
                    return false;

                var renderingsQueryResults = context.GetQueryable<SearchResultItem>()
                    .Where(i => !string.IsNullOrEmpty(i["RenderingRules"])).ToList();

                var renderingPublished = renderingsQueryResults.Any(x => x.ItemId == publishedItem.ItemId);

                if (renderingPublished) Event.RaiseEvent("caching", publishedItem.ItemId.Guid);

                return renderingPublished;
            }
        }
    }
}