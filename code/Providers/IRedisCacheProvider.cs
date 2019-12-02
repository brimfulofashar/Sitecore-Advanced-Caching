using Foundation.HtmlCache.Messages;

namespace Foundation.HtmlCache.Providers
{
    public interface IRedisCacheProvider
    {
        int Database { get; }
        T Get<T>(string key) where T : ICacheMessage;

        void Set(string key, ICacheMessage value, double duration);

        void Publish(string channel, string message);
    }
}