using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using Foundation.HtmlCache.Models;

namespace Foundation.HtmlCache.Providers
{
    public class ItemTrackingProvider : DbContext
    {
        public Database _Database { get; }
        public ItemTrackingProvider() : base("HtmlCache")
        {
            this._Database = Database;
            Database.SetInitializer<ItemTrackingProvider>(null);//Disable initializer
        }

        public DbSet<CacheKey> CacheKeys { get; set; }
        public DbSet<CacheItem> CacheItems { get; set; }
        public DbSet<CacheKeyItem> CacheKeyItems { get; set; }

        private ItemTrackingProvider(DbCompiledModel compiledModel, string connectionString): base(compiledModel)
        {
            Database.SetInitializer<ItemTrackingProvider>(null);
            Database.Connection.ConnectionString = connectionString;
            this._Database = Database;
        }

        public static ItemTrackingProvider CreateDummyContext(string suffix)
        {
            suffix = "_" + suffix;
            ItemTrackingProvider dummyContext = new ItemTrackingProvider();

            DbModelBuilder builder = new DbModelBuilder(DbModelBuilderVersion.Latest);

            builder.Configurations.Add(new EntityTypeConfiguration<CacheItem>());
            string tableName = "CacheItems" + suffix;
            builder.Entity<CacheItem>().ToTable(tableName);

            builder.Configurations.Add(new EntityTypeConfiguration<CacheKey>());
            tableName = "CacheKeys" + suffix;
            builder.Entity<CacheKey>().ToTable(tableName);
            
            builder.Configurations.Add(new EntityTypeConfiguration<CacheKeyItem>());
            tableName = "CacheKeysItems" + suffix;
            builder.Entity<CacheKeyItem>().ToTable(tableName);

            // Compile ORM object, hard link connection
            DbConnection dummyConnection = dummyContext.Database.Connection;
            DbCompiledModel compiledModel = builder.Build(dummyConnection).Compile();

            // Finally make our database context
            dummyContext = new ItemTrackingProvider(compiledModel, dummyContext.Database.Connection.ConnectionString);

            var tempSql = CreateTempTables(suffix);

            dummyContext.Database.ExecuteSqlCommand(tempSql);

            return dummyContext;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CacheKeyItem>()
                .HasKey(x => new { x.CacheKey_Id, x.Id })
                .HasIndex(x => new { x.CacheKey_Id, x.CacheItem_Id });

            modelBuilder.Entity<CacheKey>()
                .HasKey(x => x.Id)
                .HasMany(s => s.CacheItems)
                .WithMany(c => c.CacheKeys)
                .Map(ck =>
                {
                    ck.MapLeftKey("CacheKey_Id");
                    ck.MapRightKey("CacheItem_ItemId");
                    ck.ToTable("CacheKeysItems");
                });
            modelBuilder.Entity<CacheKey>().HasIndex(x => new { x.HtmlCacheKey });

            modelBuilder.Entity<CacheItem>()
                .HasKey(x => x.Id)
                .HasMany(s => s.CacheKeys)
                .WithMany(c => c.CacheItems)
                .Map(ci =>
                {
                    ci.MapLeftKey("CacheItem_ItemId");
                    ci.MapRightKey("CacheKey_Id");
                    ci.ToTable("CacheKeysItems");
                });
            modelBuilder.Entity<CacheItem>().HasIndex(x => new { x.ItemId });

            base.OnModelCreating(modelBuilder);
        }

        private static string CreateTempTables(string suffix)
        {
            var createScript = string.Format(@"
            CREATE TABLE [dbo].[CacheItems{0}] (
              [Id] [uniqueidentifier] NOT NULL,
              [ItemId] [uniqueidentifier] NOT NULL,
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
            [ItemId] ASC
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
            ALTER TABLE [dbo].[CacheKeysItems{0}] WITH CHECK ADD CONSTRAINT [FK_CacheKeysItems_CacheItems{0}] FOREIGN KEY ([CacheItem_Id]) REFERENCES [dbo].[CacheItems{0}] ([Id])
            ALTER TABLE [dbo].[CacheKeysItems{0}] CHECK CONSTRAINT [FK_CacheKeysItems_CacheItems{0}]
            ALTER TABLE [dbo].[CacheKeysItems{0}] WITH CHECK ADD CONSTRAINT [FK_CacheKeysItems_CacheKeys{0}] FOREIGN KEY ([CacheKey_Id]) REFERENCES [dbo].[CacheKeys{0}] ([Id])
            ALTER TABLE [dbo].[CacheKeysItems{0}] CHECK CONSTRAINT [FK_CacheKeysItems_CacheKeys{0}]", suffix);

            return createScript;
        }
    }
}