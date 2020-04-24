using System.Threading.Tasks;
using Foundation.HtmlCache.Messaging.Message;
using Sitecore.Caching;
using Sitecore.Framework.Messaging;
using Sitecore.Sites;

namespace Foundation.HtmlCache.Messaging.Handlers
{
    public class BroadcastHtmlCacheHandler : IMessageHandler<BroadcastHtmlCacheMessage>
    {
        public BroadcastHtmlCacheHandler()
        {

        }

        public Task Handle(BroadcastHtmlCacheMessage message, IMessageReceiveContext receiveContext,
            IMessageReplyContext replyContext)
        {
            SiteContext siteContext = SiteContext.GetSite(message.SiteName);
            if (message.ToRemove)
            {
                foreach (var cacheKey in message.HtmlCacheKey.Split('|'))
                {
                    CacheManager.GetHtmlCache(siteContext).RemoveKeysContaining(cacheKey);
                }
            }
            else
            {
                CacheManager.GetHtmlCache(siteContext).SetHtml(
                    message.HtmlCacheKey,
                    message.HtmlCacheResult);
            }


            return Task.CompletedTask;
        }
    }
}