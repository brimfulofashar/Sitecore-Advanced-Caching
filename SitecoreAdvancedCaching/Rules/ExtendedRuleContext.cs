using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Rules;

namespace SitecoreAdvancedCaching.Rules
{
    public class ExtendedRuleContext : RuleContext
    {
        public Item ItemToCompare { get; set; }
        public Item PageItem { get; set; }
    }
}