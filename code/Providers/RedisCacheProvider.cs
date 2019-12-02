using System;
using Foundation.HtmlCache.Connections;
using Foundation.HtmlCache.Messages;
using Newtonsoft.Json;
using Sitecore.Diagnostics;

using StackExchange.Redis;

namespace Foundation.HtmlCache.Providers
{
    public class RedisCacheProvider : IRedisCacheProvider
    {
        private readonly IRedisSharedConnection redisSharedConnection;
        public int Database { get; }

        public RedisCacheProvider(IRedisSharedConnection redisSharedConnection)
        {
            this.redisSharedConnection = redisSharedConnection;
            Database = int.Parse(Sitecore.Configuration.Settings.GetSetting("redisDatabase"));
        }

        public ICacheMessage Get<ICacheMessage>(string key) where ICacheMessage : Messages.ICacheMessage
        {
            try
            {
                RedisValue redisValue = this.redisSharedConnection.ConnectionMultiplexer.GetDatabase(Database).StringGet(key);
                return JsonConvert.DeserializeObject<ICacheMessage>(redisValue);
            }
            catch (Exception e)
            {
                Log.Error($"Failed to Get Redis entry with key {key}", e, this);
                throw;
            }
        }

        public void Set(string key, ICacheMessage value, double duration)
        {
            try
            {
                if (duration > 0)
                {
                    this.redisSharedConnection.ConnectionMultiplexer.GetDatabase(Database).StringSet(key,JsonConvert.SerializeObject(value),TimeSpan.FromSeconds(duration));
                }
                else
                {
                    this.redisSharedConnection.ConnectionMultiplexer.GetDatabase(Database).StringSet(key, JsonConvert.SerializeObject(value));
                }
            }
            catch (Exception e)
            {
                Log.Error($"Failed to set Redis entry with key {key}", e, this);
            }
        }

        public void Publish(string channel, string message)
        {
            try
            {
                this.redisSharedConnection.ConnectionMultiplexer.GetSubscriber().Publish(channel, message);
            }
            catch (Exception e)
            {
                Log.Error($"Failed to Publish Redis message {message} to channel {channel}", e, this);
            }
        }
    }
}