using System.IO;
using System.Web;
using Foundation.HtmlCache.Providers;
using Sitecore.Pipelines;

namespace Foundation.HtmlCache.Pipelines
{
    public class InitializePersistedHtmlCache
    {
        public void Initialize(PipelineArgs args)
        {
            var appDataFolder = Sitecore.Configuration.Settings.GetSetting("PersistentCacheFolder");

            var htmlCacheFolder = HttpContext.Current.Server.MapPath(appDataFolder);

            foreach (var file in Directory.GetFiles(htmlCacheFolder, "*.*", SearchOption.AllDirectories))
            {
                var html = File.ReadAllText(file);
                ItemTrackingStore.Instance.PersistedHtmlCache.Add(new FileInfo(file).Name, html);
            }
        }
    }
}