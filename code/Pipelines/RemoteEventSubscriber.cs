using System;
using Sitecore.Data.Events;
using Sitecore.Diagnostics;
using Sitecore.Eventing;
using Sitecore.Events;
using Sitecore.Pipelines;
using Foundation.HtmlCache.Events;

namespace Foundation.HtmlCache.Pipelines
{
    public class RemoteEventSubscriber
    {
        public void Initialize(PipelineArgs args)
        {
            Log.Info("HtmlCache: Initializing remote event subscribers", this);
            var itemPublishedAction = new Action<ItemPublishedArgs>(RaiseRemoteEvent);
            EventManager.Subscribe(itemPublishedAction);
            var clearCacheAction = new Action<ClearCacheArgs>(RaiseRemoteEvent);
            EventManager.Subscribe(clearCacheAction);
        }

        private static void RaiseRemoteEvent<TEvent>(TEvent @event) where TEvent : IHasEventName
        {
            RemoteEventArgs<TEvent> remoteEventArgs = new RemoteEventArgs<TEvent>(@event);
            Event.RaiseEvent(@event.EventName, (IPassNativeEventArgs)remoteEventArgs);
        }
    }
}