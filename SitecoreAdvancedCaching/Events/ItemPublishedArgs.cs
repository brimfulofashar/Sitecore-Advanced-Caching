using System;
using System.Runtime.Serialization;
using Sitecore.Eventing;

namespace SitecoreAdvancedCaching.Events
{
    [DataContract]
    public class ItemPublishedArgs : IHasEventName
    {
        public ItemPublishedArgs(string eventName, Guid itemId, bool itemIsDeleted)
        {
            EventName = eventName;
            ItemId = itemId;
            ItemIsDeleted = itemIsDeleted;
        }

        [DataMember] public bool ItemIsDeleted { get; protected set; }

        [DataMember] public Guid ItemId { get; protected set; }

        [DataMember] public string EventName { get; protected set; }
    }
}