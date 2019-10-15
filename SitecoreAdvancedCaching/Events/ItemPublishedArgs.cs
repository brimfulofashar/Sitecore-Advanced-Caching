using System;
using System.Runtime.Serialization;
using Sitecore.Eventing;
using SitecoreAdvancedCaching.Models;

namespace SitecoreAdvancedCaching.Events
{
    [DataContract]
    public class ItemPublishedArgs : IHasEventName
    {
        public ItemPublishedArgs(string eventName, Guid itemId, PublishOperation.PublishOperationEnum publishOperationEnum)
        {
            EventName = eventName;
            ItemId = itemId;
            PublishOperationEnum = publishOperationEnum;
        }

        [DataMember] public PublishOperation.PublishOperationEnum PublishOperationEnum { get; protected set; }

        [DataMember] public Guid ItemId { get; protected set; }

        [DataMember] public string EventName { get; protected set; }
    }
}