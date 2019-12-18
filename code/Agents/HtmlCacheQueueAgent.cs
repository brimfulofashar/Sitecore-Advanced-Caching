using System;
using System.Linq;
using Foundation.HtmlCache.DB;
using Foundation.HtmlCache.Models;

namespace Foundation.HtmlCache.Agents
{
    public class HtmlCacheQueueAgent
    {
        public void ProcessCacheQueue()
        {
            
        }

        public string sql = @"
            DECLARE @CacheQueueId BIGINT
DECLARE @CacheQueueMessageType_Id INT
DECLARE @CacheQueueUpdateVersion ROWVERSION
DECLARE @CacheQueueBlockerUpdateVersion ROWVERSION
DECLARE @BlockingMode BIT
DECLARE @BlockTokenTaken BIT
DECLARE @PendingQueueLength BIGINT
DECLARE @SiteName VARCHAR(250)
DECLARE @SiteLang VARCHAR(250)
DECLARE @HtmlCacheKeys NVARCHAR(250)

BEGIN TRY
	BEGIN TRANSACTION;

		SELECT @PendingQueueLength = COUNT(Id) FROM CacheQueue;

		SELECT @BlockingMode = BlockingMode FROM [CacheQueueBlocker] WHERE Id = 1

		IF (@BlockingMode = 0)
		BEGIN
			WITH CTE (Id)
			AS
			(
				SELECT TOP 1 cq.Id FROM CacheQueue cq WHERE Processing = 0 GROUP BY cq.Id, cq.CacheQueueMessageType_Id ORDER BY cq.CacheQueueMessageType_Id DESC, cq.Id ASC
			)
			SELECT @CacheQueueId = cq.Id, @CacheQueueMessageType_Id = cq.CacheQueueMessageType_Id, @CacheQueueUpdateVersion = cq.UpdateVersion
			FROM CTE
			INNER JOIN CacheQueue cq
			ON cq.Id = CTE.Id
						
			SELECT @CacheQueueBlockerUpdateVersion = UpdateVersion
			FROM CacheQueueBlocker
	
			SELECT @CacheQueueUpdateVersion, @CacheQueueBlockerUpdateVersion, @CacheQueueMessageType_Id	

		END
		UPDATE CacheQueue SET Processing = 1 WHERE Id = @CacheQueueId AND UpdateVersion = @CacheQueueUpdateVersion
		DECLARE @Updated bit
		IF (@@ROWCOUNT > 0)
		BEGIN
			SET @Updated = 1
		END
		ELSE
		BEGIN
			SET @Updated = 0
		END
		SELECT @Updated as Updated
		IF @Updated = 1
		BEGIN		            
			IF (@CacheQueueMessageType_Id <> 1)
			BEGIN
				UPDATE [CacheQueueBlocker]
				SET BlockingMode = 1
				WHERE Id = 1
				AND UpdateVersion = @CacheQueueBlockerUpdateVersion
				IF @@ROWCOUNT > 0
				BEGIN
					SELECT 'BlockTokenTaken'
					SET @BlockTokenTaken = 1
				END
				ELSE
				BEGIN
					SELECT 'BlockTokenNOTTaken';
					SET @BlockTokenTaken = 0;
					THROW 5000,'Block Token Not Taken',1;
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
				using (SELECT ckt.Id, csl.Id as cslId, cslt.Id as csltId, ckt.HtmlCacheKey, ckt.HtmlCacheResult 
				FROM CacheSiteLangTemp cslt
				INNER JOIN CacheKeysTemp ckt
				ON cslt.Id = ckt.CacheSiteLang_Id
				LEFT JOIN CacheSiteLang csl
				ON cslt.Name = csl.Name
				AND cslt.Lang = csl.Lang
				WHERE cslt.CacheQueue_Id = @CacheQueueId) as S 
				on (T.HtmlCacheKey = S.HtmlCacheKey AND T.CacheSiteLang_Id = S.cslId)
				when matched
				then update set T.HtmlCacheKey = S.HtmlCacheKey, T.HtmlCacheResult = S.HtmlCacheResult, T.CacheSiteLang_Id = S.cslId
				when not matched
				then insert (Id, HtmlCacheKey, HtmlCacheResult, CacheSiteLang_Id) values (S.Id, S.HtmlCacheKey, S.HtmlCacheResult, S.csltId);				
                        
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

				UPDATE CacheQueueBlocker SET BlockingMode = 0 WHERE ID = 1 AND UpdateVersion = @CacheQueueBlockerUpdateVersion

				SELECT @HtmlCacheKeys = COALESCE(@HtmlCacheKeys + '|', '') + HtmlCacheKey FROM (SELECT DISTINCT(HtmlCacheKey) FROM @DeletedCacheKeys) as cachekeys
			END
			SELECT @BlockTokenTaken
			IF(@BlockTokenTaken = 1 AND @CacheQueueMessageType_Id = 3)
			BEGIN
			            
				SELECT @SiteName = Name, @SiteLang = Lang 
				FROM [CacheSiteLangTemp]
				WHERE CacheQueue_Id = @CacheQueueId

				SELECT @SiteName, @SiteLang;

				WITH CTEDeleteSite(CacheQueueId)
				AS
				(
					SELECT cq.Id FROM CacheQueue cq
					INNER JOIN CacheSiteLangTemp cslt
					ON cq.Id = cslt.CacheQueue_Id
					WHERE cslt.Name = @SiteName AND cslt.Lang = @SiteLang
				)
				DELETE FROM CacheQueue WHERE Id IN (SELECT CacheQueueId FROM CTEDeleteSite)

				DELETE csl FROM CacheSiteLang csl WHERE csl.Name = @SiteName AND csl.Lang = @SiteLang
					
				UPDATE CacheQueueBlocker SET BlockingMode = 0 WHERE ID = 1 AND UpdateVersion = @CacheQueueBlockerUpdateVersion				
			END				
		END
		SELECT @PendingQueueLength as PendingQueueLength, @SiteName as SiteName, @SiteLang as SiteLang, @HtmlCacheKeys as DeletedCacheKeys
		
	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	SELECT   
        ERROR_NUMBER() AS ErrorNumber  
       ,ERROR_MESSAGE() AS ErrorMessage;   
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
END CATCH";
    }


}