using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Foundation.HtmlCache.Messages;
using Foundation.HtmlCache.Providers;
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

                            var addToCacheStore = new AddToCacheStore(Context.Site.SiteInfo.Name, Context.Site.SiteInfo.Language, args.CacheKey, args.Rendering.Id.ToString(), recording, cacheKey);
                            IRedisCacheProvider redis = DependencyResolver.Current.GetServices<IRedisCacheProvider>().FirstOrDefault();
                            if (redis != null)
                            {
                                redis.Set(cacheKey, addToCacheStore,
                                    args.Rendering.Caching.Timeout.TotalMilliseconds);
                                redis.Publish(Context.Site.Name + "_" + Context.Site.Language, cacheKey);
                            }
                        }
                    }
                }
            }
        }
    }
}