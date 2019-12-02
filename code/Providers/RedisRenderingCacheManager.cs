using System.Linq;
using System.Web.Mvc;
using Foundation.HtmlCache.Messages;

namespace Foundation.HtmlCache.Providers
{
    public class RedisRenderingCacheManager : IRedisRenderingCacheManager
    {
        public RedisRenderingCacheManager()
        {
            this.RedisCacheProvider = DependencyResolver.Current.GetServices<IRedisCacheProvider>().FirstOrDefault();
        }

        public IRedisCacheProvider RedisCacheProvider { get; }

        public ICacheMessage GetRenderingCacheValue(string key)
        {
            var cacheValue = this.RedisCacheProvider.Get<ICacheMessage>(key);

            return cacheValue;
        }
    }
}