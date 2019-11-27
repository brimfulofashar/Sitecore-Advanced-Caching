using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

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
                    var appDataFolder = Sitecore.Configuration.Settings.GetSetting("PersistentCacheFolder");

                    var htmlCacheFolder = HttpContext.Current.Server.MapPath(appDataFolder);

                    var site = Sitecore.Context.Site;

                    var siteName = site.Name;

                    if (!string.IsNullOrEmpty(siteName))
                    {
                        var siteNameFolder = htmlCacheFolder + "/" + siteName;
                        if (!Directory.Exists(siteNameFolder))
                        {
                            Directory.CreateDirectory(siteNameFolder);
                        }

                        if (!string.IsNullOrEmpty(site.Language))
                        {
                            var siteLanguageFolder = siteNameFolder + "/" + site.Language;
                            if (!Directory.Exists(siteLanguageFolder))
                            {
                                Directory.CreateDirectory(siteLanguageFolder);
                            }
                        }
                    }

                    var matchCollection = new Regex("[a-zA-Z0-9]+").Matches(args.CacheKey);
                    var fileName = htmlCacheFolder + "/" + site.Name + "/" + site.Language + "/" + string.Join("_", matchCollection.Cast<Match>().Select(m => m.Value));

                    RecordingTextWriter writer = args.Writer as RecordingTextWriter;
                    if (writer != null)
                    {
                        string recording = writer.GetRecording();

                        File.WriteAllText(fileName, recording);
                    }
                }
            }
        }
    }
}