USE [master]
GO
CREATE DATABASE [HtmlCache]
 CONTAINMENT = PARTIAL
 ON  PRIMARY 
( NAME = N'habitat.dev_Cache', FILENAME = N'D:\MSSQL13.MSSQLSERVER\MSSQL\DATA\habitat.dev_Cache.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'habitat.dev_Cache_log', FILENAME = N'D:\MSSQL13.MSSQLSERVER\MSSQL\Logs\habitat.dev_Cache_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [HtmlCache] SET COMPATIBILITY_LEVEL = 130
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
CREATE TABLE [CacheItems](
	[Id] [uniqueidentifier] NOT NULL,
	[ItemId] [uniqueidentifier] NOT NULL,
	[CacheKey_Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CacheItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheItemsTemp](
	[Id] [uniqueidentifier] NOT NULL,
	[CacheQueue_Id] [bigint] NULL,
	[ItemId] [uniqueidentifier] NOT NULL,
	[CacheKey_Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CacheItemsTemp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheKeys](
	[Id] [uniqueidentifier] NOT NULL,
	[CacheSiteLang_Id] [uniqueidentifier] NULL,
	[HtmlCacheKey] [varchar](500) NOT NULL,
	[HtmlCacheResult] [varchar](max) NOT NULL,
 CONSTRAINT [PK_CacheKeys] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheKeysItems](
	[Id] [uniqueidentifier] NOT NULL,
	[CacheKey_Id] [uniqueidentifier] NOT NULL,
	[CacheItem_Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CacheKeysItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheKeysItemsTemp](
	[Id] [uniqueidentifier] NOT NULL,
	[CacheQueue_Id] [bigint] NULL,
	[CacheKey_Id] [uniqueidentifier] NOT NULL,
	[CacheItem_Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CacheKeysItemsTemp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheKeysTemp](
	[Id] [uniqueidentifier] NOT NULL,
	[CacheQueue_Id] [bigint] NOT NULL,
	[CacheSiteLang_Id] [uniqueidentifier] NULL,
	[HtmlCacheKey] [varchar](500) NOT NULL,
	[HtmlCacheResult] [varchar](max) NOT NULL,
 CONSTRAINT [PK_CacheKeysTemp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheQueue](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CacheQueueMessageType_Id] [int] NOT NULL,
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
CREATE TABLE [CacheQueueBlocker](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BlockingMode] [bit] NOT NULL,
	[CacheQueue_Id] [bigint] NULL,
	[UpdateVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_CacheQueueBlocker] PRIMARY KEY CLUSTERED 
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
CREATE TABLE [CacheSiteLang](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[Lang] [varchar](250) NOT NULL,
 CONSTRAINT [PK_CacheSiteLang] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CacheSiteLangTemp](
	[Id] [uniqueidentifier] NOT NULL,
	[CacheQueue_Id] [bigint] NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[Lang] [varchar](250) NOT NULL,
 CONSTRAINT [PK_CacheSiteLangTemp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [PublishedItems](
	[CacheQueue_Id] [bigint] NOT NULL,
	[ItemId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PublishedItems] PRIMARY KEY CLUSTERED 
(
	[CacheQueue_Id] ASC,
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [CacheQueueBlocker] ON 
GO
INSERT [CacheQueueBlocker] ([Id], [BlockingMode], [CacheQueue_Id]) VALUES (1, 0, NULL)
GO
SET IDENTITY_INSERT [CacheQueueBlocker] OFF
GO
SET IDENTITY_INSERT [CacheQueueMessageType] ON 
GO
INSERT [CacheQueueMessageType] ([Id], [MessageType]) VALUES (1, N'AddToCache')
GO
INSERT [CacheQueueMessageType] ([Id], [MessageType]) VALUES (2, N'DeleteFromCache')
GO
INSERT [CacheQueueMessageType] ([Id], [MessageType]) VALUES (3, N'DeleteSiteFromCache')
GO
SET IDENTITY_INSERT [CacheQueueMessageType] OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_CacheItems] ON [CacheItems]
(
	[ItemId] ASC,
	[CacheKey_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_CacheKeys] ON [CacheKeys]
(
	[HtmlCacheKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_CacheKeysItems] ON [CacheKeysItems]
(
	[CacheKey_Id] ASC,
	[CacheItem_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_CacheSiteLangTemp] ON [CacheSiteLangTemp]
(
	[Name] ASC,
	[Lang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [CacheItems]  WITH CHECK ADD  CONSTRAINT [FK_CacheItems_CacheKeys] FOREIGN KEY([CacheKey_Id])
REFERENCES [CacheKeys] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [CacheItems] CHECK CONSTRAINT [FK_CacheItems_CacheKeys]
GO
ALTER TABLE [CacheItemsTemp]  WITH CHECK ADD  CONSTRAINT [FK_CacheItemsTemp_CacheKeysTemp] FOREIGN KEY([CacheKey_Id])
REFERENCES [CacheKeysTemp] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [CacheItemsTemp] CHECK CONSTRAINT [FK_CacheItemsTemp_CacheKeysTemp]
GO
ALTER TABLE [CacheItemsTemp]  WITH CHECK ADD  CONSTRAINT [FK_CacheItemsTemp_CacheQueue] FOREIGN KEY([CacheQueue_Id])
REFERENCES [CacheQueue] ([Id])
GO
ALTER TABLE [CacheItemsTemp] CHECK CONSTRAINT [FK_CacheItemsTemp_CacheQueue]
GO
ALTER TABLE [CacheKeys]  WITH CHECK ADD  CONSTRAINT [FK_CacheKeys_CacheSiteLang] FOREIGN KEY([CacheSiteLang_Id])
REFERENCES [CacheSiteLang] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [CacheKeys] CHECK CONSTRAINT [FK_CacheKeys_CacheSiteLang]
GO
ALTER TABLE [CacheKeysItems]  WITH CHECK ADD  CONSTRAINT [FK_CacheKeysItems_CacheItems] FOREIGN KEY([CacheItem_Id])
REFERENCES [CacheItems] ([Id])
GO
ALTER TABLE [CacheKeysItems] CHECK CONSTRAINT [FK_CacheKeysItems_CacheItems]
GO
ALTER TABLE [CacheKeysItems]  WITH CHECK ADD  CONSTRAINT [FK_CacheKeysItems_CacheKeys] FOREIGN KEY([CacheKey_Id])
REFERENCES [CacheKeys] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [CacheKeysItems] CHECK CONSTRAINT [FK_CacheKeysItems_CacheKeys]
GO
ALTER TABLE [CacheKeysItemsTemp]  WITH CHECK ADD  CONSTRAINT [FK_CacheKeysItemsTemp_CacheItemsTemp] FOREIGN KEY([CacheItem_Id])
REFERENCES [CacheItemsTemp] ([Id])
GO
ALTER TABLE [CacheKeysItemsTemp] CHECK CONSTRAINT [FK_CacheKeysItemsTemp_CacheItemsTemp]
GO
ALTER TABLE [CacheKeysItemsTemp]  WITH CHECK ADD  CONSTRAINT [FK_CacheKeysItemsTemp_CacheKeysTemp] FOREIGN KEY([CacheKey_Id])
REFERENCES [CacheKeysTemp] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [CacheKeysItemsTemp] CHECK CONSTRAINT [FK_CacheKeysItemsTemp_CacheKeysTemp]
GO
ALTER TABLE [CacheKeysItemsTemp]  WITH CHECK ADD  CONSTRAINT [FK_CacheKeysItemsTemp_CacheQueue] FOREIGN KEY([CacheQueue_Id])
REFERENCES [CacheQueue] ([Id])
GO
ALTER TABLE [CacheKeysItemsTemp] CHECK CONSTRAINT [FK_CacheKeysItemsTemp_CacheQueue]
GO
ALTER TABLE [CacheKeysTemp]  WITH CHECK ADD  CONSTRAINT [FK_CacheKeysTemp_CacheQueue] FOREIGN KEY([CacheQueue_Id])
REFERENCES [CacheQueue] ([Id])
GO
ALTER TABLE [CacheKeysTemp] CHECK CONSTRAINT [FK_CacheKeysTemp_CacheQueue]
GO
ALTER TABLE [CacheKeysTemp]  WITH CHECK ADD  CONSTRAINT [FK_CacheKeysTemp_CacheSiteLangTemp] FOREIGN KEY([CacheSiteLang_Id])
REFERENCES [CacheSiteLangTemp] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [CacheKeysTemp] CHECK CONSTRAINT [FK_CacheKeysTemp_CacheSiteLangTemp]
GO
ALTER TABLE [CacheQueue]  WITH CHECK ADD  CONSTRAINT [FK_CacheQueue_CacheQueueMessageType] FOREIGN KEY([CacheQueueMessageType_Id])
REFERENCES [CacheQueueMessageType] ([Id])
GO
ALTER TABLE [CacheQueue] CHECK CONSTRAINT [FK_CacheQueue_CacheQueueMessageType]
GO
ALTER TABLE [CacheQueueBlocker]  WITH CHECK ADD  CONSTRAINT [FK_CacheQueueBlocker_CacheQueue] FOREIGN KEY([CacheQueue_Id])
REFERENCES [CacheQueue] ([Id])
GO
ALTER TABLE [CacheQueueBlocker] CHECK CONSTRAINT [FK_CacheQueueBlocker_CacheQueue]
GO
ALTER TABLE [CacheSiteLangTemp]  WITH CHECK ADD  CONSTRAINT [FK_CacheSiteLangTemp_CacheQueue] FOREIGN KEY([CacheQueue_Id])
REFERENCES [CacheQueue] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [CacheSiteLangTemp] CHECK CONSTRAINT [FK_CacheSiteLangTemp_CacheQueue]
GO
ALTER TABLE [PublishedItems]  WITH CHECK ADD  CONSTRAINT [FK_PublishedItems_CacheQueue] FOREIGN KEY([CacheQueue_Id])
REFERENCES [CacheQueue] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [PublishedItems] CHECK CONSTRAINT [FK_PublishedItems_CacheQueue]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [ProcessQueue]
AS
BEGIN
Begin Tran T1

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
SELECT @PendingQueueLength as PendingQueueLength, @SiteName as SiteName, @SiteLang as SiteLang, @HtmlCacheKeys as DeletedCacheKeys
Commit Tran T1
END
GO
USE [master]
GO
ALTER DATABASE [HtmlCache] SET  READ_WRITE 
GO
