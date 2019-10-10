using System;
using System.Linq;
using Sitecore.Abstractions;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using SitecoreAdvancedCaching.Models;
using SitecoreAdvancedCaching.Providers;

namespace SitecoreAdvancedCaching.Events
{
    public class IndexingEndEvent
    {
        private readonly string _indexName = Settings.GetSetting("IndexName");

        public void CaclculateRenderingCachesToClear(object sender, EventArgs args)
        {
            var indexName = ContentSearchManager.Locator.GetInstance<IEvent>().ExtractParameter(args, 0).ToString() ==
                            _indexName;
            var rebuild = (bool) ContentSearchManager.Locator.GetInstance<IEvent>().ExtractParameter(args, 1);
            if (indexName && !rebuild)
                using (var context = ContentSearchManager.GetIndex(_indexName).CreateSearchContext())
                {
                    var publishedItemsQueryResult = context.GetQueryable<CacheSearchResultItem>().Where(i =>
                        i["PublishingTimestamp"] == PublishingStartEvent.PublishingStartTimestamp).ToList();

                    foreach (var publishedItem in publishedItemsQueryResult.OrderBy(x => x.Path))
                        if (ItemAccessTracker.Instance.ItemIdKey_RenderingIDsValue_Dic[publishedItem.ItemId] != null)
                            Factory.GetSiteInfo("habitat").HtmlCache
                                .RemoveKeysContaining(publishedItem.ItemId.ToString());
                }
        }
    }
}