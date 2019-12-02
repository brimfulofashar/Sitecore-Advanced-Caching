using Foundation.HtmlCache.Messages;

namespace Foundation.HtmlCache.Providers
{
    public interface IRedisRenderingCacheManager
    {
        ICacheMessage GetRenderingCacheValue(string key);
    }
}