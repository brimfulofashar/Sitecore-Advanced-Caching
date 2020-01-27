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
CREATE TYPE [CacheHtml_CacheItem_TVP] AS TABLE(
	[Id] [uniqueidentifier] NOT NULL,
	[CacheHtmlId] [uniqueidentifier] NOT NULL,
	[CacheItemId] [uniqueidentifier] NOT NULL
)
GO
CREATE TYPE [CacheHtml_TVP] AS TABLE(
	[Id] [uniqueidentifier] NOT NULL,
	[CacheSiteId] [uniqueidentifier] NOT NULL,
	[HtmlCacheKey] [varchar](5000) NOT NULL,
	[HtmlCacheResult] [varchar](max) NOT NULL,
	[HtmlCacheKeyHash] [binary](64) NOT NULL
)
GO
CREATE TYPE [CacheItem_TVP] AS TABLE(
	[Id] [uniqueidentifier] NOT NULL,
	[ItemId] [uniqueidentifier] NOT NULL,
	[ItemLang] [varchar](250) NOT NULL,
	[IsDeleted] [bit] NOT NULL
)
GO
CREATE TYPE [CacheSite_TVP] AS TABLE(
	[Id] [uniqueidentifier] NOT NULL,
	[SiteName] [varchar](250) NOT NULL,
	[SiteLang] [varchar](250) NOT NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheHtml](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MergeId] [bigint] NOT NULL,
	[CacheSiteId] [bigint] NOT NULL,
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
	[MergeId] [bigint] NOT NULL,
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
	[CacheSiteTempId] [uniqueidentifier] NOT NULL,
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
CREATE TABLE [CacheHtmlTemp_CacheItemTemp](
	[Id] [uniqueidentifier] NOT NULL,
	[CacheQueueId] [bigint] NOT NULL,
	[CacheHtmlTempId] [uniqueidentifier] NOT NULL,
	[CacheItemTempId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CacheHtmlTemp_CacheItemTemp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheItem](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MergeId] [bigint] NOT NULL,
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
	[MergeId] [bigint] NOT NULL,
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
CREATE NONCLUSTERED INDEX [IX_CacheHtml_MergeId] ON [CacheHtml]
(
	[MergeId] ASC
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
CREATE NONCLUSTERED INDEX [IX_CacheHtml_CacheItem_MergeId] ON [CacheHtml_CacheItem]
(
	[MergeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheHtmlTemp_CacheQueueId] ON [CacheHtmlTemp]
(
	[CacheQueueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheHtmlTemp_CacheSiteTempId] ON [CacheHtmlTemp]
(
	[CacheSiteTempId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CacheHtmlTemp_HtmlCacheKeyHash] ON [CacheHtmlTemp]
(
	[HtmlCacheKeyHash] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheHtmlTemp_CacheItemTemp_CacheHtmlTempId] ON [CacheHtmlTemp_CacheItemTemp]
(
	[CacheHtmlTempId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheHtmlTemp_CacheItemTemp_CacheItemTempId] ON [CacheHtmlTemp_CacheItemTemp]
(
	[CacheItemTempId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CacheHtmlTemp_CacheItemTemp_CacheQueueId] ON [CacheHtmlTemp_CacheItemTemp]
(
	[CacheQueueId] ASC
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
CREATE NONCLUSTERED INDEX [IX_CacheItem_MergeId] ON [CacheItem]
(
	[MergeId] ASC
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
CREATE NONCLUSTERED INDEX [IX_CacheSite_MergeId] ON [CacheSite]
(
	[MergeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
ALTER TABLE [CacheHtmlTemp] ADD  CONSTRAINT [DF__CacheHtmlTem__Id__286302EC]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [CacheHtmlTemp_CacheItemTemp] ADD  CONSTRAINT [DF_CacheHtmlTemp_CacheItemTemp_Id]  DEFAULT (newid()) FOR [Id]
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
ALTER TABLE [CacheHtmlTemp]  WITH NOCHECK ADD  CONSTRAINT [FK_CacheHtmlTemp_CacheQueue] FOREIGN KEY([CacheQueueId])
REFERENCES [CacheQueue] ([Id])
GO
ALTER TABLE [CacheHtmlTemp] NOCHECK CONSTRAINT [FK_CacheHtmlTemp_CacheQueue]
GO
ALTER TABLE [CacheHtmlTemp]  WITH NOCHECK ADD  CONSTRAINT [FK_CacheHtmlTemp_CacheSiteTemp] FOREIGN KEY([CacheSiteTempId])
REFERENCES [CacheSiteTemp] ([Id])
GO
ALTER TABLE [CacheHtmlTemp] NOCHECK CONSTRAINT [FK_CacheHtmlTemp_CacheSiteTemp]
GO
ALTER TABLE [CacheHtmlTemp_CacheItemTemp]  WITH NOCHECK ADD  CONSTRAINT [FK_CacheHtmlTemp_CacheItemTemp_CacheHtmlTemp] FOREIGN KEY([CacheHtmlTempId])
REFERENCES [CacheHtmlTemp] ([Id])
GO
ALTER TABLE [CacheHtmlTemp_CacheItemTemp] NOCHECK CONSTRAINT [FK_CacheHtmlTemp_CacheItemTemp_CacheHtmlTemp]
GO
ALTER TABLE [CacheHtmlTemp_CacheItemTemp]  WITH NOCHECK ADD  CONSTRAINT [FK_CacheHtmlTemp_CacheItemTemp_CacheItemTemp] FOREIGN KEY([CacheItemTempId])
REFERENCES [CacheItemTemp] ([Id])
GO
ALTER TABLE [CacheHtmlTemp_CacheItemTemp] NOCHECK CONSTRAINT [FK_CacheHtmlTemp_CacheItemTemp_CacheItemTemp]
GO
ALTER TABLE [CacheHtmlTemp_CacheItemTemp]  WITH NOCHECK ADD  CONSTRAINT [FK_CacheHtmlTemp_CacheItemTemp_CacheQueue] FOREIGN KEY([CacheQueueId])
REFERENCES [CacheQueue] ([Id])
GO
ALTER TABLE [CacheHtmlTemp_CacheItemTemp] NOCHECK CONSTRAINT [FK_CacheHtmlTemp_CacheItemTemp_CacheQueue]
GO
ALTER TABLE [CacheItemTemp]  WITH NOCHECK ADD  CONSTRAINT [FK_CacheItemTemp_CacheQueue] FOREIGN KEY([CacheQueueId])
REFERENCES [CacheQueue] ([Id])
GO
ALTER TABLE [CacheItemTemp] NOCHECK CONSTRAINT [FK_CacheItemTemp_CacheQueue]
GO
ALTER TABLE [CacheQueue]  WITH NOCHECK ADD  CONSTRAINT [FK_CacheQueue_CacheQueueMessageType] FOREIGN KEY([CacheQueueMessageTypeId])
REFERENCES [CacheQueueMessageType] ([Id])
GO
ALTER TABLE [CacheQueue] NOCHECK CONSTRAINT [FK_CacheQueue_CacheQueueMessageType]
GO
ALTER TABLE [CacheSiteTemp]  WITH NOCHECK ADD  CONSTRAINT [FK_CacheSiteTemp_CacheQueue] FOREIGN KEY([CacheQueueId])
REFERENCES [CacheQueue] ([Id])
GO
ALTER TABLE [CacheSiteTemp] NOCHECK CONSTRAINT [FK_CacheSiteTemp_CacheQueue]
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
	DELETE FROM CacheHtmlTemp_CacheItemTemp
	DELETE FROM CacheHtmlTemp
	DELETE FROM CacheItemTemp
	DELETE FROM CacheSiteTemp
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

	SET @BlockFurtherProcessing = (SELECT COUNT(Id) FROM CacheQueue Where Processing = 1);

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

				DELETE FROM CacheHtmlTemp_CacheItemTemp WHERE CacheQueueId = @CacheQueueId
				DELETE FROM CacheHtmlTemp WHERE CacheQueueId = @CacheQueueId
				DELETE FROM CacheItemTemp WHERE CacheQueueId = @CacheQueueId
				DELETE FROM CacheSiteTemp WHERE CacheQueueId = @CacheQueueId
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
	
CREATE Table #CacheSiteMergeView(IdLong bigint, IdGuid uniqueIdentifier);
CREATE CLUSTERED INDEX IX_CacheSiteMergeView_IdLong ON #CacheSiteMergeView (IdLong);
CREATE NONCLUSTERED INDEX IX_CacheSiteMergeView_IdGuid ON #CacheSiteMergeView (IdGuid);

UPDATE cs
SET cs.MergeId = cst.CacheQueueId
FROM CacheSiteTemp cst
INNER JOIN CacheSite cs ON cs.SiteName = cst.SiteName AND cs.SiteLang = cst.SiteLang
WHERE cs.MergeId <> @CacheQueueId
AND cst.CacheQueueId = @CacheQueueId

INSERT INTO CacheSite (MergeId, SiteName, SiteLang)
SELECT @CacheQueueId, cst.SiteName, cst.SiteLang
FROM CacheSiteTemp cst
LEFT JOIN CacheSite cs ON cs.SiteName = cst.SiteName AND cs.SiteLang = cst.SiteLang
WHERE cs.Id IS NULL
AND cst.CacheQueueId = @CacheQueueId

INSERT INTO #CacheSiteMergeView (IdLong, IdGuid)
SELECT cs.Id, cst.Id
FROM CacheSite cs
INNER JOIN CacheSiteTemp cst ON cs.MergeId = cst.CacheQueueId AND cs.SiteName = cst.SiteName AND cs.SiteLang = cst.SiteLang

-- End CacheSite Merge

-- Start CacheHtml Merge

CREATE Table #CacheHtmlMergeView(IdLong bigint, IdGuid uniqueIdentifier);
CREATE CLUSTERED INDEX IX_CacheHtmlMergeView_IdLong ON #CacheHtmlMergeView (IdLong);
CREATE NONCLUSTERED INDEX IX_CacheHtmlMergeView_IdGuid ON #CacheHtmlMergeView (IdGuid);

UPDATE ch
SET ch.MergeId = cht.CacheQueueId
FROM CacheHtmlTemp cht
INNER JOIN #CacheSiteMergeView csmv ON cht.CacheSiteTempId = csmv.IdGuid
INNER JOIN CacheHtml ch ON csmv.IdLong = ch.CacheSiteId AND ch.HtmlCacheKeyHash = cht.HtmlCacheKeyHash
WHERE ch.MergeId <> @CacheQueueId
AND cht.CacheQueueId = @CacheQueueId

INSERT INTO CacheHtml (MergeId, CacheSiteId, HtmlCacheKey, HtmlCacheResult, HtmlCacheKeyHash)
SELECT @CacheQueueId, csmv.IdLong, cht.HtmlCacheKey, cht.HtmlCacheResult, cht.HtmlCacheKeyHash
FROM CacheHtmlTemp cht
INNER JOIN #CacheSiteMergeView csmv ON cht.CacheSiteTempId = csmv.IdGuid
LEFT JOIN CacheHtml ch ON csmv.IdLong = ch.CacheSiteId AND ch.HtmlCacheKeyHash = cht.HtmlCacheKeyHash
WHERE ch.Id IS NULL
AND cht.CacheQueueId = @CacheQueueId

INSERT INTO #CacheHtmlMergeView (IdLong, IdGuid)
SELECT ch.Id, cht.Id
FROM CacheHtml ch
INNER JOIN CacheHtmlTemp cht ON ch.MergeId = cht.CacheQueueId AND ch.HtmlCacheKeyHash = cht.HtmlCacheKeyHash

-- End CacheHtml Merge

-- Start CacheItem Merge

CREATE Table #CacheItemMergeView(IdLong bigint, IdGuid uniqueIdentifier);
CREATE CLUSTERED INDEX IX_CacheItemMergeView_IdLong ON #CacheItemMergeView (IdLong);
CREATE NONCLUSTERED INDEX IX_CacheItemMergeView_IdGuid ON #CacheItemMergeView (IdGuid);

UPDATE ci
SET ci.MergeId = cit.CacheQueueId
FROM CacheItemTemp cit
INNER JOIN CacheItem ci ON cit.ItemId = ci.ItemId AND cit.ItemLang = ci.ItemLang
WHERE ci.MergeId <> @CacheQueueId
AND cit.CacheQueueId = @CacheQueueId

INSERT INTO CacheItem (MergeId, ItemId, ItemLang)
SELECT @CacheQueueId, cit.ItemId, cit.ItemLang
FROM CacheItemTemp cit
LEFT JOIN CacheItem ci ON cit.ItemId = ci.ItemId AND cit.ItemLang = ci.ItemLang
WHERE ci.Id IS NULL
AND cit.CacheQueueId = @CacheQueueId

INSERT INTO #CacheItemMergeView (IdLong, IdGuid)
SELECT ci.Id, cit.Id
FROM CacheItem ci
INNER JOIN CacheItemTemp cit ON ci.MergeId = cit.CacheQueueId AND ci.ItemId = cit.ItemId AND ci.ItemLang = cit.ItemLang

-- End CacheItem Merge

-- Start CacheHtml_CacheItem Merge

UPDATE chci
SET chci.MergeId = chcit.CacheQueueId
FROM CacheHtmlTemp_CacheItemTemp chcit
INNER JOIN #CacheHtmlMergeView chmv ON chcit.CacheHtmlTempId = chmv.IdGuid
INNER JOIN #CacheItemMergeView cimv ON chcit.CacheItemTempId = cimv.IdGuid
INNER JOIN CacheHtml_CacheItem chci ON chmv.IdLong = chci.CacheHtmlId AND cimv.IdLong = chci.CacheItemId
WHERE chci.MergeId <> @CacheQueueId
AND chcit.CacheQueueId = @CacheQueueId

INSERT INTO CacheHtml_CacheItem (MergeId, CacheHtmlId, CacheItemId)
SELECT DISTINCT @CacheQueueId, chmv.IdLong, cimv.IdLong
FROM CacheHtmlTemp_CacheItemTemp chcit
INNER JOIN #CacheHtmlMergeView chmv ON chcit.CacheHtmlTempId = chmv.IdGuid
INNER JOIN #CacheItemMergeView cimv ON chcit.CacheItemTempId = cimv.IdGuid
LEFT JOIN CacheHtml_CacheItem chci ON chmv.IdLong = chci.CacheHtmlId AND cimv.IdLong = chci.CacheItemId
WHERE chci.Id IS NULL
AND chcit.CacheQueueId = @CacheQueueId

-- End CacheHtml_CacheItem Merge
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

	DELETE chtcit FROM CacheHtmlTemp_CacheItemTemp chtcit
	INNER JOIN #CacheQueuesToClear r on r.CacheQueueId = chtcit.CacheQueueId AND r.CacheQueueId <> @CacheQueueId
	
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
	@CacheSite_TVP CacheSite_TVP READONLY,
	@CacheHtml_TVP CacheHtml_TVP READONLY,
	@CacheHtml_CacheItem_TVP CacheHtml_CacheItem_TVP READONLY,
	@CacheItem_TVP CacheItem_TVP READONLY
AS
BEGIN
	
	DECLARE @CacheQueueId BIGINT

	INSERT INTO CacheQueue(CacheQueueMessageTypeId, Processing)
	VALUES(1 ,0)

	SET @CacheQueueId = SCOPE_IDENTITY()
	
	INSERT INTO CacheSiteTemp (Id, CacheQueueId, SiteName, SiteLang)
	SELECT tvp.Id, @CacheQueueId, tvp.SiteName, tvp.SiteLang
	FROM @CacheSite_TVP tvp

	INSERT INTO CacheHtmlTemp (Id, CacheQueueId, CacheSiteTempId, HtmlCacheKey, HtmlCacheResult, HtmlCacheKeyHash)
	SELECT tvp.Id, @CacheQueueId, tvp.CacheSiteId, tvp.HtmlCacheKey, tvp.HtmlCacheResult, tvp.HtmlCacheKeyHash
	FROM @CacheHtml_TVP tvp

	INSERT INTO CacheItemTemp (Id, CacheQueueId, ItemId, ItemLang, IsDeleted)
	SELECT tvp.Id, @CacheQueueId, tvp.ItemId, tvp.ItemLang, tvp.IsDeleted 
	FROM @CacheItem_TVP tvp

	INSERT INTO CacheHtmlTemp_CacheItemTemp (Id, CacheQueueId, CacheHtmlTempId, CacheItemTempId)
	SELECT tvp.Id, @CacheQueueId, tvp.CacheHtmlId, tvp.CacheItemId
	FROM @CacheHtml_CacheItem_TVP tvp
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
	@CacheItem_TVP CacheItem_TVP READONLY
AS
BEGIN
	DECLARE @CacheQueueId BIGINT

	INSERT INTO CacheQueue(CacheQueueMessageTypeId, Processing)
	VALUES(2 ,0)

	SET @CacheQueueId = SCOPE_IDENTITY()

	INSERT INTO CacheItemTemp(CacheQueueId, ItemId, ItemLang, IsDeleted)
	SELECT @CacheQueueId, ItemId, ItemLang, IsDeleted FROM @CacheItem_TVP

	INSERT INTO CacheSiteTemp(CacheQueueId, SiteLang)
	SELECT @CacheQueueId, @SiteLang
END
GO
USE [master]
GO
ALTER DATABASE [HtmlCache] SET  READ_WRITE 
GO
