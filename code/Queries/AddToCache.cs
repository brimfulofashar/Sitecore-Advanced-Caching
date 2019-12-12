namespace Foundation.HtmlCache.Queries
{
    public class AddToCache
    {
        public static string GetCreateTempTableQuery(string suffix)
        {
            return string.Format(@"
            CREATE TABLE [dbo].[CacheItems{0}] (
              [Id] [uniqueidentifier] NOT NULL,
              [ItemId] [uniqueidentifier] NOT NULL,
              [CacheKey_Id] [uniqueidentifier] NOT NULL,
              CONSTRAINT [PK_CacheItems{0}] PRIMARY KEY CLUSTERED
              (
              [Id] ASC
              ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]
            CREATE TABLE [dbo].[CacheKeys{0}] (
              [Id] [uniqueidentifier] NOT NULL,
              [SiteName] [varchar](250) NOT NULL,
              [SiteLang] [varchar](250) NOT NULL,
              [HtmlCacheKey] [varchar](500) NOT NULL,
              [HtmlCacheResult] [varchar](MAX) NOT NULL,
              CONSTRAINT [PK_CacheKeys{0}] PRIMARY KEY CLUSTERED
              (
              [Id] ASC
              ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
            CREATE TABLE [dbo].[CacheKeysItems{0}] (
              [Id] [uniqueidentifier] NOT NULL,
              [CacheKey_Id] [uniqueidentifier] NOT NULL,
              [CacheItem_Id] [uniqueidentifier] NOT NULL,
              CONSTRAINT [PK_CacheKeysItems{0}] PRIMARY KEY CLUSTERED
              (
              [Id] ASC
              ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]
            CREATE UNIQUE NONCLUSTERED INDEX [IX_CacheItems{0}] ON [dbo].[CacheItems{0}]
            (
            [ItemId] ASC,
            [CacheKey_Id] ASC
            ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            CREATE UNIQUE NONCLUSTERED INDEX [IX_CacheKeys{0}] ON [dbo].[CacheKeys{0}]
            (
            [HtmlCacheKey] ASC
            ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            CREATE UNIQUE NONCLUSTERED INDEX [IX_CacheKeysItems{0}] ON [dbo].[CacheKeysItems{0}]
            (
            [CacheKey_Id] ASC,
            [CacheItem_Id] ASC
            ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = ON, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ALTER TABLE [dbo].[CacheItems{0}]  WITH CHECK ADD  CONSTRAINT [FK_CacheItems_CacheKeys{0}] FOREIGN KEY([CacheKey_Id]) REFERENCES [dbo].[CacheKeys{0}] ([Id]) ON DELETE CASCADE
			ALTER TABLE [dbo].[CacheItems{0}] CHECK CONSTRAINT [FK_CacheItems_CacheKeys{0}]
            ALTER TABLE [dbo].[CacheKeysItems{0}] WITH CHECK ADD CONSTRAINT [FK_CacheKeysItems_CacheItems{0}] FOREIGN KEY([CacheItem_Id]) REFERENCES [dbo].[CacheItems{0}] ([Id])
			ALTER TABLE [dbo].[CacheKeysItems{0}] CHECK CONSTRAINT [FK_CacheKeysItems_CacheItems{0}]
			ALTER TABLE [dbo].[CacheKeysItems{0}] WITH CHECK ADD CONSTRAINT [FK_CacheKeysItems_CacheKeys{0}] FOREIGN KEY([CacheKey_Id]) REFERENCES [dbo].[CacheKeys{0}] ([Id]) ON DELETE CASCADE
			ALTER TABLE [dbo].[CacheKeysItems{0}] CHECK CONSTRAINT [FK_CacheKeysItems_CacheKeys{0}]", suffix);
        }

        public string GetMergeTempTableQuery(string suffix)
        {
            return string.Format(@"
            merge into CacheKeys WITH (HOLDLOCK) as T 
            using CacheKeys_{0} as S 
            on (T.HtmlCacheKey = S.HtmlCacheKey) 
            when matched 
            then update set T.HtmlCacheKey = S.HtmlCacheKey, T.HtmlCacheResult = S.HtmlCacheResult, T.SiteName = S.SiteName, T.SiteLang = S.SiteLang
            when not matched 
            then insert (Id, HtmlCacheKey, HtmlCacheResult, SiteName, SiteLang) values (S.Id, S.HtmlCacheKey, S.HtmlCacheResult, S.SiteName, S.SiteLang);
            
            merge into CacheItems WITH (HOLDLOCK) as T 
            using CacheItems_{0} as S 
            on (T.ItemId = S.ItemId) 
            when matched 
            then update set T.ItemId = S.ItemId
            when not matched 
            then insert (Id, ItemId) values (S.Id, S.ItemId);
            
            merge into CacheKeysItems WITH (HOLDLOCK) as T 
            using (SELECT ckiTemp.Id, ck.Id as CacheKey_Id, ci.Id as CacheItem_Id
            FROM CacheKeys ck
            INNER JOIN CacheKeys_{0} ckTemp on ck.HtmlCacheKey = ckTemp.HtmlCacheKey
            INNER JOIN CacheKeysItems_{0} ckiTemp on ckTemp.Id = ckiTemp.CacheKey_Id
            INNER JOIN CacheItems_{0} ciTemp on ciTemp.Id = ckiTemp.CacheItem_Id
            INNER JOIN CacheItems ci on ciTemp.ItemId = ci.ItemId) as S 
            on (T.CacheKey_Id = S.CacheKey_Id AND T.CacheItem_Id = S.CacheItem_Id) 
            when matched 
            then update set T.CacheItem_Id = S.CacheItem_Id, T.CacheKey_Id = S.CacheKey_Id
            when not matched 
            then insert (Id, CacheKey_Id, CacheItem_Id) values (S.Id, S.CacheKey_Id, S.CacheItem_Id);", suffix);
        }
    }
}