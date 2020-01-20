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
	[Lang] [varchar](250) NOT NULL,
	[IsDeleted] [bit] NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheHtml](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CacheSiteId] [bigint] NULL,
	[HtmlCacheKey] [varchar](5000) NOT NULL,
	[HtmlCacheResult] [varchar](max) NOT NULL,
	[HtmlCacheKeyHash] [binary](64) NOT NULL,
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
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CacheHtmlId] [bigint] NOT NULL,
	[CacheItemId] [bigint] NOT NULL,
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
	[HtmlCacheResult] [varchar](max) NOT NULL,
	[HtmlCacheKeyHash] [binary](64) NOT NULL,
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
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
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
	[IsDeleted] [bit] NOT NULL,
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
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
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
CREATE NONCLUSTERED INDEX [IX_CacheHtml_CacheSiteId] ON [CacheHtml]
(
	[CacheSiteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CacheHtml_HtmlCacheKeyHash] ON [CacheHtml]
(
	[HtmlCacheKeyHash] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CacheHtmlTemp_HtmlCacheKeyHash] ON [CacheHtmlTemp]
(
	[HtmlCacheKeyHash] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
ALTER TABLE [CacheHtmlTemp] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [CacheItemTemp] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [CacheSiteTemp] ADD  DEFAULT (newid()) FOR [Id]
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

CREATE PROCEDURE [PurgeDatabase]
AS
BEGIN
	DELETE FROM CacheHtml_CacheItem
	DBCC CHECKIDENT ('CacheHtml_CacheItem', RESEED, 1)
	DELETE FROM CacheHtml
	DBCC CHECKIDENT ('CacheHtml', RESEED, 1)
	DELETE FROM CacheItem
	DBCC CHECKIDENT ('CacheItem', RESEED, 1)
	DELETE FROM CacheSite
	DBCC CHECKIDENT ('CacheSite', RESEED, 1)
	DELETE FROM CacheSiteTemp
	DELETE FROM CacheHtmlTemp
	DELETE FROM CacheItemTemp
	DELETE FROM CacheQueue
	DBCC CHECKIDENT ('CacheQueue', RESEED, 1)
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [usp_GetCacheForSite]
	@SiteName varchar(250)
AS
BEGIN
	SELECT cs.SiteName, ch.HtmlCacheKey, ch.HtmlCacheResult
	FROM CacheSite cs
	INNER JOIN CacheHtml ch ON cs.Id = ch.CacheSiteId
	WHERE cs.SiteName = @SiteName
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [usp_GetStats]
AS
BEGIN
	SELECT 
    t.NAME AS TableName,
    s.Name AS SchemaName,
    p.rows AS RowCounts,
    SUM(a.total_pages) * 8 AS TotalSpaceKB, 
    CAST(ROUND(((SUM(a.total_pages) * 8) / 1024.00), 2) AS NUMERIC(36, 2)) AS TotalSpaceMB,
    SUM(a.used_pages) * 8 AS UsedSpaceKB, 
    CAST(ROUND(((SUM(a.used_pages) * 8) / 1024.00), 2) AS NUMERIC(36, 2)) AS UsedSpaceMB, 
    (SUM(a.total_pages) - SUM(a.used_pages)) * 8 AS UnusedSpaceKB,
    CAST(ROUND(((SUM(a.total_pages) - SUM(a.used_pages)) * 8) / 1024.00, 2) AS NUMERIC(36, 2)) AS UnusedSpaceMB
	FROM 
		sys.tables t
	INNER JOIN      
		sys.indexes i ON t.OBJECT_ID = i.object_id
	INNER JOIN 
		sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
	INNER JOIN 
		sys.allocation_units a ON p.partition_id = a.container_id
	LEFT OUTER JOIN 
		sys.schemas s ON t.schema_id = s.schema_id
	WHERE 
		t.NAME NOT LIKE 'dt%' 
		AND t.is_ms_shipped = 0
		AND i.OBJECT_ID > 255 
	GROUP BY 
		t.Name, s.Name, p.Rows
	ORDER BY 
		t.Name
END
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
	
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

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

-- Start CacheSite Merge
	
CREATE TABLE #CacheSiteIdTbl 
(
   Id bigint
);

CREATE Table #CacheSiteTracker(Id uniqueIdentifier, SiteName varchar(250), SiteLang varchar(250));

INSERT INTO #CacheSiteTracker(Id, SiteName, SiteLang)
SELECT cst.Id, cst.SiteName, cst.SiteLang
FROM CacheSiteTemp cst
WHERE cst.CacheQueueId = @CacheQueueId

UPDATE cs WITH (tablock)
SET SiteName = cst.SiteName, SiteLang = cst.SiteLang
OUTPUT inserted.Id INTO #CacheSiteIdTbl
FROM CacheSite cs
INNER JOIN #CacheSiteTracker cst
ON cs.SiteName = cst.SiteName AND cs.SiteLang = cs.SiteLang

INSERT INTO CacheSite WITH (tablock) (SiteName, SiteLang)
OUTPUT inserted.Id INTO #CacheSiteIdTbl
SELECT cst.SiteName, cst.SiteLang
FROM #CacheSiteTracker cst
LEFT JOIN CacheSite cs
ON cs.SiteName = cst.SiteName AND cs.SiteLang = cst.SiteLang
WHERE cs.Id IS NULL

DECLARE @CacheSiteId bigint
SET @CacheSiteId = (SELECT TOP(1) Id FROM #CacheSiteIdTbl)

-- End CacheSite Merge

-- Start CacheHtml Merge

CREATE TABLE #CacheHtmlIdTbl
(
   Id bigint
);

CREATE Table #CacheHtmlTblTracker(Id uniqueIdentifier, CacheSiteId bigint, HtmlCacheKey varchar(250), HtmlCacheResult varchar(max), HtmlCacheKeyHash binary(64));

INSERT INTO #CacheHtmlTblTracker(Id, CacheSiteId, HtmlCacheKey, HtmlCacheResult, HtmlCacheKeyHash)
SELECT cht.Id, @CacheSiteId, cht.HtmlCacheKey, cht.HtmlCacheResult, cht.HtmlCacheKeyHash
FROM CacheHtmlTemp cht
WHERE cht.CacheQueueId = @CacheQueueId

UPDATE ch WITH (tablock)
SET HtmlCacheKey = cht.HtmlCacheKey, HtmlCacheResult = cht.HtmlCacheResult, HtmlCacheKeyHash = cht.HtmlCacheKeyHash
OUTPUT inserted.Id INTO #CacheHtmlIdTbl
FROM CacheHtml ch
INNER JOIN #CacheHtmlTblTracker cht
ON ch.HtmlCacheKeyHash = cht.HtmlCacheKeyHash

INSERT INTO CacheHtml WITH (tablock) (CacheSiteId, HtmlCacheKey, HtmlCacheResult, HtmlCacheKeyHash)
OUTPUT inserted.Id INTO #CacheHtmlIdTbl
SELECT @CacheSiteId, cht.HtmlCacheKey, cht.HtmlCacheResult, cht.HtmlCacheKeyHash
FROM #CacheHtmlTblTracker cht
LEFT JOIN CacheHtml ch
ON ch.HtmlCacheKeyHash = cht.HtmlCacheKeyHash AND ch.CacheSiteId = cht.CacheSiteId
WHERE ch.Id IS NULL

DECLARE @CacheHtmlId bigint
SET @CacheHtmlId = (SELECT TOP(1) Id FROM #CacheHtmlIdTbl)

-- End CacheHtml Merge

-- Start CacheItem Merge

CREATE TABLE #CacheItemIdTbl
(
   Id bigint
);

CREATE Table #CacheItemTblTracker(Id uniqueIdentifier, ItemId uniqueidentifier, ItemLang varchar(250));

INSERT INTO #CacheItemTblTracker(Id, ItemId, ItemLang)
SELECT cit.Id, cit.ItemId, cit.ItemLang
FROM CacheItemTemp cit
WHERE cit.CacheQueueId = @CacheQueueId

UPDATE ci WITH (tablock)
SET ItemId = cit.ItemId, ItemLang = cit.ItemLang
OUTPUT inserted.Id INTO #CacheItemIdTbl
FROM CacheItem ci
INNER JOIN #CacheItemTblTracker cit
ON ci.ItemId = cit.ItemId AND ci.ItemLang = cit.ItemLang

INSERT INTO CacheItem WITH (tablock) (ItemId, ItemLang)
OUTPUT inserted.Id INTO #CacheItemIdTbl
SELECT cit.ItemId, cit.ItemLang
FROM #CacheItemTblTracker cit
LEFT JOIN CacheItem ci
ON ci.ItemId = cit.ItemId AND ci.ItemLang = cit.ItemLang
WHERE ci.Id IS NULL

-- End CacheItem Merge

-- Start CacheHtml_CacheItem Merge
UPDATE chci WITH (tablock)
SET CacheHtmlId = @CacheHtmlId, CacheItemId = cit.Id
FROM CacheHtml_CacheItem chci
INNER JOIN #CacheItemIdTbl cit
ON chci.CacheHtmlId = @CacheHtmlId AND chci.CacheItemId = cit.Id

INSERT INTO CacheHtml_CacheItem WITH (tablock) (CacheHtmlId, CacheItemId)
SELECT @CacheHtmlId, cit.Id
FROM #CacheItemIdTbl cit
LEFT JOIN CacheHtml_CacheItem chci
ON chci.CacheHtmlId = @CacheHtmlId AND chci.CacheItemId = cit.Id
WHERE chci.Id IS NULL

-- End CacheHtml_CacheItem Merge
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [usp_ProcessCacheDataOld] 
	@CacheQueueId		BIGINT
AS
	
CREATE TABLE #CacheSiteIdTbl 
(
   Id bigint
);

WITH CacheSiteCTE (SiteName, SiteLang)
AS
(
	SELECT cst.SiteName, cst.SiteLang
	FROM CacheSiteTemp cst
	WHERE cst.CacheQueueId = @CacheQueueId
)
MERGE [CacheSite] as t--WITH (HOLDLOCK) AS t 
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
OUTPUT inserted.Id INTO #CacheSiteIdTbl;

DECLARE @CacheSiteId bigint
SET @CacheSiteId = (SELECT TOP(1) Id FROM #CacheSiteIdTbl)

CREATE TABLE #CacheHtmlIdTbl
(
   Id bigint
);

WITH CacheHtmlTblCTE(HtmlCacheKey, HtmlCacheResult, HtmlCacheKeyHash)
AS
(
	SELECT cht.HtmlCacheKey, cht.HtmlCacheResult, cht.HtmlCacheKeyHash
	FROM CacheHtmlTemp cht
	WHERE cht.CacheQueueId = @CacheQueueId
)
MERGE [CacheHtml] as t--WITH (HOLDLOCK) AS t 
USING 
(
	SELECT 	HtmlCacheKey, HtmlCacheResult, HtmlCacheKeyHash
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
	t.HtmlCacheResult = s.HtmlCacheResult,
	t.HtmlCacheKeyHash = s.HtmlCacheKeyHash
WHEN NOT MATCHED 
	THEN INSERT (CacheSiteId, HtmlCacheKey, HtmlCacheResult, HtmlCacheKeyHash) 
    VALUES (@CacheSiteId, s.HtmlCacheKey, s.HtmlCacheResult, s.HtmlCacheKeyHash)
OUTPUT inserted.Id INTO #CacheHtmlIdTbl;

DECLARE @CacheHtmlId bigint
SET @CacheHtmlId = (SELECT TOP(1) Id FROM #CacheHtmlIdTbl)

CREATE TABLE #CacheItemIdTbl
(
   Id bigint
);

WITH CacheItemTblCTE(ItemId, ItemLang)
AS
(
	SELECT cit.ItemId, cit.ItemLang
	FROM CacheItemTemp cit
	WHERE cit.CacheQueueId = @CacheQueueId
)
MERGE [CacheItem] as t-- WITH (HOLDLOCK) AS t 
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
OUTPUT inserted.Id INTO #CacheItemIdTbl;

MERGE [CacheHtml_CacheItem] as t-- WITH (HOLDLOCK) AS t
USING
(
	SELECT @CacheHtmlId as CacheHtmlId, ciidtbl.Id as CacheItemId
	FROM #CacheItemIdTbl ciidtbl
) as s
ON
(
	s.CacheHtmlId = t.CacheHtmlId AND s.CacheItemId = t.CacheItemId
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
	
	CREATE TABLE #CacheQueuesToClear (CacheQueueId bigint, SiteLang varchar(250), ItemId uniqueidentifier, ItemLang varchar(250), IsDeleted bit)

	INSERT INTO #CacheQueuesToClear (CacheQueueId, ItemId, ItemLang, IsDeleted)
	SELECT cit.CacheQueueId, cit.ItemId, cit.ItemLang, cit.IsDeleted
	FROM CacheItemTemp cit
	WHERE cit.CacheQueueId = @CacheQueueId


	CREATE TABLE #CacheEntriesToClear (SiteName varchar(250), SiteLang varchar(250), CacheHtmlId bigint, CacheHtmlCacheItemId bigint, HtmlCacheKey varchar(5000), CacheItemId bigint)

	INSERT INTO #CacheEntriesToClear (SiteName, SiteLang, CacheHtmlId, CacheHtmlCacheItemId, HtmlCacheKey, CacheItemId)
	SELECT cs.SiteName, cs.SiteLang, chci2.CacheHtmlId, chci2.Id, ch.HtmlCacheKey, chci2.CacheItemId
	FROM #CacheQueuesToClear cqc
	INNER JOIN CacheItem ci ON ci.ItemId = cqc.ItemId AND ci.ItemLang = cqc.ItemLang
	INNER JOIN CacheHtml_CacheItem chci1 ON ci.Id = chci1.CacheItemId
	INNER JOIN CacheHtml_CacheItem chci2 ON chci1.CacheHtmlId = chci2.CacheHtmlId
	--INNER JOIN CacheHtml_CacheItem chci3 ON chci2.CacheItemId = chci3.CacheItemId
	INNER JOIN CacheHtml ch ON chci2.CacheHtmlId = ch.Id
	INNER JOIN CacheSite cs ON ch.CacheSiteId = cs.Id

	DELETE chci FROM CacheHtml_CacheItem chci 
	INNER JOIN #CacheEntriesToClear r on r.CacheHtmlCacheItemId = chci.Id

	-- here we delete orphaned records
	DELETE ci FROM CacheItem ci 
	LEFT JOIN #CacheQueuesToClear r on r.ItemId = ci.ItemId
	WHERE r.IsDeleted = 1

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

	ALTER INDEX IX_CacheHtml_CacheItem_CacheHtmlId ON CacheHtml_CacheItem DISABLE
	ALTER INDEX IX_CacheHtml_CacheItem_CacheItemId ON CacheHtml_CacheItem DISABLE
	ALTER INDEX IX_CacheItem_ItemId ON CacheItem DISABLE 
	
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
	AND cst.SiteLang = @SiteLang

	DELETE	cht FROM CacheHtmlTemp cht
	JOIN	#CacheQueuesToClear r on r.Id = cht.CacheQueueId AND r.Id <> @CacheQueueId

	DELETE	cit FROM CacheItemTemp cit
	JOIN	#CacheQueuesToClear r on r.Id = cit.CacheQueueId AND r.Id <> @CacheQueueId

	DELETE	cst FROM CacheSiteTemp cst
	JOIN	#CacheQueuesToClear r on r.Id = cst.CacheQueueId AND r.Id <> @CacheQueueId

	DELETE	cq FROM CacheQueue cq
	JOIN	#CacheQueuesToClear r on r.Id = cq.Id AND r.Id <> @CacheQueueId
	
	CREATE TABLE #ResultToClear (CacheSiteId bigint, CacheHtmlId bigint, CacheHtmlCacheItemId bigint, CacheItemId bigint)

	INSERT INTO #ResultToClear (CacheSiteId, CacheHtmlId, CacheHtmlCacheItemId, CacheItemId)
	SELECT	cs.Id as CacheSiteId, 
			ch.Id as CacheHtmlId, 
			chci.Id as CacheHtmlCacheItemId, 
			ci.Id as CacheItemId
	FROM	CacheSite cs
	INNER JOIN CacheHtml ch	ON cs.Id = ch.CacheSiteId	
	INNER JOIN CacheHtml_CacheItem chci	ON ch.Id = chci.CacheHtmlId
	INNER JOIN CacheItem ci	ON chci.CacheItemId = ci.Id
	WHERE cs.SiteName = @SiteName AND cs.SiteLang = @SiteLang

	DELETE	chci FROM CacheHtml_CacheItem chci
	JOIN	#ResultToClear r on r.CacheHtmlCacheItemId = chci.id

	DELETE	ch FROM CacheHtml ch
	JOIN	#ResultToClear r on r.CacheHtmlId = ch.Id

	DELETE	ci FROM CacheItem ci
	JOIN	#ResultToClear r on r.CacheItemId = ci.Id

	DELETE	cs FROM CacheSite cs
	JOIN	#ResultToClear r on r.CacheSiteId = cs.Id

	ALTER INDEX IX_CacheHtml_CacheItem_CacheHtmlId ON CacheHtml_CacheItem REBUILD
	ALTER INDEX IX_CacheHtml_CacheItem_CacheItemId ON CacheHtml_CacheItem REBUILD
	ALTER INDEX IX_CacheItem_ItemId ON CacheItem REBUILD 
		
	SELECT @CacheQueueMessageTypeId AS CacheQueueMessageTypeId, @SiteName, @SiteLang, '' AS HtmlCacheKey
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

	INSERT INTO CacheHtmlTemp (CacheQueueId, HtmlCacheKey, HtmlCacheResult, HtmlCacheKeyHash)
	VALUES (@CacheQueueId, @HtmlCacheKey, @HtmlCacheResult, (hashbytes('SHA2_512',@HtmlCacheKey)))

	INSERT INTO CacheItemTemp(CacheQueueId, ItemId, ItemLang, IsDeleted)
	SELECT @CacheQueueId, Id, Lang, IsDeleted FROM @Ids
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

	INSERT INTO CacheItemTemp(CacheQueueId, ItemId, ItemLang, IsDeleted)
	SELECT @CacheQueueId, Id, Lang, IsDeleted FROM @Ids

	INSERT INTO CacheSiteTemp(CacheQueueId, SiteLang)
	SELECT @CacheQueueId, @SiteLang
END
GO
USE [master]
GO
ALTER DATABASE [HtmlCache] SET  READ_WRITE 
GO
