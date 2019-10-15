using System;
using Sitecore.Data.Events;
using Sitecore.Diagnostics;
using Sitecore.Eventing;
using Sitecore.Events;
using Sitecore.Pipelines;
using SitecoreAdvancedCaching.Events;

namespace SitecoreAdvancedCaching.Pipelines
{
    public class ItemPublishingWatchProcesser
    {
        public void Initialize(PipelineArgs args)
        {
            Log.Info("Initializing publish item watcher", this);
            var action = new Action<ItemPublishedArgs>(RaiseRemoteEvent);
            EventManager.Subscribe(action);
        }

        private static void RaiseRemoteEvent<TEvent>(TEvent @event) where TEvent : IHasEventName
        {
            RemoteEventArgs<TEvent> remoteEventArgs = new RemoteEventArgs<TEvent>(@event);
            Event.RaiseEvent(@event.EventName, (IPassNativeEventArgs)remoteEventArgs);
        }
    }
}