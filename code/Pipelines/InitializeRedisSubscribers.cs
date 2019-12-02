using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Foundation.HtmlCache.Connections;
using Foundation.HtmlCache.Providers;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Jobs;
using Sitecore.Pipelines;
using Sitecore.Sites;
using Sitecore.Web;

using StackExchange.Redis;

namespace Foundation.HtmlCache.Pipelines
{
    public class InitializeRedisSubscribers
    {
        private readonly IRedisRenderingCacheManager _redisRenderingCacheManager;

        private readonly IRedisSharedConnection _redisSharedConnection;

        public InitializeRedisSubscribers()
        {
            this._redisRenderingCacheManager = DependencyResolver.Current.GetServices<IRedisRenderingCacheManager>().FirstOrDefault();
            this._redisSharedConnection = DependencyResolver.Current.GetServices<IRedisSharedConnection>().FirstOrDefault();
        }

        public void Initialize(PipelineArgs args)
        {
            Log.Info("Initializing Redis Subscriber", this);

            this.StartJob();
        }

        public void Start(SiteInfo site)
        {
            this._redisSharedConnection.ConnectionMultiplexer.GetSubscriber().Subscribe(
                site.Name + "_" + site.Language,
                (channel, message) => this.FireReceiveEvent(message));
        }

        public void StartJob()
        {
            List<SiteInfo> sites = Factory.GetSiteInfoList();
            foreach (SiteInfo site in sites)
            {
                var jobOptions = new JobOptions(
                    "RedisSubscriber",
                    "RedisSubscriber",
                    site.Name,
                    this,
                    "Start",
                    new object[] { site });

                Log.Info($"Redis subscriber job starting for {site.Name}", this);
                JobManager.Start(jobOptions);
            }
        }

        private void FireReceiveEvent(RedisValue key)
        {
            var cacheValue = this._redisRenderingCacheManager.GetRenderingCacheValue(key);
            cacheValue?.Handle();
        }
    }
}