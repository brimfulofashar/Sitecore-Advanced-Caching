using System;
using System.Runtime.Serialization;
using Sitecore.Eventing;
using Foundation.HtmlCache.Models;

namespace Foundation.HtmlCache.Events
{
    [DataContract]
    public class ClearCacheArgs : IHasEventName
    {
        public ClearCacheArgs(string eventName, Guid? itemId, string language, ClearCacheOperation.ClearCacheOperationEnum clearCacheOperationEnum)
        {
            EventName = eventName;
            ItemId = itemId;
            ClearCacheOperationEnum = clearCacheOperationEnum;
            Language = language;
        }

        [DataMember] public ClearCacheOperation.ClearCacheOperationEnum ClearCacheOperationEnum { get; protected set; }

        [DataMember] public Guid? ItemId { get; protected set; }

        [DataMember] public string Language { get; protected set; }

        [DataMember] public string EventName { get; protected set; }
    }
}