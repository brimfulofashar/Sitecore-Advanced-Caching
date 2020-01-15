USE [master]
GO
CREATE DATABASE [HtmlCache]
 CONTAINMENT = PARTIAL
 ON  PRIMARY 
( NAME = N'habitat.dev_Cache', FILENAME = N'D:\MSSQL13.MSSQLSERVER\MSSQL\DATA\HtmlCache.mdf' , SIZE = 65536KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'habitat.dev_Cache_log', FILENAME = N'D:\MSSQL13.MSSQLSERVER\MSSQL\Logs\HtmlCache_log.ldf' , SIZE = 65536KB , MAXSIZE = UNLIMITED , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [HtmlCache] SET COMPATIBILITY_LEVEL = 130
GO
ALTER DATABASE [HtmlCache] SET RECOVERY SIMPLE 
GO
USE [HtmlCache]
GO
CREATE USER [htmlcacheuser] WITH PASSWORD=N'vvieyJdmbEdULTRsG9GD7P1sutYMXGtqnFgaM6apPc8=', DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [htmlcacheuser]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TYPE [ItemMetaData] AS TABLE(
	[Id] [uniqueidentifier] NOT NULL,
	[Lang] [varchar](250) NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheHtml](
	[Id] [uniqueidentifier] NOT NULL,
	[CacheSiteId] [uniqueidentifier] NULL,
	[HtmlCacheKey] [varchar](5000) NOT NULL,
	[HtmlCacheKeyHash]  AS (hashbytes('SHA2_512',[HtmlCacheKey])) PERSISTED,
	[HtmlCacheResult] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Cache] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheHtml_CacheItem](
	[Id] [uniqueidentifier] NOT NULL,
	[CacheHtmlId] [uniqueidentifier] NOT NULL,
	[CacheItemId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CacheHtml_CacheItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheHtmlTemp](
	[Id] [uniqueidentifier] NOT NULL,
	[CacheQueueId] [bigint] NOT NULL,
	[HtmlCacheKey] [varchar](5000) NOT NULL,
	[HtmlCacheKeyHash]  AS (hashbytes('SHA2_512',[HtmlCacheKey])) PERSISTED,
	[HtmlCacheResult] [varchar](max) NOT NULL,
 CONSTRAINT [PK_CacheHtmlTemp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheItem](
	[Id] [uniqueidentifier] NOT NULL,
	[ItemId] [uniqueidentifier] NOT NULL,
	[ItemLang] [varchar](250) NOT NULL,
 CONSTRAINT [PK_CacheItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheItemTemp](
	[Id] [uniqueidentifier] NOT NULL,
	[CacheQueueId] [bigint] NOT NULL,
	[ItemId] [uniqueidentifier] NOT NULL,
	[ItemLang] [varchar](250) NOT NULL,
 CONSTRAINT [PK_CacheItemTemp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheQueue](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CacheQueueMessageTypeId] [int] NOT NULL,
	[Processing] [bit] NOT NULL,
	[ProcessingBy] [varchar](250) NULL,
	[UpdateVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_CacheQueue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheQueueMessageType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MessageType] [varchar](100) NOT NULL,
 CONSTRAINT [PK_CacheQueueMessageType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheSite](
	[Id] [uniqueidentifier] NOT NULL,
	[SiteName] [varchar](250) NOT NULL,
	[SiteLang] [varchar](250) NOT NULL,
 CONSTRAINT [PK_CacheSite] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheSiteTemp](
	[Id] [uniqueidentifier] NOT NULL,
	[CacheQueueId] [bigint] NOT NULL,
	[SiteName] [varchar](250) NULL,
	[SiteLang] [varchar](250) NULL,
 CONSTRAINT [PK_CacheSiteTemp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [CacheQueueMessageType] ON 
GO
INSERT [CacheQueueMessageType] ([Id], [MessageType]) VALUES (1, N'AddToCache')
GO
INSERT [CacheQueueMessageType] ([Id], [MessageType]) VALUES (2, N'DeleteHtmlFromCache')
GO
INSERT [CacheQueueMessageType] ([Id], [MessageType]) VALUES (3, N'DeleteSiteFromCache')
GO
SET IDENTITY_INSERT [CacheQueueMessageType] OFF
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF
GO
CREATE NONCLUSTERED INDEX [IX_Cache_HtmlCacheKeyHash] ON [CacheHtml]
(
	[HtmlCacheKeyHash] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheHtml_CacheSiteId] ON [CacheHtml]
(
	[CacheSiteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheHtml_CacheItem_CacheHtmlId] ON [CacheHtml_CacheItem]
(
	[CacheHtmlId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheHtml_CacheItem_CacheItemId] ON [CacheHtml_CacheItem]
(
	[CacheItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheHtmlTemp_CacheQueueId] ON [CacheHtmlTemp]
(
	[CacheQueueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF
GO
CREATE NONCLUSTERED INDEX [IX_CacheHtmlTemp_HtmlCacheKeyHash] ON [CacheHtmlTemp]
(
	[HtmlCacheKeyHash] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheItem_ItemId] ON [CacheItem]
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CacheItem_ItemLang] ON [CacheItem]
(
	[ItemLang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheItemTemp_CacheQueueId] ON [CacheItemTemp]
(
	[CacheQueueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheItemTemp_ItemId] ON [CacheItemTemp]
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CacheItemTemp_ItemLang] ON [CacheItemTemp]
(
	[ItemLang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheQueue_CacheQueueMessageTypeId] ON [CacheQueue]
(
	[CacheQueueMessageTypeId] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheQueue_Processing] ON [CacheQueue]
(
	[Processing] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheQueue_UpdateVersion] ON [CacheQueue]
(
	[UpdateVersion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CacheSite_SiteLang] ON [CacheSite]
(
	[SiteLang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CacheSite_SiteName] ON [CacheSite]
(
	[SiteName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheSiteTemp_CacheQueueId] ON [CacheSiteTemp]
(
	[CacheQueueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CacheSiteTemp_SiteLang] ON [CacheSiteTemp]
(
	[SiteLang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CacheSiteTemp_SiteName] ON [CacheSiteTemp]
(
	[SiteName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
ALTER TABLE [CacheHtml] ADD  CONSTRAINT [DF_Cache_Id]  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [CacheHtml_CacheItem] ADD  CONSTRAINT [DF_Cache_CacheItem_Id]  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [CacheHtmlTemp] ADD  CONSTRAINT [DF_CacheTemp_Id]  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [CacheItem] ADD  CONSTRAINT [DF_CacheItem_Id]  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [CacheItemTemp] ADD  CONSTRAINT [DF_CacheItemTemp_Id]  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [CacheSite] ADD  CONSTRAINT [DF_CacheSite_Id]  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [CacheSiteTemp] ADD  CONSTRAINT [DF_CacheSiteTemp_Id]  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [CacheHtml]  WITH CHECK ADD  CONSTRAINT [FK_CacheHtml_CacheSite] FOREIGN KEY([CacheSiteId])
REFERENCES [CacheSite] ([Id])
GO
ALTER TABLE [CacheHtml] CHECK CONSTRAINT [FK_CacheHtml_CacheSite]
GO
ALTER TABLE [CacheHtml_CacheItem]  WITH CHECK ADD  CONSTRAINT [FK_CacheHtml_CacheItem_CacheHtml] FOREIGN KEY([CacheHtmlId])
REFERENCES [CacheHtml] ([Id])
GO
ALTER TABLE [CacheHtml_CacheItem] CHECK CONSTRAINT [FK_CacheHtml_CacheItem_CacheHtml]
GO
ALTER TABLE [CacheHtml_CacheItem]  WITH CHECK ADD  CONSTRAINT [FK_CacheHtml_CacheItem_CacheItem] FOREIGN KEY([CacheItemId])
REFERENCES [CacheItem] ([Id])
GO
ALTER TABLE [CacheHtml_CacheItem] CHECK CONSTRAINT [FK_CacheHtml_CacheItem_CacheItem]
GO
ALTER TABLE [CacheHtmlTemp]  WITH CHECK ADD  CONSTRAINT [FK_CacheHtmlTemp_CacheQueue] FOREIGN KEY([CacheQueueId])
REFERENCES [CacheQueue] ([Id])
GO
ALTER TABLE [CacheHtmlTemp] CHECK CONSTRAINT [FK_CacheHtmlTemp_CacheQueue]
GO
ALTER TABLE [CacheItemTemp]  WITH CHECK ADD  CONSTRAINT [FK_CacheItemTemp_CacheQueue] FOREIGN KEY([CacheQueueId])
REFERENCES [CacheQueue] ([Id])
GO
ALTER TABLE [CacheItemTemp] CHECK CONSTRAINT [FK_CacheItemTemp_CacheQueue]
GO
ALTER TABLE [CacheQueue]  WITH CHECK ADD  CONSTRAINT [FK_CacheQueue_CacheQueueMessageType] FOREIGN KEY([CacheQueueMessageTypeId])
REFERENCES [CacheQueueMessageType] ([Id])
GO
ALTER TABLE [CacheQueue] CHECK CONSTRAINT [FK_CacheQueue_CacheQueueMessageType]
GO
ALTER TABLE [CacheSiteTemp]  WITH CHECK ADD  CONSTRAINT [FK_CacheSiteTemp_CacheQueue] FOREIGN KEY([CacheQueueId])
REFERENCES [CacheQueue] ([Id])
GO
ALTER TABLE [CacheSiteTemp] CHECK CONSTRAINT [FK_CacheSiteTemp_CacheQueue]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [usp_LockAndProcessCacheQueueEntry] 
	@ProcessingBy Varchar(250),
	@CacheQueueCount BIGINT OUTPUT
AS
BEGIN
	BEGIN TRANSACTION

	BEGIN TRY

	DECLARE @CacheQueueId bigint
	DECLARE @CacheQueueMessageTypeId int
	DECLARE @Processing bit
	DECLARE @UpdateVersion timestamp

	DECLARE @BlockFurtherProcessing bit

	SET @BlockFurtherProcessing = (SELECT COUNT(Id) FROM CacheQueue Where @CacheQueueMessageTypeId > 1 AND Processing = 1);

	CREATE TABLE #CacheNameLangKey (Id BIGINT IDENTITY(1, 1) primary key ,CacheQueueMessageTypeId int, SiteName varchar(250), SiteLang varchar(250), HtmlCacheKey varchar(5000))

	IF @BlockFurtherProcessing = 0
	BEGIN
		SELECT TOP 1 @CacheQueueId = Id, 
		@CacheQueueMessageTypeId = CacheQueueMessageTypeId,
		@Processing = Processing,
		@UpdateVersion = UpdateVersion
		FROM CacheQueue
		WHERE Processing = 0
		ORDER BY CacheQueueMessageTypeId DESC, Id ASC

		IF @Processing = 0
		BEGIN
			UPDATE CacheQueue
			SET Processing = 1,
			ProcessingBy = @ProcessingBy
			WHERE Id = @CacheQueueId
			AND UpdateVersion = @UpdateVersion
			
			IF @@ROWCOUNT > 0
			BEGIN
				IF @CacheQueueId IS NOT NULL AND @CacheQueueMessageTypeId = 1
				BEGIN
					EXEC usp_ProcessCacheData @CacheQueueId
					INSERT INTO #CacheNameLangKey(CacheQueueMessageTypeId, SiteName, SiteLang, HtmlCacheKey)
					SELECT @CacheQueueMessageTypeId AS CacheQueueMessageTypeId, NULL AS SiteName, NULL AS SiteLang, NULL AS HtmlCacheKey
				END
				ELSE IF @CacheQueueId IS NOT NULL AND @CacheQueueMessageTypeId = 2
				BEGIN
					INSERT INTO #CacheNameLangKey(CacheQueueMessageTypeId, SiteName, SiteLang, HtmlCacheKey)
					EXEC usp_ProcessDeleteHtmlFromCache @CacheQueueId, @CacheQueueMessageTypeId
				END
				ELSE IF @CacheQueueId IS NOT NULL AND @CacheQueueMessageTypeId = 3
				BEGIN
					INSERT INTO #CacheNameLangKey(CacheQueueMessageTypeId, SiteName, SiteLang, HtmlCacheKey)
					EXEC usp_ProcessDeleteSiteFromCache @CacheQueueId, @CacheQueueMessageTypeId
				END

				DELETE From CacheSiteTemp WHERE CacheQueueId = @CacheQueueId
				DELETE From CacheHtmlTemp WHERE CacheQueueId = @CacheQueueId
				DELETE From CacheItemTemp WHERE CacheQueueId = @CacheQueueId
				DELETE FROM CacheQueue WHERE Id = @CacheQueueId	
			
				SELECT @CacheQueueCount = COUNT(Id) FROM CacheQueue	
			END	
		END
		ELSE
		BEGIN
			SELECT @CacheQueueCount =  0
		END
	END

	SELECT CacheQueueMessageTypeId, SiteName, SiteLang, HtmlCacheKey
	FROM #CacheNameLangKey

	COMMIT TRANSACTION 
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION

			DECLARE @ErrorMessage NVARCHAR(4000);  
			DECLARE @ErrorSeverity INT;  
			DECLARE @ErrorState INT;  
  
			SELECT
				@ErrorMessage = ERROR_MESSAGE(),  
				@ErrorSeverity = ERROR_SEVERITY(),  
				@ErrorState = ERROR_STATE();  
  
			RAISERROR (@ErrorMessage,  @ErrorSeverity, @ErrorState);  

        END
    END CATCH
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [usp_ProcessCacheData] 
	@CacheQueueId		BIGINT
AS
	
DECLARE @CacheSiteIdTbl TABLE
(
   Id uniqueidentifier
);

WITH CacheSiteCTE (SiteName, SiteLang)
AS
(
	SELECT cst.SiteName, cst.SiteLang
	FROM CacheSiteTemp cst
	WHERE cst.CacheQueueId = @CacheQueueId
)
MERGE [CacheSite] WITH (HOLDLOCK) AS t 
USING 
(
	SELECT 	SiteName, SiteLang
	FROM	CacheSiteCTE
) AS s 
ON 
(
	s.SiteName = t.SiteName AND s.SiteLang = t.SiteLang 
) 
WHEN MATCHED
	THEN UPDATE SET
	t.SiteName = s.SiteName,
	t.SiteLang = s.SiteLang
WHEN NOT MATCHED 
	THEN INSERT ([SiteName], [SiteLang]) 
    VALUES (s.SiteName, s.SiteLang)
OUTPUT inserted.Id INTO @CacheSiteIdTbl;

DECLARE @CacheSiteId uniqueidentifier
SET @CacheSiteId = (SELECT TOP(1) Id FROM @CacheSiteIdTbl)

DECLARE @CacheHtmlIdTbl TABLE
(
   Id uniqueidentifier
);

WITH CacheHtmlTblCTE(HtmlCacheKey, HtmlCacheKeyHash, HtmlCacheResult)
AS
(
	SELECT cht.HtmlCacheKey, cht.HtmlCacheKeyHash, cht.HtmlCacheResult
	FROM CacheHtmlTemp cht
	WHERE cht.CacheQueueId = @CacheQueueId
)
MERGE [CacheHtml] WITH (HOLDLOCK) AS t 
USING 
(
	SELECT 	HtmlCacheKey, HtmlCacheKeyHash, HtmlCacheResult
	FROM	CacheHtmlTblCTE
) AS s 
ON 
(
	s.HtmlCacheKeyHash = t.HtmlCacheKeyHash
) 
WHEN MATCHED
	THEN UPDATE SET
	t.CacheSiteId = @CacheSiteId,
	t.HtmlCacheKey = s.HtmlCacheKey,
	t.HtmlCacheResult = s.HtmlCacheResult
WHEN NOT MATCHED 
	THEN INSERT (CacheSiteId, HtmlCacheKey, HtmlCacheResult) 
    VALUES (@CacheSiteId, s.HtmlCacheKey, s.HtmlCacheResult)
OUTPUT inserted.Id INTO @CacheHtmlIdTbl;

DECLARE @CacheHtmlId uniqueidentifier
SET @CacheHtmlId = (SELECT TOP(1) Id FROM @CacheHtmlIdTbl)

DECLARE @CacheItemIdTbl TABLE
(
   Id uniqueidentifier
);

WITH CacheItemTblCTE(ItemId, ItemLang)
AS
(
	SELECT cit.ItemId, cit.ItemLang
	FROM CacheItemTemp cit
	WHERE cit.CacheQueueId = @CacheQueueId
)
MERGE [CacheItem] WITH (HOLDLOCK) AS t 
USING 
(
	SELECT 	ItemId, ItemLang
	FROM	CacheItemTblCTE
) AS s 
ON 
(
	s.ItemId = t.ItemId AND
	s.ItemLang = t.ItemLang
) 
WHEN MATCHED
	THEN UPDATE SET
	t.ItemId = s.ItemId,
	t.ItemLang = s.ItemLang
WHEN NOT MATCHED 
	THEN INSERT (ItemId, ItemLang) 
    VALUES (s.ItemId, s.ItemLang)
OUTPUT inserted.Id INTO @CacheItemIdTbl;

MERGE [CacheHtml_CacheItem] WITH (HOLDLOCK) AS t
USING
(
	SELECT chidtbl.Id as CacheHtmlId, ciidtbl.Id as CacheItemId
	FROM @CacheHtmlIdTbl chidtbl, @CacheItemIdTbl ciidtbl
) as s
ON
(
	s.CacheHtmlId = t.CacheHtmlId AND
	s.CacheItemId = t.CacheItemId
)
WHEN MATCHED
	THEN UPDATE SET
	t.CacheHtmlId = s.CacheHtmlId,
	t.CacheItemId = s.CacheItemId
WHEN NOT MATCHED 
	THEN INSERT (CacheHtmlId, CacheItemId) 
    VALUES (s.CacheHtmlId, s.CacheItemId);
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [usp_ProcessDeleteHtmlFromCache] 
	@CacheQueueId bigint,
	@CacheQueueMessageTypeId int
AS
BEGIN
	
	CREATE TABLE #CacheQueuesToClear (CacheQueueId bigint, SiteLang varchar(250), ItemId uniqueidentifier, ItemLang varchar(250))

	INSERT INTO #CacheQueuesToClear (CacheQueueId, SiteLang, ItemId, ItemLang)
	SELECT cq.Id, cst.SiteLang, cit.ItemId, cit.ItemLang
	FROM CacheItemTemp cit
	INNER JOIN CacheQueue cq ON cit.CacheQueueId = cq.Id
	INNER JOIN CacheSiteTemp cst ON cq.Id = cst.CacheQueueId
	WHERE cq.Id = @CacheQueueId


	CREATE TABLE #CacheEntriesToClear (SiteName varchar(250), SiteLang varchar(250), CacheHtmlId uniqueIdentifier, CacheHtmlCacheItemId uniqueIdentifier, HtmlCacheKey varchar(5000), CacheItemId uniqueIdentifier)

	INSERT INTO #CacheEntriesToClear (SiteName, SiteLang, CacheHtmlId, CacheHtmlCacheItemId, HtmlCacheKey, CacheItemId)
	SELECT cs.SiteName, cs.SiteLang, chci2.CacheHtmlId, chci3.Id, ch.HtmlCacheKey, chci3.CacheItemId
	FROM #CacheQueuesToClear cqc
	INNER JOIN CacheItem ci ON ci.ItemId = cqc.ItemId AND ci.ItemLang = cqc.ItemLang
	INNER JOIN CacheHtml_CacheItem chci1 ON ci.Id = chci1.CacheItemId
	INNER JOIN CacheHtml_CacheItem chci2 ON chci1.CacheHtmlId = chci2.CacheHtmlId
	INNER JOIN CacheHtml_CacheItem chci3 ON chci2.CacheItemId = chci3.CacheItemId
	INNER JOIN CacheHtml ch ON chci3.CacheHtmlId = ch.Id
	INNER JOIN CacheSite cs ON ch.CacheSiteId = cs.Id

	DELETE chci FROM CacheHtml_CacheItem chci 
	INNER JOIN #CacheEntriesToClear r on r.CacheHtmlCacheItemId = chci.Id

	DELETE ci FROM CacheItem ci 
	INNER JOIN #CacheEntriesToClear r on r.CacheItemId = ci.Id

	DELETE ch FROM CacheHtml ch
	INNER JOIN #CacheEntriesToClear r on r.CacheHtmlId = ch.Id

	DELETE cht FROM CacheHtmlTemp cht
	INNER JOIN #CacheQueuesToClear r on r.CacheQueueId = cht.CacheQueueId AND r.CacheQueueId <> @CacheQueueId

	DELETE cit FROM CacheItemTemp cit
	INNER JOIN #CacheQueuesToClear r on r.CacheQueueId = cit.CacheQueueId AND r.CacheQueueId <> @CacheQueueId

	DELETE cst FROM CacheSiteTemp cst
	INNER JOIN #CacheQueuesToClear r on r.CacheQueueId = cst.CacheQueueId AND r.CacheQueueId <> @CacheQueueId

	DELETE cq FROM CacheQueue cq
	INNER JOIN #CacheQueuesToClear r on r.CacheQueueId = cq.Id AND r.CacheQueueId <> @CacheQueueId

	SELECT DISTINCT @CacheQueueMessageTypeId AS CacheQueueMessageTypeId, SiteName, SiteLang, HtmlCacheKey FROM #CacheEntriesToClear										
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [usp_ProcessDeleteSiteFromCache]
	@CacheQueueId bigint,
	@CacheQueueMessageTypeId int
AS
BEGIN
	
	DECLARE @SiteName varchar(250)
	DECLARE @SiteLang varchar(250)

	SELECT @SiteName = SiteName, @SiteLang = SiteLang FROM CacheSiteTemp WHERE CacheQueueId = @CacheQueueId

	CREATE TABLE #CacheQueuesToClear (Id bigint)

	INSERT INTO #CacheQueuesToClear (Id)
	SELECT cq.Id
	FROM CacheQueue cq
	INNER JOIN CacheSiteTemp cst
	on cq.Id = cst.CacheQueueId
	WHERE cq.Id <> @CacheQueueId
	AND cst.SiteName = SiteName
	AND ((cst.SiteLang = @SiteLang) OR ((@SiteLang = '' OR @SiteLang IS NULL) AND cst.SiteLang = cst.SiteLang))

	DELETE	cht FROM CacheHtmlTemp cht
	JOIN	#CacheQueuesToClear r on r.Id = cht.CacheQueueId AND r.Id <> @CacheQueueId

	DELETE	cit FROM CacheItemTemp cit
	JOIN	#CacheQueuesToClear r on r.Id = cit.CacheQueueId AND r.Id <> @CacheQueueId

	DELETE	cst FROM CacheSiteTemp cst
	JOIN	#CacheQueuesToClear r on r.Id = cst.CacheQueueId AND r.Id <> @CacheQueueId

	DELETE	cq FROM CacheQueue cq
	JOIN	#CacheQueuesToClear r on r.Id = cq.Id AND r.Id <> @CacheQueueId
	
	CREATE TABLE #ResultToClear (CacheSiteId uniqueIdentifier, CacheHtmlId uniqueIdentifier, CacheHtmlCacheItemId uniqueIdentifier, CacheItemId uniqueIdentifier)

	INSERT INTO #ResultToClear (CacheSiteId, CacheHtmlId, CacheHtmlCacheItemId, CacheItemId)
	SELECT	cs.Id as CacheSiteId, 
			ch.Id as CacheHtmlId, 
			chci.Id as CacheHtmlCacheItemId, 
			ci.Id as CacheItemId
	FROM	CacheQueue cq
	INNER JOIN CacheSiteTemp cst	ON cq.Id = cst.CacheQueueId	
	INNER JOIN CacheSite cs	ON cst.SiteName = cs.SiteName	AND cst.SiteLang = cs.SiteLang
	INNER JOIN CacheHtml ch	ON cs.Id = ch.CacheSiteId	
	INNER JOIN CacheHtml_CacheItem chci	ON ch.Id = chci.CacheHtmlId
	INNER JOIN CacheItem ci	ON chci.CacheItemId = ci.Id
	WHERE cq.Id = @CacheQueueId

	DELETE	chci FROM CacheHtml_CacheItem chci
	JOIN	#ResultToClear r on r.CacheHtmlCacheItemId = chci.id

	DELETE	ch FROM CacheHtml ch
	JOIN	#ResultToClear r on r.CacheHtmlId = ch.Id

	DELETE	ci FROM CacheItem ci
	JOIN	#ResultToClear r on r.CacheItemId = ci.Id

	DELETE	cs FROM CacheSite cs
	JOIN	#ResultToClear r on r.CacheSiteId = cs.Id
		
	SELECT @CacheQueueMessageTypeId AS CacheQueueMessageTypeId, SiteName, SiteLang, '' AS HtmlCacheKey FROM CacheSiteTemp WHERE CacheQueueId = @CacheQueueId
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [usp_QueueCacheData]
	@SiteName varchar(250),
	@SiteLang varchar(250),
	@HtmlCacheKey varchar(5000),
	@HtmlCacheResult varchar(max),
	@Ids ItemMetaData READONLY
AS
BEGIN
	DECLARE @CacheQueueId BIGINT

	INSERT INTO CacheQueue(CacheQueueMessageTypeId, Processing)
	VALUES(1 ,0)

	SET @CacheQueueId = SCOPE_IDENTITY()

	INSERT INTO CacheSiteTemp(CacheQueueId, SiteName, SiteLang)
	VALUES(@CacheQueueId, @SiteName, @SiteLang)

	INSERT INTO CacheHtmlTemp (CacheQueueId, HtmlCacheKey, HtmlCacheResult)
	VALUES (@CacheQueueId, @HtmlCacheKey, @HtmlCacheResult)

	INSERT INTO CacheItemTemp(CacheQueueId, ItemId, ItemLang)
	SELECT @CacheQueueId, Id, Lang FROM @Ids
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [usp_QueueDeleteSiteFromCache]
	@SiteName varchar(250),
	@SiteLang varchar(250)
AS
BEGIN
	DECLARE @CacheQueueId BIGINT

	INSERT INTO CacheQueue(CacheQueueMessageTypeId, Processing)
	VALUES(3 ,0)

	SET @CacheQueueId = SCOPE_IDENTITY()

	INSERT INTO CacheSiteTemp(CacheQueueId, SiteName, SiteLang)
	VALUES(@CacheQueueId, @SiteName, @SiteLang)
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [usp_QueuePublishData]
	@SiteLang varchar(250),
	@Ids ItemMetaData READONLY
AS
BEGIN
	DECLARE @CacheQueueId BIGINT

	INSERT INTO CacheQueue(CacheQueueMessageTypeId, Processing)
	VALUES(2 ,0)

	SET @CacheQueueId = SCOPE_IDENTITY()

	INSERT INTO CacheItemTemp(CacheQueueId, ItemId, ItemLang)
	SELECT @CacheQueueId, Id, Lang FROM @Ids

	INSERT INTO CacheSiteTemp(CacheQueueId, SiteLang)
	SELECT @CacheQueueId, @SiteLang
END
GO
USE [master]
GO
ALTER DATABASE [HtmlCache] SET  READ_WRITE 
GO
