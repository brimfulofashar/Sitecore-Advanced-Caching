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
CREATE TABLE [Cache_CacheItem](
	[Id] [uniqueidentifier] NOT NULL,
	[CacheHtmlId] [uniqueidentifier] NOT NULL,
	[CacheItemId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Cache_CacheItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
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
CREATE TABLE [CacheHtmlTemp](
	[Id] [uniqueidentifier] NOT NULL,
	[CacheQueueId] [bigint] NOT NULL,
	[HtmlCacheKey] [varchar](5000) NOT NULL,
	[HtmlCacheKeyHash]  AS (hashbytes('SHA2_512',[HtmlCacheKey])) PERSISTED,
	[HtmlCacheResult] [varchar](max) NOT NULL,
 CONSTRAINT [PK_CacheTemp] PRIMARY KEY CLUSTERED 
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
	[Site] [varchar](250) NULL,
	[Language] [varchar](250) NULL,
	[Processing] [bit] NOT NULL,
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
	[SiteName] [varchar](250) NOT NULL,
	[SiteLang] [varchar](250) NOT NULL,
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
CREATE NONCLUSTERED INDEX [IX_CacheQueue_CacheQueueMessageTypeId] ON [CacheQueue]
(
	[CacheQueueMessageTypeId] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [Cache_CacheItem] ADD  CONSTRAINT [DF_Cache_CacheItem_Id]  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [CacheHtml] ADD  CONSTRAINT [DF_Cache_Id]  DEFAULT (newsequentialid()) FOR [Id]
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
ALTER TABLE [Cache_CacheItem]  WITH CHECK ADD  CONSTRAINT [FK_Cache_CacheItem_Cache] FOREIGN KEY([CacheHtmlId])
REFERENCES [CacheHtml] ([Id])
GO
ALTER TABLE [Cache_CacheItem] CHECK CONSTRAINT [FK_Cache_CacheItem_Cache]
GO
ALTER TABLE [Cache_CacheItem]  WITH CHECK ADD  CONSTRAINT [FK_Cache_CacheItem_CacheItem] FOREIGN KEY([CacheItemId])
REFERENCES [CacheItem] ([Id])
GO
ALTER TABLE [Cache_CacheItem] CHECK CONSTRAINT [FK_Cache_CacheItem_CacheItem]
GO
ALTER TABLE [CacheHtml]  WITH CHECK ADD  CONSTRAINT [FK_Cache_CacheSite] FOREIGN KEY([CacheSiteId])
REFERENCES [CacheSite] ([Id])
GO
ALTER TABLE [CacheHtml] CHECK CONSTRAINT [FK_Cache_CacheSite]
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

CREATE PROCEDURE [usp_QueueCacheData]
	@SiteName varchar(250),
	@SiteLang varchar(250),
	@HtmlCacheKey varchar(5000),
	@HtmlCacheResult varchar(max),
	@Ids GuidList READONLY
AS
BEGIN
	DECLARE @CacheQueueIdTbl Table (Id bigint)

	INSERT INTO CacheQueue(CacheQueueMessageTypeId ,Site ,Language ,Processing)
	OUTPUT inserted.Id INTO @CacheQueueIdTbl
	VALUES(1 ,NULL ,NULL ,0)

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

	-- CREATE SPROCS

	CREATE PROCEDURE [usp_SyncCacheData] 
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

MERGE [Cache_CacheItem] WITH (HOLDLOCK) AS t
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

DELETE FROM CacheQueue where Id = @CacheQueueId
GO
USE [master]
GO
ALTER DATABASE [HtmlCache] SET  READ_WRITE 
GO