using System.Web;
using Foundation.HtmlCache.Helpers;
using Sitecore.Mvc.Pipelines.Request.RequestBegin;

namespace Foundation.HtmlCache.Pipelines
{
    public class BeginTrackingContext : RequestBeginProcessor
    {
        public override void Process(RequestBeginArgs args)
        {
            HttpContext.Current.Items[TVPHelper.HttpContextKey] = new TVPHelper();
        }
    }
}