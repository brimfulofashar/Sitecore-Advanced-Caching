using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace SitecoreAdvancedCaching.Models
{
    public class CacheSearchResultItem : SearchResultItem
    {
        [IndexField("itemhaslayout")]
        [DataMember]
        public virtual bool ItemHasLayout { get; set; }

        [IndexField("itemrenderings")]
        [DataMember]
        public virtual string ItemRenderings { get; set; }

        [IndexField("publishingtimestamp")]
        [DataMember]
        public virtual string PublishingTimestamp { get; set; }
        [IndexField("cachingrules")]
        [DataMember]
        public virtual string CachingRules { get; set; }
        [IndexField("itemdatasources")]
        [DataMember]
        public virtual string ItemDatasources { get; set; }


    }
}