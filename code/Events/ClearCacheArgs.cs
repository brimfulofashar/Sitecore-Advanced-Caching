using System.Collections.Generic;
using System.Runtime.Serialization;
using Foundation.HtmlCache.DB;
using Foundation.HtmlCache.Models;
using Sitecore.Eventing;

namespace Foundation.HtmlCache.Events
{
    [DataContract]
    public class ClearCacheArgs : IHasEventName
    {
        public ClearCacheArgs(string eventName, CacheSiteLangKeys cacheSiteLangKeys, ClearCacheOperation.ClearCacheOperationEnum clearCacheOperationEnum)
        {
            EventName = eventName;
            CacheSiteLangKeys = cacheSiteLangKeys;
            ClearCacheOperationEnum = clearCacheOperationEnum;
        }

        [DataMember] public ClearCacheOperation.ClearCacheOperationEnum ClearCacheOperationEnum { get; protected set; }

        [DataMember] public CacheSiteLangKeys CacheSiteLangKeys { get; set; }

        [DataMember] public string EventName { get; protected set; }
    }
}