using System.Web;
using Foundation.HtmlCache.Helpers;
using Sitecore;
using Sitecore.Mvc.Pipelines.Request.RequestEnd;

namespace Foundation.HtmlCache.Pipelines
{
    public class EndTrackingContext: RequestEndProcessor
    {
        public override void Process(RequestEndArgs args)
        {
            var tvpHelper = HttpContext.Current.Items[TVPHelper.HttpContextKey] as TVPHelper;

            foreach (var htmlCacheKey in tvpHelper.Tracker.Keys)
            {
                var renderingProcessorArgs = tvpHelper.Tracker[htmlCacheKey];
                foreach (var itemMetaData in renderingProcessorArgs.ItemAccessList)
                {
                    tvpHelper.ProcessTrackingData(Sitecore.Context.Site.SiteInfo.Name,
                        Context.Site.SiteInfo.Language, renderingProcessorArgs.CacheKey,
                        renderingProcessorArgs.CacheResult, itemMetaData.Id, itemMetaData.Language);
                }
            }

            HttpContext.Current.Items[TVPHelper.HttpContextKey] = tvpHelper;
        }
    }
}