using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Foundation.HtmlCache.Messages;
using Foundation.HtmlCache.Providers;
using Newtonsoft.Json;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Jobs;
using Sitecore.Pipelines;
using Sitecore.Web;

using StackExchange.Redis;

namespace Foundation.HtmlCache.Pipelines
{
    public class InitializeRedisSubscribers
    {
        private readonly IRedisCacheProvider _redisCacheProvider;

        private string _role;

        public InitializeRedisSubscribers()
        {
            this._redisCacheProvider = DependencyResolver.Current.GetServices<IRedisCacheProvider>().FirstOrDefault();
            _role = ConfigurationManager.AppSettings["role:define"];
        }

        public void Initialize(PipelineArgs args)
        {
            Log.Info("Initializing Redis Subscriber", this);

            this.StartJob();
        }

        public void Start(string siteNameLang)
        {
            var setting = Sitecore.Configuration.Settings.GetSetting("redisCompetingConsumerMessageTypes");
            if (!string.IsNullOrEmpty(setting))
            {
                var redisCompetingConsumerMessageTypes = setting.Split('|').Where(x => !string.IsNullOrEmpty(x)).ToList();

                foreach (var redisCompetingConsumerMessageType in redisCompetingConsumerMessageTypes)
                {
                    this._redisCacheProvider.ConnectionMultiplexer.GetSubscriber().Subscribe(siteNameLang, delegate
                    {
                        var message = this._redisCacheProvider.Database.ListRightPop(redisCompetingConsumerMessageType);
                        this.ConsumeAndProcess(message);
                    });
                }
            }

            this._redisCacheProvider.ConnectionMultiplexer.GetSubscriber().Subscribe(
                siteNameLang,
                (channel, message) => this.Process(message));
        }

        public void StartJob()
        {
            List<SiteInfo> sites = Factory.GetSiteInfoList();
            foreach (SiteInfo site in sites)
            {
                var jobOptions = new DefaultJobOptions(
                    "RedisSubscriber",
                    "RedisSubscriber",
                    site.Name,
                    this,
                    "Start",
                    new object[] { site.Name + "_" + site.Language });

                Log.Info($"Redis subscriber job starting for {site.Name}", this);
                JobManager.Start(jobOptions);
            }
        }

        private void Process(RedisValue redisValue)
        {
            if (!string.IsNullOrEmpty(redisValue))
            {
                var stringVal = redisValue.ToString();
                ICacheMessage cacheValue = JsonConvert.DeserializeObject<ICacheMessage>(stringVal);
                cacheValue?.Handle();
            }
        }

        private void ConsumeAndProcess(RedisValue redisValue)
        {
            if (!string.IsNullOrEmpty(redisValue))
            {
                var stringVal = redisValue.ToString();
                ICacheMessage cacheValue = JsonConvert.DeserializeObject<ICacheMessage>(stringVal,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                        NullValueHandling = NullValueHandling.Ignore,
                    });
                cacheValue?.Handle();
            }
        }
    }
}