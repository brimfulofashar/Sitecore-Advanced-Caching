namespace Foundation.HtmlCache.Queries
{
    public class DeleteFromCache
    {
        public static string GetQuery()
        {
        return @"
        SET NOCOUNT ON;

	    DECLARE @Counter BIGINT
	    SELECT @Counter = MIN(ID) FROM CacheQueue
	    DECLARE @Suffix char(32)
	    DECLARE @DeleteFromCacheStatement as NVARCHAR(max)
	    DECLARE @MaxId BIGINT
	    SELECT @MaxId = MAX(ID) FROM CacheQueue
	    DECLARE @MessageId BIGINT
	    SELECT @MessageId = MIN(ID) FROM CacheQueue WHERE CacheQueueMessageType_Id = 2

	    SET @DeleteFromCacheStatement = '
		    DELETE FROM CacheKeys
		    WHERE HtmlCacheKey IN
		    (
		    SELECT ck.HtmlCacheKey
		    FROM CacheKeys ck          
		    INNER JOIN CacheKeysItems cki 
		    ON ck.Id = cki.CacheKey_Id 
		    INNER JOIN CacheItems ci
		    ON ci.Id = cki.CacheItem_Id
		    INNER JOIN PublishedItems pi 
		    ON ci.ItemId = pi.ItemId
		    INNER JOIN CacheQueue cq
		    ON pi.CacheQueueId = ' + CAST(@MessageId AS varchar(12)) +')'

        WHILE @Counter <= @MaxId
        BEGIN
		    SELECT @Suffix = Suffix FROM CacheQueue with (rowlock updlock) WHERE Id = @Counter AND CacheQueueMessageType_Id = 1
      
  		    SET @DeleteFromCacheStatement =  CONCAT(@DeleteFromCacheStatement, '            
            DELETE FROM CacheKeys_' + @Suffix + '
		    WHERE HtmlCacheKey IN
		    (
		    SELECT ckTemp.HtmlCacheKey
		    FROM CacheKeys_' + @Suffix + ' ckTemp          
		    INNER JOIN CacheKeysItems_' + @Suffix + ' ckiTemp 
		    ON ckTemp.Id = ckiTemp.CacheKey_Id 
		    INNER JOIN CacheItems_' + @Suffix + ' ciTemp
		    ON ciTemp.Id = ckiTemp.CacheItem_Id
		    INNER JOIN PublishedItems pi 
		    ON ciTemp.ItemId = pi.ItemId
		    INNER JOIN CacheQueue cq
		    ON pi.CacheQueueId = ' + CAST(@MessageId AS varchar(12)) +')')

		    SET @Counter = @Counter + 1
	    END

	    SET @DeleteFromCacheStatement = CONCAT(@DeleteFromCacheStatement, '
	    DELETE FROM PublishedItems WHERE CacheQueueId = ' + CAST(@MessageId AS varchar(12)) +'')

	    SET @DeleteFromCacheStatement = CONCAT(@DeleteFromCacheStatement, '
	    DELETE FROM CacheQueue WHERE Id = ' + CAST(@MessageId AS varchar(12)) +'')";
        }

        public static string GetDeleteTempTableQuery(string suffix)
        {
            return string.Format(@"
            DROP TABLE CacheKeysItems_{0};
            DROP TABLE CacheItems_{0};
            DROP TABLE CacheKeys_{0};", suffix);
        }
    }
}