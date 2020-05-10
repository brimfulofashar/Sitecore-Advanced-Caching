# Sitecore-Advanced-Caching
Cache clearing module for Sitecore - Currently tested against MVC only.

FEATURES:  
- Clears a component's html cache entry based on the item that was publish.   
- Fallback mechanism of clearing the site's cache, site's cache in all languages, and all site in all languages.  
- Cache entries are persisted to cache database to mitigate slow startup on app recyle.  
- New cache entries are published to all CD servers to minimize recomputation.  
- Works with both SQL and Azure Service Bus

STEPS:  
Step 1 - Run sql script located in Scripts folder. The ConcurrencyId columns have computed column with 12. Change this to the number of virtual CPU's assigned to the database or leave as is for Azure Database.  
Step 2 - Compile and deploy config and dll to both CM and CD  
Step 3 - Add HtmlCache ConnectionString  
Step 4 - Set the HtmlCacheQueueSubscriberPostfix config setting (unique per CD server).  
Step 5 - Run Unicorn.  
Step 6 - Under cache settings for each component, set cacheable to true and set the cacheable templates field to enable item tracking. These cacheable templates should be set in accordance to the datasources for that component.  
