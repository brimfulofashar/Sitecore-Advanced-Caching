using Foundation.HtmlCache.Messages;
using StackExchange.Redis;

namespace Foundation.HtmlCache.Providers
{
    public interface IRedisCacheProvider
    {
        IDatabase Database { get; }
        T Get<T>(string key) where T : ICacheMessage;

        void Set(string key, ICacheMessage value, double duration);

        void Publish(string channel, string message);

        ConnectionMultiplexer ConnectionMultiplexer { get; }
    }
}