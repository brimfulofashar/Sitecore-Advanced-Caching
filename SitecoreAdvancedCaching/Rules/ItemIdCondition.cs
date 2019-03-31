using Sitecore.Diagnostics;
using Sitecore.Rules.Conditions;

namespace SitecoreAdvancedCaching.Rules
{
    public class ItemIdCondition<T> : StringOperatorCondition<T> where T : ExtendedRuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, nameof(ruleContext));
            var obj = ruleContext.Item;
            if (obj == null)
                return false;
            return ruleContext.ItemToCompare != null && ruleContext.ItemToCompare == ruleContext.Item;
        }
    }
}