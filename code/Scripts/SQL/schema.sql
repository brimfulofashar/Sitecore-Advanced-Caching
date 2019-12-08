USE [master]
GO
/****** Object:  Database [HtmlCache]    Script Date: 08/12/2019 13:13:03 ******/
CREATE DATABASE [HtmlCache]
GO
ALTER DATABASE [HtmlCache] SET CONTAINMENT = PARTIAL
GO
USE [HtmlCache]
GO
/****** Object:  User [htmlcacheuser]    Script Date: 08/12/2019 15:41:23 ******/
CREATE USER [htmlcacheuser] WITH PASSWORD=N'kuqG0I+RD1uBAhZc0Us+Elryv7XDG0S+WhRqs15O0SU=', DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [htmlcacheuser]
GO
/****** Object:  Table [dbo].[CacheItems]    Script Date: 08/12/2019 15:41:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CacheItems](
	[Id] [uniqueidentifier] NOT NULL,
	[ItemId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CacheItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CacheKeys]    Script Date: 08/12/2019 15:41:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CacheKeys](
	[Id] [uniqueidentifier] NOT NULL,
	[SiteName] [varchar](250) NOT NULL,
	[SiteLang] [varchar](250) NOT NULL,
	[HtmlCacheKey] [varchar](500) NOT NULL,
	[HtmlCacheResult] [varchar](max) NOT NULL,
 CONSTRAINT [PK_CacheKeys] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CacheKeysItems]    Script Date: 08/12/2019 15:41:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CacheKeysItems](
	[Id] [uniqueidentifier] NOT NULL,
	[CacheKey_Id] [uniqueidentifier] NOT NULL,
	[CacheItem_Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CacheKeysItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CacheQueue]    Script Date: 08/12/2019 15:41:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CacheQueue](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CacheQueueMessageType_Id] [int] NOT NULL,
	[Suffix] [char](32) NOT NULL,
 CONSTRAINT [PK_CacheQueue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CacheQueueMessageType]    Script Date: 08/12/2019 15:41:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CacheQueueMessageType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MessageType] [varchar](100) NOT NULL,
 CONSTRAINT [PK_CacheQueueMessageType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PublishedItems]    Script Date: 08/12/2019 15:41:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PublishedItems](
	[ItemId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PublishedItems] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_CacheItems]    Script Date: 08/12/2019 15:41:23 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_CacheItems] ON [dbo].[CacheItems]
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_CacheKeys]    Script Date: 08/12/2019 15:41:23 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_CacheKeys] ON [dbo].[CacheKeys]
(
	[HtmlCacheKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CacheKeysItems]    Script Date: 08/12/2019 15:41:23 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_CacheKeysItems] ON [dbo].[CacheKeysItems]
(
	[CacheKey_Id] ASC,
	[CacheItem_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CacheKeysItems]  WITH CHECK ADD  CONSTRAINT [FK_CacheKeysItems_CacheItems] FOREIGN KEY([CacheItem_Id])
REFERENCES [dbo].[CacheItems] ([Id])
GO
ALTER TABLE [dbo].[CacheKeysItems] CHECK CONSTRAINT [FK_CacheKeysItems_CacheItems]
GO
ALTER TABLE [dbo].[CacheKeysItems]  WITH CHECK ADD  CONSTRAINT [FK_CacheKeysItems_CacheKeys] FOREIGN KEY([CacheKey_Id])
REFERENCES [dbo].[CacheKeys] ([Id])
GO
ALTER TABLE [dbo].[CacheKeysItems] CHECK CONSTRAINT [FK_CacheKeysItems_CacheKeys]
GO
ALTER TABLE [dbo].[CacheQueue]  WITH CHECK ADD  CONSTRAINT [FK_CacheQueue_CacheQueueMessageType] FOREIGN KEY([CacheQueueMessageType_Id])
REFERENCES [dbo].[CacheQueueMessageType] ([Id])
GO
ALTER TABLE [dbo].[CacheQueue] CHECK CONSTRAINT [FK_CacheQueue_CacheQueueMessageType]
GO
/****** Object:  StoredProcedure [dbo].[MergeQueuedTrackingData]    Script Date: 08/12/2019 15:41:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MergeQueuedTrackingData]-- @SuffixStr char(32)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Counter BIGINT
	SELECT @Counter = MIN(ID) FROM CacheQueue
	DECLARE @Suffix char(32)

    WHILE @Counter <= (SELECT MAX(ID) FROM CacheQueue)
    BEGIN
  
  		SELECT @Suffix = Suffix FROM CacheQueue with (rowlock updlock) WHERE Id = @Counter
  
  		DECLARE @MergeStatement as NVARCHAR(max)
		SET @MergeStatement = 'merge into CacheKeys WITH (HOLDLOCK) as T 
        using CacheKeys_' + @Suffix + ' as S 
        on (T.HtmlCacheKey = S.HtmlCacheKey) 
        when matched 
        then update set T.HtmlCacheKey = S.HtmlCacheKey, T.HtmlCacheResult = S.HtmlCacheResult, T.SiteName = S.SiteName, T.SiteLang = S.SiteLang
        when not matched 
        then insert (Id, HtmlCacheKey, HtmlCacheResult, SiteName, SiteLang) values (S.Id, S.HtmlCacheKey, S.HtmlCacheResult, S.SiteName, S.SiteLang);
            
        merge into CacheItems WITH (HOLDLOCK) as T 
        using CacheItems_' + @Suffix + '  as S 
        on (T.ItemId = S.ItemId) 
        when matched 
        then update set T.ItemId = S.ItemId
        when not matched 
        then insert (Id, ItemId) values (S.Id, S.ItemId);
            
        merge into CacheKeysItems WITH (HOLDLOCK) as T 
        using (SELECT ckiTemp.Id, ck.Id as CacheKey_Id, ci.Id as CacheItem_Id
        FROM CacheKeys ck
        INNER JOIN CacheKeys_' + @Suffix + '  ckTemp on ck.HtmlCacheKey = ckTemp.HtmlCacheKey
        INNER JOIN CacheKeysItems_' + @Suffix + '  ckiTemp on ckTemp.Id = ckiTemp.CacheKey_Id
        INNER JOIN CacheItems_' + @Suffix + '  ciTemp on ciTemp.Id = ckiTemp.CacheItem_Id
        INNER JOIN CacheItems ci on ciTemp.ItemId = ci.ItemId) as S 
        on (T.CacheKey_Id = S.CacheKey_Id AND T.CacheItem_Id = S.CacheItem_Id) 
        when matched 
        then update set T.CacheItem_Id = S.CacheItem_Id, T.CacheKey_Id = S.CacheKey_Id
        when not matched 
        then insert (Id, CacheKey_Id, CacheItem_Id) values (S.Id, S.CacheKey_Id, S.CacheItem_Id);
		
		DROP TABLE CacheKeysItems_' + @Suffix +';
		DROP TABLE CacheKeys_' + @Suffix +';
		DROP TABLE CacheItems_' + @Suffix +';'

		EXECUTE sp_executesql @MergeStatement;
		
		DELETE FROM CacheQueue WHERE ID = @Counter 
  
  		SET @Counter = @Counter + 1
	END
END
GO