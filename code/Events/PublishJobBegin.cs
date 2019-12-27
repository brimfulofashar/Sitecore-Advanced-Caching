﻿using System;
using Foundation.HtmlCache.Models;
using Sitecore.Events;
using Sitecore.Publishing;

namespace Foundation.HtmlCache.Events
{
    public class PublishJobBegin
    {
        public void BeginPublishTracking(object sender, EventArgs args)
        {
            Publisher publisher = Event.ExtractParameter(args, 0) as Publisher;
            if (publisher != null)
            {
                var sourceDb = publisher.Options.SourceDatabase.Name;
                var destinationDb = publisher.Options.TargetDatabase.Name;
                if (Sitecore.Context.Items.Contains(PublishItemTracking.Name))
                {
                    Sitecore.Context.Items.Add(PublishItemTracking.Name,
                        new PublishItemTracking(sourceDb, destinationDb));
                }
            }

        }
    }
}