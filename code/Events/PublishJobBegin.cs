using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Foundation.HtmlCache.Models;
using Sitecore.Diagnostics;
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
                Sitecore.Context.Items.Add(PublishItemTracking.Name, new PublishItemTracking(sourceDb, destinationDb));
            }

        }
    }
}