using System.Configuration;
using StackExchange.Redis;

namespace Foundation.HtmlCache.Connections
{
    public class RedisSharedConnection : IRedisSharedConnection
    {
        public RedisSharedConnection()
        {
            var cacheConnectionString = Sitecore.Configuration.Settings.GetSetting("redisConnectionString");
            ConfigurationManager.ConnectionStrings.Add(new ConnectionStringSettings("redisConnectionString", cacheConnectionString));
            this.ConnectionMultiplexer = ConnectionMultiplexer.Connect(ConfigurationManager.ConnectionStrings["redisConnectionString"].ConnectionString);
        }

        public ConnectionMultiplexer ConnectionMultiplexer { get; }
    }
}