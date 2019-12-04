using System;
using System.Configuration;
using Foundation.HtmlCache.Messages;
using Newtonsoft.Json;
using Sitecore.Diagnostics;

using StackExchange.Redis;

namespace Foundation.HtmlCache.Providers
{
    public class RedisCacheProvider : IRedisCacheProvider
    {
        public IDatabase Database { get; }

        public RedisCacheProvider()
        {
            var redisConnectionString = ConfigurationManager.ConnectionStrings["redis"].ConnectionString;
            this.ConnectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
            Database = this.ConnectionMultiplexer.GetDatabase(int.Parse(Sitecore.Configuration.Settings.GetSetting("redisDatabase")));
        }

        public ConnectionMultiplexer ConnectionMultiplexer { get; }

        public ICacheMessage Get<ICacheMessage>(string key) where ICacheMessage : Messages.ICacheMessage
        {
            try
            {
                RedisValue redisValue = this.Database.StringGet(key);
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
                    this.Database.StringSet(key,JsonConvert.SerializeObject(value),TimeSpan.FromSeconds(duration));
                }
                else
                {
                    this.Database.StringSet(key, JsonConvert.SerializeObject(value));
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
                this.ConnectionMultiplexer.GetSubscriber().Publish(channel, message);
            }
            catch (Exception e)
            {
                Log.Error($"Failed to Publish Redis message {message} to channel {channel}", e, this);
            }
        }
    }
}