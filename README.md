# Sitecore-Advanced-Caching
Cache clearing module for Sitecore

This module clears a component's html cache entry based on the item which was publish. Fallback mechanism of clearing the site's cache, site's cache in all languages and clearing all caches for all sites is also provided. Cache entries are also persisted to a database to mitigate slow startup on app recyle.  

Step 1 - Run sql script located in Scripts folder  
Step 2 - Compile and deploy config and dll to both CM and CD  
Step 3 - Add HtmlCache ConnectionString  
