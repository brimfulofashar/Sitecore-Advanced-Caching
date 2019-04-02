using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using Sitecore.Rules;

namespace SitecoreAdvancedCaching.Rules
{
    public class ExtendedRuleContext : RuleContext
    {
        public string IndexName { get; set; }
        public SearchResultItem SearchResultItem { get; set; }
    }
}