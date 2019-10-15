using System;
using System.Runtime.Serialization;
using Sitecore.Eventing;

namespace SitecoreAdvancedCaching.Events
{
    [DataContract]
    public class ItemPublishedArgs : IHasEventName
    {
        public ItemPublishedArgs(string instanceName, string eventName, Guid itemId, bool itemIsDeleted)
        {
            InstanceName = instanceName;
            EventName = eventName;
            ItemId = itemId;
            ItemIsDeleted = itemIsDeleted;
        }

        [DataMember] public string InstanceName { get; protected set; }

        [DataMember] public bool ItemIsDeleted { get; protected set; }

        [DataMember] public Guid ItemId { get; protected set; }

        [DataMember] public string EventName { get; protected set; }
    }
}