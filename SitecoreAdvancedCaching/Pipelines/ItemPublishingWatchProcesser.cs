using System;
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
            var action = new Action<ItemPublishedEvent>(RaiseRemoteEvent);
            EventManager.Subscribe(action);
        }

        private void RaiseRemoteEvent(ItemPublishedEvent myEvent)
        {
            Event.RaiseEvent("publish:itemProcessed:Remote", myEvent);
        }
    }
}