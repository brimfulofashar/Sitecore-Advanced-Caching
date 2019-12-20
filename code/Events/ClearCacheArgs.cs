using System.Collections.Generic;
using System.Runtime.Serialization;
using Foundation.HtmlCache.Models;
using Sitecore.Eventing;

namespace Foundation.HtmlCache.Events
{
    [DataContract]
    public class ClearCacheArgs : IHasEventName
    {
        public ClearCacheArgs(string eventName, Dictionary<string, Dictionary<string, List<string>>> nameLangKeys, ClearCacheOperation.ClearCacheOperationEnum clearCacheOperationEnum)
        {
            EventName = eventName;
            NameLangKeys = nameLangKeys;
            ClearCacheOperationEnum = clearCacheOperationEnum;
        }

        [DataMember] public ClearCacheOperation.ClearCacheOperationEnum ClearCacheOperationEnum { get; protected set; }

        [DataMember] public List<string> Languages { get; protected set; }

        [DataMember] public List<string> HtmlCacheKeys { get; protected set; }

        [DataMember] public Dictionary<string, Dictionary<string, List<string>>> NameLangKeys { get; set; }

        [DataMember] public string EventName { get; protected set; }
    }
}