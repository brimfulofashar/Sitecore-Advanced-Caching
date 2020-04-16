USE [master]
GO
CREATE DATABASE [HtmlCache] CONTAINMENT = PARTIAL
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
CREATE TYPE [CacheHtml_CacheItem_TVP] AS TABLE(
	[Id] [bigint] NOT NULL,
	[CacheHtmlId] [bigint] NOT NULL,
	[CacheItemId] [bigint] NOT NULL
)
GO
CREATE TYPE [CacheHtml_TVP] AS TABLE(
	[Id] [bigint] NOT NULL,
	[CacheSiteId] [bigint] NOT NULL,
	[HtmlCacheKey] [varchar](5000) NOT NULL,
	[HtmlCacheResult] [varchar](max) NOT NULL,
	[HtmlCacheKeyHash] [binary](64) NOT NULL
)
GO
CREATE TYPE [CacheItem_TVP] AS TABLE(
	[Id] [bigint] NOT NULL,
	[ItemId] [uniqueidentifier] NOT NULL,
	[ItemLang] [varchar](250) NOT NULL,
	[IsDeleted] [bit] NOT NULL
)
GO
CREATE TYPE [CacheSite_TVP] AS TABLE(
	[Id] [bigint] NOT NULL,
	[SiteName] [varchar](250) NOT NULL,
	[SiteLang] [varchar](250) NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ARITHABORT ON
GO
CREATE TABLE [CacheHtml](
	[ConcurrencyId]  AS (CONVERT([tinyint],abs([Id])%(12))) PERSISTED NOT NULL,
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CacheSiteId] [bigint] NOT NULL,
	[HtmlCacheKey] [varchar](5000) NOT NULL,
	[HtmlCacheResult] [varchar](max) NOT NULL,
	[HtmlCacheKeyHash] [binary](64) NOT NULL,
 CONSTRAINT [PK_CacheHtml] PRIMARY KEY CLUSTERED 
(
	[ConcurrencyId] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ARITHABORT ON
GO
CREATE TABLE [CacheHtml_CacheItem](
	[ConcurrencyId]  AS (CONVERT([tinyint],abs([Id])%(12))) PERSISTED NOT NULL,
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CacheHtmlId] [bigint] NOT NULL,
	[CacheItemId] [bigint] NOT NULL,
 CONSTRAINT [PK_CacheHtml_CacheItem] PRIMARY KEY CLUSTERED 
(
	[ConcurrencyId] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ARITHABORT ON
GO
CREATE TABLE [CacheItem](
	[ConcurrencyId]  AS (CONVERT([tinyint],abs([Id])%(12))) PERSISTED NOT NULL,
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ItemId] [uniqueidentifier] NOT NULL,
	[ItemLang] [varchar](250) NOT NULL,
 CONSTRAINT [PK_CacheItem] PRIMARY KEY CLUSTERED 
(
	[ConcurrencyId] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ARITHABORT ON
GO
CREATE TABLE [CacheSite](
	[ConcurrencyId]  AS (CONVERT([tinyint],abs([Id])%(12))) PERSISTED NOT NULL,
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SiteName] [varchar](250) NOT NULL,
	[SiteLang] [varchar](250) NOT NULL,
 CONSTRAINT [PK_CacheSite] PRIMARY KEY CLUSTERED 
(
	[ConcurrencyId] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
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
CREATE UNIQUE NONCLUSTERED INDEX [IX_CacheHtml_Id] ON [CacheHtml]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
CREATE UNIQUE NONCLUSTERED INDEX [IX_CacheHtml_CacheItem_Id] ON [CacheHtml_CacheItem]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_CacheItem_Id] ON [CacheItem]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
CREATE UNIQUE NONCLUSTERED INDEX [IX_CacheSite_Id] ON [CacheSite]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [usp_DeleteCacheData]
	@CacheItem_TVP CacheItem_TVP READONLY
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

	BEGIN TRANSACTION

	CREATE TABLE #CacheEntriesToClear (SiteName varchar(250), SiteLang varchar(250), HtmlCacheKey varchar(5000), CacheHtmlId bigint, CacheHtmlCacheItemId bigint, CacheItemId bigint)

	INSERT INTO #CacheEntriesToClear (SiteName, SiteLang, HtmlCacheKey, CacheHtmlId, CacheHtmlCacheItemId, CacheItemId)
	SELECT cs.SiteName, cs.SiteLang, ch.HtmlCacheKey, chci2.CacheHtmlId, chci2.Id, chci2.CacheItemId
	FROM @CacheItem_TVP cqc
	INNER JOIN CacheItem ci WITH (UPDLOCK)
	ON ci.ItemId = cqc.ItemId AND ci.ItemLang = cqc.ItemLang
	INNER JOIN CacheHtml_CacheItem chci1 
	ON ci.Id = chci1.CacheItemId
	INNER JOIN CacheHtml_CacheItem chci2 WITH (UPDLOCK)
	ON chci1.CacheHtmlId = chci2.CacheHtmlId
	INNER JOIN CacheHtml ch WITH (UPDLOCK)
	ON chci2.CacheHtmlId = ch.Id
	INNER JOIN CacheSite cs WITH (UPDLOCK)
	ON ch.CacheSiteId = cs.Id

	
	DELETE chci 
	FROM CacheHtml_CacheItem chci 
	INNER JOIN #CacheEntriesToClear r on r.CacheHtmlCacheItemId = chci.Id

	DELETE ci 
	FROM CacheItem ci 
	INNER JOIN #CacheEntriesToClear r on r.CacheItemId = ci.Id

	DELETE ch 
	FROM CacheHtml ch
	INNER JOIN #CacheEntriesToClear r on r.CacheHtmlId = ch.Id

	SELECT DISTINCT SiteName, SiteLang, 
	SUBSTRING(
	(
		SELECT '|' + HtmlCacheKey
		FROM #CacheEntriesToClear cetc
		ORDER BY cetc.HtmlCacheKey
		FOR XML PATH('')
		),2,200000
	) 
		AS HtmlCacheKey  
		FROM #CacheEntriesToClear

	COMMIT

END


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [usp_DeleteCacheDataForSite]
	@SiteName varchar(250),
	@SiteLang varchar(250)
AS
BEGIN

	ALTER INDEX IX_CacheHtml_CacheItem_CacheHtmlId ON CacheHtml_CacheItem DISABLE
	ALTER INDEX IX_CacheHtml_CacheItem_CacheItemId ON CacheHtml_CacheItem DISABLE
	ALTER INDEX IX_CacheItem_ItemId ON CacheItem DISABLE 
	
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
	
	SELECT @SiteName as SiteName, @SiteLang	as SiteLang
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
CREATE PROCEDURE [usp_MergeCacheData] 
	@CacheSite_TVP CacheSite_TVP READONLY,
	@CacheHtml_TVP CacheHtml_TVP READONLY,
	@CacheHtml_CacheItem_TVP CacheHtml_CacheItem_TVP READONLY,
	@CacheItem_TVP CacheItem_TVP READONLY
AS
BEGIN
	-- Start CacheSite Merge

	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

	BEGIN TRANSACTION
	
	CREATE Table #CacheSiteMergeView(Id bigint, TempId bigint);

	INSERT INTO CacheSite (SiteName, SiteLang)
	SELECT cst.SiteName, cst.SiteLang
	FROM @CacheSite_TVP cst
	LEFT JOIN CacheSite cs WITH (UPDLOCK)
	ON cs.SiteName = cst.SiteName AND cs.SiteLang = cst.SiteLang
	WHERE cs.Id IS NULL

	INSERT INTO #CacheSiteMergeView (Id, TempId)
	SELECT cs.Id, cst.Id
	FROM CacheSite cs
	INNER JOIN @CacheSite_TVP cst ON cs.SiteName = cst.SiteName AND cs.SiteLang = cst.SiteLang

	-- End CacheSite Merge

	-- Start CacheHtml Merge


	CREATE Table #CacheHtmlMergeView(Id bigint, TempId bigint);

	INSERT INTO CacheHtml (CacheSiteId, HtmlCacheKey, HtmlCacheResult, HtmlCacheKeyHash)
	SELECT csmv.Id, cht.HtmlCacheKey, cht.HtmlCacheResult, cht.HtmlCacheKeyHash
	FROM @CacheHtml_TVP cht
	INNER JOIN #CacheSiteMergeView csmv 
	ON cht.Id = csmv.TempId
	LEFT JOIN CacheHtml ch  WITH (UPDLOCK)
	ON csmv.Id = ch.CacheSiteId AND ch.HtmlCacheKeyHash = cht.HtmlCacheKeyHash
	WHERE ch.Id IS NULL

	INSERT INTO #CacheHtmlMergeView (Id, TempId)
	SELECT ch.Id, cht.Id
	FROM CacheHtml ch
	INNER JOIN @CacheHtml_TVP cht ON ch.HtmlCacheKeyHash = cht.HtmlCacheKeyHash

	-- End CacheHtml Merge

	-- Start CacheItem Merge

	CREATE Table #CacheItemMergeView(Id bigint, TempId bigint);

	INSERT INTO CacheItem (ItemId, ItemLang)
	SELECT cit.ItemId, cit.ItemLang
	FROM @CacheItem_TVP cit
	LEFT JOIN CacheItem ci  WITH (UPDLOCK)
	ON cit.ItemId = ci.ItemId AND cit.ItemLang = ci.ItemLang
	WHERE ci.Id IS NULL

	INSERT INTO #CacheItemMergeView (Id, TempId)
	SELECT ci.Id, cit.Id
	FROM CacheItem ci
	INNER JOIN @CacheItem_TVP cit ON ci.ItemId = cit.ItemId AND ci.ItemLang = cit.ItemLang

	-- End CacheItem Merge

	-- Start CacheHtml_CacheItem Merge

	INSERT INTO CacheHtml_CacheItem (CacheHtmlId, CacheItemId)
	SELECT DISTINCT chmv.Id, cimv.Id
	FROM @CacheHtml_CacheItem_TVP chcit
	INNER JOIN #CacheHtmlMergeView chmv 
	ON chcit.CacheHtmlId = chmv.TempId
	INNER JOIN #CacheItemMergeView cimv 
	ON chcit.CacheItemId = cimv.TempId
	LEFT JOIN CacheHtml_CacheItem chci  WITH (UPDLOCK)
	ON chmv.Id = chci.CacheHtmlId AND cimv.Id = chci.CacheItemId
	WHERE chci.Id IS NULL

	-- End CacheHtml_CacheItem Merge
	COMMIT
END
GO
USE [master]
GO
ALTER DATABASE [HtmlCache] SET  READ_WRITE 
GO