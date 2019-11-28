using System.Linq;
using System.Text.RegularExpressions;
using Foundation.HtmlCache.Bus;
using Foundation.HtmlCache.Messages;
using Sitecore;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using Sitecore.SecurityModel;

namespace Foundation.HtmlCache.Pipelines
{
    public class SaveToPersistantCache : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            if (args.Rendering.RenderingType == "r" && !args.UsedCache && args.Cacheable && !string.IsNullOrEmpty(args.CacheKey) && !args.Rendering.Caching.VaryByUser)
            {
                bool.TryParse(Sitecore.Configuration.Settings.GetSetting("PersistRecordedHtml"), out var persistRecordedHtml);
                if (persistRecordedHtml)
                {
                    RecordingTextWriter writer = args.Writer as RecordingTextWriter;
                    if (writer != null)
                    {
                        string recording = writer.GetRecording();

                        using (new SecurityDisabler())
                        {
                            var matchCollection = new Regex("[a-zA-Z0-9]+").Matches(args.CacheKey);
                            string cacheKey = string.Join("_", matchCollection.Cast<Match>().Select(m => m.Value));

                            var addToCacheStore = new AddToCacheStore(Context.Site.SiteInfo.Name, Context.Site.SiteInfo.Language, args.CacheKey, args.Rendering.Item.ID.ToString(), recording, cacheKey);
                            HtmlCacheMessageBus.Send(addToCacheStore);
                        }
                    }
                }
            }
        }
    }
}