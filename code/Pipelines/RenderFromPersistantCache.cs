using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Foundation.HtmlCache.Pipelines
{
    public class RenderFromPersistantCache : AddRecordedHtmlToCache
    {
        public override void Process(RenderRenderingArgs args)
        {
            if (args.Rendering.RenderingType == "r" && !args.UsedCache && args.Cacheable &&
                !string.IsNullOrEmpty(args.CacheKey))
            {
                var appDataFolder = Sitecore.Configuration.Settings.GetSetting("PersistentCacheFolder");

                var htmlCacheFolder = HttpContext.Current.Server.MapPath(appDataFolder);

                var site = Sitecore.Context.Site;

                var matchCollection = new Regex("[a-zA-Z0-9]+").Matches(args.CacheKey);
                var fileName = htmlCacheFolder + "/" + site.Name + "/" + site.Language + "/" + string.Join("_", matchCollection.Cast<Match>().Select(m => m.Value));

                if (File.Exists(fileName))
                {
                    var html = File.ReadAllText(fileName);

                    args.Writer.Write(html);

                    base.Process(args);

                    args.UsedCache = true;
                    args.Cacheable = false;
                    args.Rendered = true;
                }
            }
        }
    }
}