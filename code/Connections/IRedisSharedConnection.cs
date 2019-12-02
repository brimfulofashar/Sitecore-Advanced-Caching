using StackExchange.Redis;

namespace Foundation.HtmlCache.Connections
{
    public interface IRedisSharedConnection
    {
        ConnectionMultiplexer ConnectionMultiplexer { get; }
    }
}