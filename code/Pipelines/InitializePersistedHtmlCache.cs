using System.Linq;
using System.Web.Mvc;
using Foundation.HtmlCache.Connections;
using Sitecore.Pipelines;

namespace Foundation.HtmlCache.Pipelines
{
    public class InitializePersistedHtmlCache
    {
        public void Initialize(PipelineArgs args)
        {
            IRedisSharedConnection redis = DependencyResolver.Current.GetServices<IRedisSharedConnection>().FirstOrDefault();
            System.Net.EndPoint[] endpoints = redis?.ConnectionMultiplexer.GetEndPoints();
            if (endpoints != null)
            {
                foreach (var endpoint in endpoints)
                {
                    var server = redis.ConnectionMultiplexer.GetServer(endpoint);
                    if (server.IsSlave)
                    {
                        var db = redis.ConnectionMultiplexer.GetDatabase();
                        var values = db.StringGet(server.Keys().ToArray());
                    }
                }
            }

        }
    }
}