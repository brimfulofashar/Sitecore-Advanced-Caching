# Sitecore-Advanced-Caching
Cache clearing module for Sitecore

FEATURES:  
- Clears a component's html cache entry based on the item that was publish.   
- Fallback mechanism of clearing the site's cache, site's cache in all languages, all site in all languages.  
- Cache entries are persisted to cache database to mitigate slow startup on app recyle.  
- New cache entries are published to all CD servers to minimize recomputation.  

STEPS:  
Step 1 - Run sql script located in Scripts folder  
Step 2 - Compile and deploy config and dll to both CM and CD  
Step 3 - Add HtmlCache ConnectionString  
Step 4 - Set the HtmlCacheQueueSubscriberPostfix config setting per CD server.  
Step 5 - Run Unicorn.  
Step 6 - Under cache settings for component, set the cacheable templates field to enable item tracking.  
