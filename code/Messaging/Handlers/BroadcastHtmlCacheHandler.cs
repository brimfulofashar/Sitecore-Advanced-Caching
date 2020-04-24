using System.Threading.Tasks;
using Foundation.HtmlCache.Messaging.Message;
using Sitecore.Caching;
using Sitecore.Framework.Messaging;
using Sitecore.Sites;

namespace Foundation.HtmlCache.Messaging.Handlers
{
    public class BroadcastHtmlCacheHandler : IMessageHandler<BroadcastHtmlCacheMessage>
    {
        public Task Handle(BroadcastHtmlCacheMessage message, IMessageReceiveContext receiveContext,
            IMessageReplyContext replyContext)
        {
            SiteContext siteContext = SiteContext.GetSite(message.SiteName);
            if (message.ToRemove)
            {
                if (!string.IsNullOrEmpty(message.HtmlCacheKey))
                {
                    foreach (var cacheKey in message.HtmlCacheKey.Split('|'))
                    {
                        CacheManager.GetHtmlCache(siteContext).RemoveKeysContaining(cacheKey);
                    }
                }
                else
                {
                    CacheManager.GetHtmlCache(siteContext).Clear();
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