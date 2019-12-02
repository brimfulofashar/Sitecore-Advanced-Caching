using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Foundation.HtmlCache.Connections;
using Foundation.HtmlCache.Messages;
using Foundation.HtmlCache.Providers;
using Newtonsoft.Json;
using Sitecore.Pipelines;
using StackExchange.Redis;

namespace Foundation.HtmlCache.Pipelines
{
    public class InitializePersistedHtmlCache
    {
        public void Initialize(PipelineArgs args)
        {
            IRedisSharedConnection redis = DependencyResolver.Current.GetServices<IRedisSharedConnection>().First();
            IRedisCacheProvider redisCacheProvider = DependencyResolver.Current.GetServices<IRedisCacheProvider>().First();
            System.Net.EndPoint[] endpoints = redis?.ConnectionMultiplexer.GetEndPoints();
            if (endpoints != null)
            {
                foreach (var endpoint in endpoints)
                {
                    var server = redis.ConnectionMultiplexer.GetServer(endpoint);
                    if (server.IsSlave)
                    {
                        var db = redis.ConnectionMultiplexer.GetDatabase(redisCacheProvider.Database);
                        foreach (var key in server.Keys())
                        {
                            string value = db.StringGet(key);
                            var addToCache = JsonConvert.DeserializeObject<AddToCacheStore>(value);

                            ItemTrackingStore.Instance.PersistedHtmlCache.Add(addToCache.CacheKey, new KeyValuePair<string, string>(addToCache.RenderingId, addToCache.CachedHtml));
                        }
                    }
                }
            }

        }
    }
}