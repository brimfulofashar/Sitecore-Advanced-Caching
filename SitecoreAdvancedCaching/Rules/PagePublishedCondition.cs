using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Rules.Conditions;

namespace SitecoreAdvancedCaching.Rules
{
    public class PagePublishedCondition<T> : WhenCondition<T> where T : ExtendedRuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, nameof(ruleContext));

            var publishedItem = ruleContext.SearchResultItem;
            if (publishedItem == null)
                return false;

            bool.TryParse(publishedItem.Fields["itemhaslayout"].ToString(), out var itemHasLayout);
            if (itemHasLayout) Event.RaiseEvent("caching", publishedItem.ItemId.Guid);

            return itemHasLayout;
        }
    }
}