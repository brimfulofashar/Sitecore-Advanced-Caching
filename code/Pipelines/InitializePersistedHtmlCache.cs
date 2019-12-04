using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
            IRedisCacheProvider redisCacheProvider = DependencyResolver.Current.GetServices<IRedisCacheProvider>().First();
            System.Net.EndPoint[] endpoints = redisCacheProvider.ConnectionMultiplexer.GetEndPoints();
            if (endpoints != null)
            {
                foreach (var endpoint in endpoints)
                {
                    var server = redisCacheProvider.ConnectionMultiplexer.GetServer(endpoint);
                    if (server.IsSlave)
                    {
                        foreach (var key in server.Keys())
                        {
                            string value = redisCacheProvider.Database.StringGet(key);
                            var addToCache = JsonConvert.DeserializeObject<AddToCacheStore>(value);

                            ItemTrackingStore.Instance.PersistedHtmlCache.Add(addToCache.CacheKey, new KeyValuePair<string, string>(addToCache.RenderingId, addToCache.CachedHtml));
                        }
                    }
                }
            }

        }
    }
}