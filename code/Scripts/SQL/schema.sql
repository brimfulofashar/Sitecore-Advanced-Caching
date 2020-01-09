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
CREATE TYPE [GuidList] AS TABLE(
	[Id] [uniqueidentifier] NOT NULL
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheItem](
	[Id] [uniqueidentifier] NOT NULL,
	[ItemId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CacheItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
 CONSTRAINT [PK_CacheItemTemp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [CacheQueueMessageType] ON 
GO
INSERT [CacheQueueMessageType] ([Id], [MessageType]) VALUES (1, N'AddToCache')
GO
INSERT [CacheQueueMessageType] ([Id], [MessageType]) VALUES (2, N'DeleteFromCache')
GO
INSERT [CacheQueueMessageType] ([Id], [MessageType]) VALUES (3, N'DeleteSiteFromCache')
GO
INSERT [CacheQueueMessageType] ([Id], [MessageType]) VALUES (4, N'DeleteSiteFromCacheAllLanguages')
GO
INSERT [CacheQueueMessageType] ([Id], [MessageType]) VALUES (5, N'DeleteSiteFromCacheAllSites')
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheHtml_CacheItem_CacheHtmlId_CacheItemId] ON [CacheHtml_CacheItem]
(
	[CacheHtmlId] ASC,
	[CacheItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheHtmlTemp_CacheQueueId] ON [CacheHtmlTemp]
(
	[CacheQueueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheItem_ItemId] ON [CacheItem]
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheItemTemp_CacheQueueId] ON [CacheItemTemp]
(
	[CacheQueueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheItemTemp_ItemId] ON [CacheItemTemp]
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheQueue_CacheQueueMessageTypeId] ON [CacheQueue]
(
	[CacheQueueMessageTypeId] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheQueue_Processing] ON [CacheQueue]
(
	[Processing] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheQueue_UpdateVersion] ON [CacheQueue]
(
	[UpdateVersion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CacheSite_SiteLang] ON [CacheSite]
(
	[SiteLang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CacheSite_SiteName] ON [CacheSite]
(
	[SiteName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheSiteTemp_CacheQueueId] ON [CacheSiteTemp]
(
	[CacheQueueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CacheSiteTemp_SiteLang] ON [CacheSiteTemp]
(
	[SiteLang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CacheSiteTemp_SiteName] ON [CacheSiteTemp]
(
	[SiteName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
ON DELETE CASCADE
GO
ALTER TABLE [CacheHtmlTemp] CHECK CONSTRAINT [FK_CacheHtmlTemp_CacheQueue]
GO
ALTER TABLE [CacheItemTemp]  WITH CHECK ADD  CONSTRAINT [FK_CacheItemTemp_CacheQueue] FOREIGN KEY([CacheQueueId])
REFERENCES [CacheQueue] ([Id])
ON DELETE CASCADE
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
ON DELETE CASCADE
GO
ALTER TABLE [CacheSiteTemp] CHECK CONSTRAINT [FK_CacheSiteTemp_CacheQueue]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [usp_LockAndProcessCacheQueueEntry] 
	@ProcessingBy Varchar(250)
AS
BEGIN
	DECLARE @CacheQueueId bigint
	DECLARE @CacheQueueMessageTypeId int
	DECLARE @Processing bit
	DECLARE @UpdateVersion timestamp

	DECLARE @BlockFurtherProcessing bit

	SET @BlockFurtherProcessing = (SELECT COUNT(Id) FROM CacheQueue Where @CacheQueueMessageTypeId > 1 AND Processing = 1);

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

			DECLARE @CacheQueueIdTbl Table (Id bigint)
			
			UPDATE CacheQueue
			SET Processing = 1,
			ProcessingBy = @ProcessingBy
			output inserted.id
			INTO @CacheQueueIdTbl
			WHERE Id = @CacheQueueId
			AND UpdateVersion = @UpdateVersion
			
			SELECT TOP(1) @CacheQueueId = Id FROM @CacheQueueIdTbl AS CacheQueueID

			IF @CacheQueueId IS NOT NULL AND @CacheQueueMessageTypeId = 1
			BEGIN
				EXEC usp_ProcessCacheData @CacheQueueId
			END
			ELSE IF @CacheQueueId IS NOT NULL AND @CacheQueueMessageTypeId = 2
			BEGIN
				EXEC usp_ProcessDeleteHtmlFromCache @CacheQueueId
			END
			ELSE IF @CacheQueueId IS NOT NULL AND (@CacheQueueMessageTypeId = 3 OR @CacheQueueMessageTypeId = 4 OR @CacheQueueMessageTypeId = 5)
			BEGIN
				EXEC usp_ProcessDeleteSiteFromCache @CacheQueueId
			END

			DELETE FROM CacheQueue WHERE Id = @CacheQueueId
		END
		ELSE
		BEGIN
			SELECT NULL
		END
	END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	-- CREATE SPROCS

	CREATE PROCEDURE [usp_ProcessCacheData] 
		@CacheQueueId		BIGINT
	AS
	
DECLARE @CacheSiteTbl Table (SiteName varchar(250), SiteLang varchar(250))

INSERT INTO @CacheSiteTbl(SiteName, SiteLang)
select cst.SiteName, cst.SiteLang
FROM CacheSiteTemp cst
WHERE cst.CacheQueueId = @CacheQueueId


DECLARE @CacheSiteIdTbl TABLE
(
   Id uniqueidentifier
);

MERGE [CacheSite] WITH (HOLDLOCK) AS t 
USING 
(
	SELECT 	SiteName, SiteLang
	FROM	@CacheSiteTbl
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

DECLARE @CacheHtmlTbl Table (HtmlCacheKey varchar(5000), HtmlCacheKeyHash varbinary(8000), HtmlCacheResult varchar(max))
INSERT INTO @CacheHtmlTbl(HtmlCacheKey, HtmlCacheKeyHash, HtmlCacheResult)
select cht.HtmlCacheKey, cht.HtmlCacheKeyHash, cht.HtmlCacheResult
FROM CacheHtmlTemp cht
WHERE cht.CacheQueueId = @CacheQueueId


DECLARE @CacheSiteId uniqueidentifier
SET @CacheSiteId = (SELECT TOP(1) Id FROM @CacheSiteIdTbl)

DECLARE @CacheHtmlIdTbl TABLE
(
   Id uniqueidentifier
);

MERGE [CacheHtml] WITH (HOLDLOCK) AS t 
USING 
(
	SELECT 	HtmlCacheKey, HtmlCacheKeyHash, HtmlCacheResult
	FROM	@CacheHtmlTbl
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

DECLARE @CacheItemTbl Table (ItemId uniqueidentifier)

INSERT INTO @CacheItemTbl(ItemId)
select cit.ItemId
FROM CacheItemTemp cit
WHERE cit.CacheQueueId = @CacheQueueId

DECLARE @CacheItemIdTbl TABLE
(
   Id uniqueidentifier
);

MERGE [CacheItem] WITH (HOLDLOCK) AS t 
USING 
(
	SELECT 	ItemId
	FROM	@CacheItemTbl
) AS s 
ON 
(
	s.ItemId = t.ItemId
) 
WHEN MATCHED
	THEN UPDATE SET
	t.ItemId = s.ItemId
WHEN NOT MATCHED 
	THEN INSERT (ItemId) 
    VALUES (s.ItemId)
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
	@CacheQueueId bigint
AS
BEGIN
	 DECLARE @Result TABLE (HtmlCacheId uniqueIdentifier, CacheHtmlCacheItemId uniqueIdentifier, HtmlCacheKey varchar(5000), CacheItemId uniqueIdentifier)

	 INSERT INTO @Result (HtmlCacheId, CacheHtmlCacheItemId, HtmlCacheKey, CacheItemId)
	 SELECT ch.Id as HtmlCacheId, cci2.Id as CacheCacheItemId, ch.HtmlCacheKey, ci2.Id CacheItemId
	 FROM CacheItemTemp cit
	 INNER JOIN CacheItem ci1
	 on cit.ItemId = ci1.ItemId
	 INNER JOIN CacheHtml_CacheItem cci1
	 on ci1.Id = cci1.CacheItemId
	 INNER JOIN CacheHtml ch
	 on cci1.CacheHtmlId = ch.Id
	 INNER JOIN CacheHtml_CacheItem cci2
	 ON ch.Id = cci2.CacheHtmlId
	 INNER JOIN CacheItem ci2
	 ON cci2.CacheItemId = ci2.Id
	 INNER JOIN CacheQueue cq
	 ON cit.CacheQueueId = cq.Id
	 INNER JOIN CacheSiteTemp cst
	 ON cq.Id = cst.CacheQueueId
	 where cq.Id = @CacheQueueId

	 DELETE FROM CacheHtml_CacheItem WHERE Id in (SELECT CacheHtmlCacheItemId FROM @Result)
	 DELETE FROM CacheHtml WHERE Id in (Select HtmlCacheId FROM @Result)
	 DELETE FROM CacheItem WHERE Id in (Select CacheItemId FROM @Result)

	 SELECT DISTINCT HtmlCacheKey FROM @Result
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [usp_ProcessDeleteSiteFromCache]
	@CacheQueueId bigint
AS
BEGIN
	
	DECLARE @ResultToClear TABLE (CacheSiteId uniqueIdentifier, CacheHtmlId uniqueIdentifier, CacheHtmlCacheItemId uniqueIdentifier, CacheItemId uniqueIdentifier)

	INSERT INTO @ResultToClear (CacheSiteId, CacheHtmlId, CacheHtmlCacheItemId, CacheItemId)
	SELECT cs.Id as CacheSiteId, ch.Id as CacheHtmlId, chci.Id as CacheHtmlCacheItemId, ci.Id as CacheItemId
	FROM CacheQueue cq
	INNER JOIN CacheSiteTemp cst
	ON cq.Id = cst.CacheQueueId
	INNER JOIN CacheSite cs
	ON cst.SiteName = cs.SiteName
	AND cst.SiteLang = cs.SiteLang
	INNER JOIN CacheHtml ch
	ON cs.Id = ch.CacheSiteId
	INNER JOIN CacheHtml_CacheItem chci
	ON ch.Id = chci.CacheHtmlId
	INNER JOIN CacheItem ci
	ON chci.CacheItemId = ci.Id

	DELETE FROM CacheHtml_CacheItem WHERE Id IN (SELECT CacheHtmlCacheItemId FROM @ResultToClear)
	DELETE FROM CacheHtml WHERE Id IN (SELECT CacheHtmlId FROM @ResultToClear)
	DELETE FROM CacheItem WHERE Id IN (SELECT CacheItemId FROM @ResultToClear)
	DELETE FROM CacheSite WHERE Id IN (SELECT CacheSiteId FROM @ResultToClear)

	SELECT SiteName, SiteLang FROM CacheSiteTemp WHERE CacheQueueId = @CacheQueueId
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
	@Ids GuidList READONLY
AS
BEGIN
	DECLARE @CacheQueueIdTbl Table (Id bigint)

	INSERT INTO CacheQueue(CacheQueueMessageTypeId, Processing)
	OUTPUT inserted.Id INTO @CacheQueueIdTbl
	VALUES(1 ,0)

	DECLARE @CacheQueueId bigint
	SET @CacheQueueId = (SELECT TOP(1) Id FROM @CacheQueueIdTbl)

	INSERT INTO CacheSiteTemp(CacheQueueId, SiteName, SiteLang)
	VALUES(@CacheQueueId, @SiteName, @SiteLang)

	INSERT INTO CacheHtmlTemp (CacheQueueId, HtmlCacheKey, HtmlCacheResult)
	VALUES (@CacheQueueId, @HtmlCacheKey, @HtmlCacheResult)

	INSERT INTO CacheItemTemp(CacheQueueId, ItemId)
	SELECT @CacheQueueId, Id FROM @Ids
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [usp_QueueDeleteSiteFromCache]
	@SiteName varchar(250),
	@SiteLang varchar(250),
	@CacheQueueMessageTypeId int
AS
BEGIN
	DECLARE @CacheQueueIdTbl Table (Id bigint)

	INSERT INTO CacheQueue(CacheQueueMessageTypeId, Processing)
	OUTPUT inserted.Id INTO @CacheQueueIdTbl
	VALUES(@CacheQueueMessageTypeId ,0)

	DECLARE @CacheQueueId bigint
	SET @CacheQueueId = (SELECT TOP(1) Id FROM @CacheQueueIdTbl)

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
	@Ids GuidList READONLY
AS
BEGIN
	DECLARE @CacheQueueIdTbl Table (Id bigint)

	INSERT INTO CacheQueue(CacheQueueMessageTypeId, Processing)
	OUTPUT inserted.Id INTO @CacheQueueIdTbl
	VALUES(2 ,0)

	DECLARE @CacheQueueId bigint
	SET @CacheQueueId = (SELECT TOP(1) Id FROM @CacheQueueIdTbl)

	INSERT INTO CacheItemTemp(CacheQueueId, ItemId)
	SELECT @CacheQueueId, Id FROM @Ids

	INSERT INTO CacheSiteTemp(CacheQueueId, SiteLang)
	SELECT @CacheQueueId, @SiteLang
END
GO
USE [master]
GO
ALTER DATABASE [HtmlCache] SET  READ_WRITE 
GO
