using System.Linq;
using Foundation.HtmlCache.DB;
using Foundation.HtmlCache.Models;

namespace Foundation.HtmlCache.Agents
{
    public class HtmlCacheQueueAgent
    {
        public void ProcessCacheQueue()
        {
            using (var ctx = new ItemTrackingProvider())
            {
                bool process = true;
                while (process)
                {
                    ctx.Database.BeginTransaction();
                    var result = ctx.ProcessQueue().First();
                    if (result.PendingQueueLength <= 0)
                    {
                        process = false;
                    }

                    if (!string.IsNullOrEmpty(result.DeletedCacheKeys))
                    {
                        // notify cd's to clear the htmlcachekey
                    }
                    else
                    {
                        // notify cd's to clear site cache
                    }
                    ctx.Database.CurrentTransaction.Commit();
                }
            }
        }

        public string sql = @"
            DECLARE @CacheQueueId BIGINT
            DECLARE @CacheQueueMessageType_Id INT
            DECLARE @CacheQueueUpdateVersion ROWVERSION
            DECLARE @CacheQueueBlockerUpdateVersion ROWVERSION
            DECLARE @BlockingMode BIT
            DECLARE @BlockTokenTaken BIT
            DECLARE @PendingQueueLength BIGINT
            DECLARE @SiteName NVARCHAR(250)
            DECLARE @SiteLang NVARCHAR(250)
            DECLARE @HtmlCacheKeys NVARCHAR(250)

            SELECT @PendingQueueLength = COUNT(Id) FROM CacheQueue;

            SELECT TOP 1 @BlockingMode = BlockingMode FROM [CacheQueueBlocker]

            IF (@BlockingMode = 0)
            BEGIN
	            WITH CTE (Id)
	            AS
	            (
		            SELECT MIN(Id) FROM CacheQueue WHERE Processing = 0
	            )
	            SELECT @CacheQueueId = cq.Id, @CacheQueueMessageType_Id = cq.CacheQueueMessageType_Id, @CacheQueueUpdateVersion = cq.UpdateVersion, @CacheQueueBlockerUpdateVersion = cqb.UpdateVersion
	            FROM CTE
	            INNER JOIN CacheQueue cq
	            ON cq.Id = CTE.Id
	            LEFT JOIN CacheQueueBlocker cqb
	            ON cq.Id = cqb.CacheQueue_Id
	            ORDER BY cq.CacheQueueMessageType_Id DESC

	            UPDATE CacheQueue SET Processing = 1 WHERE Id = @CacheQueueId AND UpdateVersion = @CacheQueueUpdateVersion
	            IF @@ROWCOUNT > 0
	            BEGIN
		            
		            IF (@CacheQueueMessageType_Id <> 1 AND @CacheQueueBlockerUpdateVersion IS NOT NULL)
		            BEGIN
			            IF (@PendingQueueLength = 0)
			            BEGIN
				            UPDATE [CacheQueueBlocker]
				            SET BlockingMode = 1, CacheQueue_Id = @CacheQueueId
				            WHERE BlockingMode = 0 
				            AND CacheQueue_Id IS NULL
				            AND UpdateVersion = @CacheQueueBlockerUpdateVersion
				            IF @@ROWCOUNT > 0
				            BEGIN
					            SET @BlockTokenTaken = 1
				            END
			            END
			            ELSE
			            BEGIN
				            SET @BlockTokenTaken = 0
			            END
		            END

		            IF ((@BlockingMode = 0 AND @CacheQueueMessageType_Id = 1))
		            BEGIN
			            merge into CacheSiteLang WITH (HOLDLOCK) as T 
			            using (SELECT Id, Name, Lang FROM CacheSiteLangTemp WHERE CacheQueue_Id = @CacheQueueId) as S 
			            on (T.Name = S.Name AND T.Lang = S.Lang) 
			            when matched
			            then update set T.Name = S.Name, T.Lang = S.Lang
			            when not matched
			            then insert (Id, Name, Lang) values (S.Id, S.Name, S.Lang);
			            
			            merge into CacheKeys WITH (HOLDLOCK) as T 
			            using (SELECT Id, CacheSiteLang_Id, HtmlCacheKey, HtmlCacheResult FROM CacheKeysTemp WHERE CacheQueue_Id = @CacheQueueId) as S 
			            on (T.HtmlCacheKey = S.HtmlCacheKey AND T.CacheSiteLang_Id = S.CacheSiteLang_Id) 
			            when matched
			            then update set T.HtmlCacheKey = S.HtmlCacheKey, T.HtmlCacheResult = S.HtmlCacheResult, T.CacheSiteLang_Id = S.CacheSiteLang_Id
			            when not matched
			            then insert (Id, HtmlCacheKey, HtmlCacheResult, CacheSiteLang_Id) values (S.Id, S.HtmlCacheKey, S.HtmlCacheResult, S.CacheSiteLang_Id);
                        
			            merge into CacheItems WITH (HOLDLOCK) as T 
			            using (SELECT Id, ItemId, CacheKey_Id FROM CacheItemsTemp WHERE CacheQueue_Id = @CacheQueueId) as S 
			            on (T.ItemId = S.ItemId) 
			            when matched
			            then update set T.ItemId = S.ItemId, T.CacheKey_Id = S.CacheKey_Id
			            when not matched
			            then insert (Id, ItemId, CacheKey_Id) values (S.Id, S.ItemId, S.CacheKey_Id);
                        
			            merge into CacheKeysItems WITH (HOLDLOCK) as T 
			            using (SELECT ckiTemp.Id, ck.Id as CacheKey_Id, ci.Id as CacheItem_Id
			            FROM CacheKeys ck
			            INNER JOIN CacheKeysTemp ckTemp on ck.HtmlCacheKey = ckTemp.HtmlCacheKey
			            INNER JOIN CacheKeysItemsTemp ckiTemp on ckTemp.Id = ckiTemp.CacheKey_Id
			            INNER JOIN CacheItemsTemp ciTemp on ciTemp.Id = ckiTemp.CacheItem_Id
			            INNER JOIN CacheItems ci on ciTemp.ItemId = ci.ItemId WHERE ckiTemp.CacheQueue_Id = @CacheQueueId) as S 
			            on (T.CacheKey_Id = S.CacheKey_Id AND T.CacheItem_Id = S.CacheItem_Id) 
			            when matched
			            then update set T.CacheItem_Id = S.CacheItem_Id, T.CacheKey_Id = S.CacheKey_Id
			            when not matched
			            then insert (Id, CacheKey_Id, CacheItem_Id) values (S.Id, S.CacheKey_Id, S.CacheItem_Id);

			            DELETE FROM CacheQueue WHERE Id = @CacheQueueId;

			            IF (@BlockTokenTaken = 1)
			            BEGIN
				            UPDATE [CacheQueueBlocker]
				            SET BlockingMode = 0, CacheQueue_Id = NULL
				            WHERE BlockingMode = 1 AND CacheQueue_Id IS NOT NULL
			            END			
		            END
		            IF(@BlockTokenTaken = 1 AND @CacheQueueMessageType_Id = 2)
		            BEGIN
			            DECLARE @DeletedCacheKeys table (HtmlCacheKey nvarchar(3000));

			            SELECT @SiteName = Name, @SiteLang = Lang 
			            FROM [CacheSiteLangTemp]
			            WHERE CacheQueue_Id = @CacheQueueId;

			            WITH CTECacheTemp (HtmlCacheKey)
			            AS
			            (
				            SELECT ckt.HtmlCacheKey FROM PublishedItems pi				
				            INNER JOIN CacheItemsTemp cit
				            ON pi.ItemId = cit.ItemId
				            INNER JOIN CacheKeysItemsTemp ckit
				            ON cit.ItemId = ckit.CacheItem_Id
				            INNER JOIN CacheKeysTemp ckt
				            ON ckit.CacheKey_Id = ckt.Id
				            WHERE pi.CacheQueue_Id = @CacheQueueId
			            )
			            DELETE FROM CacheKeysTemp OUTPUT deleted.HtmlCacheKey INTO @DeletedCacheKeys WHERE HtmlCacheKey IN (SELECT HtmlCacheKey FROM CTECacheTemp);

			            WITH CTECache (HtmlCacheKey)
			            AS
			            (
				            SELECT ck.HtmlCacheKey FROM PublishedItems pi
				            INNER JOIN CacheItems ci
				            ON pi.ItemId = ci.ItemId
				            INNER JOIN CacheKeysItems cki
				            ON ci.ItemId = cki.CacheItem_Id
				            INNER JOIN CacheKeys ck
				            ON cki.CacheKey_Id = ck.Id
				            WHERE pi.CacheQueue_Id = @CacheQueueId			
			            )
			            DELETE FROM CacheKeys OUTPUT deleted.HtmlCacheKey INTO @DeletedCacheKeys WHERE HtmlCacheKey IN (SELECT HtmlCacheKey FROM CTECache);

			            DELETE FROM CacheQueue WHERE Id = @CacheQueueId;

			            SELECT @HtmlCacheKeys = COALESCE(@HtmlCacheKeys + '|', '') + HtmlCacheKey FROM (SELECT DISTINCT(HtmlCacheKey) FROM @DeletedCacheKeys) as cachekeys

			            
			            
		            END
		            IF(@BlockTokenTaken = 1 AND @CacheQueueMessageType_Id = 3)
		            BEGIN
			            
			            SELECT @SiteName = Name, @SiteLang = Lang 
			            FROM [CacheSiteLangTemp]
			            WHERE CacheQueue_Id = @CacheQueueId

			            DELETE csl
			            FROM CacheSiteLang csl
			            INNER JOIN CacheSiteLangTemp cslt
			            ON csl.Name = cslt.Name AND csl.Lang = cslt.Lang
			            WHERE cslt.CacheQueue_Id = @CacheQueueId

			            DELETE FROM CacheQueue WHERE Id = @CacheQueueId;
		            END
	            END
            END
            SELECT @PendingQueueLength as PendingQueueLength, @SiteName, @SiteLang, @HtmlCacheKeys as DeletedCacheKeys";
    }


}