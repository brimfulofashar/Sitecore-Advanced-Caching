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
            var itemHasLayout = false;
            bool.TryParse(ruleContext.SearchResultItem.Fields["itemhaslayout"].ToString(), out itemHasLayout);
            return itemHasLayout;
        }
    }
}