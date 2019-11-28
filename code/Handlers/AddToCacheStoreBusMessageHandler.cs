using Foundation.HtmlCache.Messages;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Providers;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Framework.Messaging;
using Task = System.Threading.Tasks.Task;

namespace Foundation.HtmlCache.Handlers
{
    public class AddToCacheStoreBusMessageHandler : IMessageHandler<AddToCacheStore>
    {
        public Task Handle(AddToCacheStore message, IMessageReceiveContext receiveContext, IMessageReplyContext replyContext)
        {
            Item cachedItem = Factory.GetDatabase("web").GetItem("/sitecore/system/Modules/HtmlCache/" + message.Name);
            if (cachedItem == null)
            {
                cachedItem = Factory.GetDatabase("web").GetItem("/sitecore/system/Modules/HtmlCache").Add(message.Name, new TemplateID(new ID("{987E6DC4-F4E6-4BE8-8349-A4513244A112}")));
            }

            cachedItem.Editing.BeginEdit();
            cachedItem.Fields["CacheKey"].Value = message.CacheKey;
            cachedItem.Fields["RenderingId"].Value = message.RenderingId;
            cachedItem.Fields["CachedHtml"].Value = message.CachedHtml;
            cachedItem.Fields["SiteName"].Value = message.SiteInfoName;
            cachedItem.Fields["SiteLanguage"].Value = message.SiteInfoLanguage;
            cachedItem.Editing.EndEdit();
            return Task.CompletedTask;
        }
    }
}