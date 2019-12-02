using System.Configuration;
using StackExchange.Redis;

namespace Foundation.HtmlCache.Connections
{
    public class RedisSharedConnection : IRedisSharedConnection
    {
        public RedisSharedConnection()
        {
            this.ConnectionMultiplexer = ConnectionMultiplexer.Connect(ConfigurationManager.ConnectionStrings["cache"].ConnectionString);
        }

        public ConnectionMultiplexer ConnectionMultiplexer { get; }
    }
}