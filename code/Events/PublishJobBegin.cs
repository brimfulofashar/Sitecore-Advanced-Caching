using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Foundation.HtmlCache.Extensions;
using Foundation.HtmlCache.Models;
using Sitecore.Events;
using Sitecore.Globalization;
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
                BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
                FieldInfo field = typeof(Publisher).GetField("languages", flags);
                IEnumerable<Language> languages = field.GetValue(publisher) as IEnumerable<Language>;

                var sourceDb = publisher.Options.SourceDatabase.Name;
                var destinationDb = publisher.Options.TargetDatabase.Name;
                if (!Sitecore.Context.Items.Contains(PublishItemTracking.Name))
                {
                    Sitecore.Context.Items.Add(PublishItemTracking.Name,
                        new PublishItemTracking(sourceDb, destinationDb, languages.Select(x => x.Name).ToList()));
                }
            }

        }
    }
}