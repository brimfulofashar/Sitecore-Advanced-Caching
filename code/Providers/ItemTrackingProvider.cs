using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
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

        public DbSet<CacheItem> CacheItems { get; set; } // CacheItems
        public DbSet<CacheKey> CacheKeys { get; set; } // CacheKeys
        public DbSet<CacheKeyItem> CacheKeysItems { get; set; } // CacheKeysItems
        public DbSet<CacheQueue> CacheQueues { get; set; } // CacheQueue
        public DbSet<CacheQueueMessageType> CacheQueueMessageTypes { get; set; } // CacheQueueMessageType
        public DbSet<PublishedItem> PublishedItems { get; set; } // PublishedItems

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

            //            builder.Configurations.Add(new EntityTypeConfiguration<CacheKey>());
            //            string tableName = "CacheKeys" + suffix;
            //            builder.Entity<CacheKey>().ToTable(tableName);
            //
            //            builder.Configurations.Add(new EntityTypeConfiguration<CacheItem>());
            //            tableName = "CacheItems" + suffix;
            //            builder.Entity<CacheItem>().ToTable(tableName);
            //
            //            builder.Configurations.Add(new EntityTypeConfiguration<CacheKeyItem>());
            //            tableName = "CacheKeysItems" + suffix;
            //            builder.Entity<CacheKeyItem>().ToTable(tableName);
            //            builder.Configurations.Add(new EntityTypeConfiguration<CacheQueue>());
            //            tableName = "CacheQueue";
            //            builder.Entity<CacheQueue>().ToTable(tableName);
            //
            //            builder.Configurations.Add(new EntityTypeConfiguration<PublishedItem>());
            //            tableName = "PublishedItems";
            //            builder.Entity<PublishedItem>().ToTable(tableName);

            builder.Configurations.Add(new CacheItemConfiguration(suffix));
            builder.Configurations.Add(new CacheKeyConfiguration(suffix));
            builder.Configurations.Add(new CacheKeysItemConfiguration(suffix));
            builder.Configurations.Add(new CacheQueueConfiguration());
            builder.Configurations.Add(new CacheQueueMessageTypeConfiguration());
            builder.Configurations.Add(new PublishedItemConfiguration());

            

            // Compile ORM object, hard link connection
            DbConnection dummyConnection = dummyContext.Database.Connection;
            DbCompiledModel compiledModel = builder.Build(dummyConnection).Compile();

            // Finally make our database context
            dummyContext = new ItemTrackingProvider(compiledModel, dummyContext.Database.Connection.ConnectionString);

            var tempSql = GetCreateTempTablesStatement(suffix);

            dummyContext.Database.ExecuteSqlCommand(tempSql);
            return dummyContext;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //            modelBuilder.Entity<CacheKeyItem>()
            //                .HasKey(x => new { x.CacheKey_Id, x.Id })
            //                .HasIndex(x => new { x.CacheKey_Id, x.CacheItem_Id });
            //
            //            modelBuilder.Entity<CacheKey>()
            //                .HasKey(x => x.Id)
            //                .HasMany(s => s.CacheItems)
            //                .WithMany(c => c.CacheKeys)
            //                .Map(ck =>
            //                {
            //                    ck.MapLeftKey("CacheKey_Id");
            //                    ck.MapRightKey("CacheItem_Id");
            //                    ck.ToTable("CacheKeysItems");
            //                });
            //
            //            modelBuilder.Entity<CacheKey>()
            //                .HasIndex(x => new { x.HtmlCacheKey });
            //
            //            modelBuilder.Entity<CacheKey>()
            //                .HasMany(x => x.CacheItems)
            //                .WithRequired(s => s.CacheKey)
            //                .HasForeignKey(s => s.CacheKey_Id);
            //
            //            modelBuilder.Entity<CacheItem>()
            //                .HasKey(x => x.Id)
            //                .HasMany(s => s.CacheKeys)
            //                .WithMany(c => c.CacheItems)
            //                .Map(ci =>
            //                {
            //                    ci.MapLeftKey("CacheItem_Id");
            //                    ci.MapRightKey("CacheKey_Id");
            //                    ci.ToTable("CacheKeysItems");
            //                });
            //            modelBuilder.Entity<CacheItem>().HasIndex(x => new { x.ItemId });
            //
            //            base.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new CacheItemConfiguration(""));
            modelBuilder.Configurations.Add(new CacheKeyConfiguration(""));
            modelBuilder.Configurations.Add(new CacheKeysItemConfiguration(""));
            modelBuilder.Configurations.Add(new CacheQueueConfiguration());
            modelBuilder.Configurations.Add(new CacheQueueMessageTypeConfiguration());
            modelBuilder.Configurations.Add(new PublishedItemConfiguration());

            // Indexes        
            modelBuilder.Entity<CacheItem>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheItems", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheKey>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheKeys", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheKeyItem>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheKeysItems", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheQueue>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheQueue", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheQueueMessageType>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheQueueMessageType", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<PublishedItem>()
                .Property(e => e.CacheQueueId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_PublishedItems", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<PublishedItem>()
                .Property(e => e.ItemId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_PublishedItems", 2) { IsUnique = true, IsClustered = true })
                );

        }

        public class CacheItemConfiguration : EntityTypeConfiguration<CacheItem>
        {
            public CacheItemConfiguration(string suffix): this("dbo", suffix)
            {
            }

            public CacheItemConfiguration(string schema, string suffix)
            {
                ToTable("CacheItems" + suffix, schema);
                HasKey(x => x.Id);

                Property(x => x.Id).HasColumnName(@"Id").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
                Property(x => x.ItemId).HasColumnName(@"ItemId").HasColumnType("uniqueidentifier").IsRequired();
                Property(x => x.CacheKeyId).HasColumnName(@"CacheKey_Id").HasColumnType("uniqueidentifier").IsRequired();

                // Foreign keys
                HasRequired(a => a.CacheKey).WithMany(b => b.CacheItems).HasForeignKey(c => c.CacheKeyId); // FK_CacheItems_CacheKeys
            }
        }

        // CacheKeys
        public class CacheKeyConfiguration : EntityTypeConfiguration<CacheKey>
        {
            public CacheKeyConfiguration(string suffix): this("dbo", suffix)
            {
            }

            public CacheKeyConfiguration(string schema, string suffix)
            {
                ToTable("CacheKeys" + suffix, schema);
                HasKey(x => x.Id);

                Property(x => x.Id).HasColumnName(@"Id").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
                Property(x => x.SiteName).HasColumnName(@"SiteName").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(250);
                Property(x => x.SiteLang).HasColumnName(@"SiteLang").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(250);
                Property(x => x.HtmlCacheKey).HasColumnName(@"HtmlCacheKey").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(500);
                Property(x => x.HtmlCacheResult).HasColumnName(@"HtmlCacheResult").HasColumnType("varchar(max)").IsRequired().IsUnicode(false);
            }
        }

        // CacheKeysItems
        public class CacheKeysItemConfiguration : EntityTypeConfiguration<CacheKeyItem>
        {
            public CacheKeysItemConfiguration(string suffix): this("dbo", suffix)
            {
            }

            public CacheKeysItemConfiguration(string schema, string suffix)
            {
                ToTable("CacheKeysItems" + suffix, schema);
                HasKey(x => x.Id);

                Property(x => x.Id).HasColumnName(@"Id").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
                Property(x => x.CacheKeyId).HasColumnName(@"CacheKey_Id").HasColumnType("uniqueidentifier").IsRequired();
                Property(x => x.CacheItemId).HasColumnName(@"CacheItem_Id").HasColumnType("uniqueidentifier").IsRequired();

                // Foreign keys
                HasRequired(a => a.CacheItem).WithMany(b => b.CacheKeysItems).HasForeignKey(c => c.CacheItemId).WillCascadeOnDelete(false); // FK_CacheKeysItems_CacheItems
                HasRequired(a => a.CacheKey).WithMany(b => b.CacheKeysItems).HasForeignKey(c => c.CacheKeyId); // FK_CacheKeysItems_CacheKeys
            }
        }

        // CacheQueue
        public class CacheQueueConfiguration : EntityTypeConfiguration<CacheQueue>
        {
            public CacheQueueConfiguration()
                : this("dbo")
            {
            }

            public CacheQueueConfiguration(string schema)
            {
                ToTable("CacheQueue", schema);
                HasKey(x => x.Id);

                Property(x => x.Id).HasColumnName(@"Id").HasColumnType("bigint").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                Property(x => x.CacheQueueMessageTypeId).HasColumnName(@"CacheQueueMessageType_Id").HasColumnType("int").IsRequired();
                Property(x => x.Suffix).HasColumnName(@"Suffix").HasColumnType("char").IsOptional().IsFixedLength().IsUnicode(false).HasMaxLength(32);

                // Foreign keys
                HasRequired(a => a.CacheQueueMessageType).WithMany(b => b.CacheQueues).HasForeignKey(c => c.CacheQueueMessageTypeId).WillCascadeOnDelete(false); // FK_CacheQueue_CacheQueueMessageType
            }
        }

        // CacheQueueMessageType
        public class CacheQueueMessageTypeConfiguration : EntityTypeConfiguration<CacheQueueMessageType>
        {
            public CacheQueueMessageTypeConfiguration()
                : this("dbo")
            {
            }

            public CacheQueueMessageTypeConfiguration(string schema)
            {
                ToTable("CacheQueueMessageType", schema);
                HasKey(x => x.Id);

                Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                Property(x => x.MessageType).HasColumnName(@"MessageType").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(100);
            }
        }

        // PublishedItems
        public class PublishedItemConfiguration : EntityTypeConfiguration<PublishedItem>
        {
            public PublishedItemConfiguration()
                : this("dbo")
            {
            }

            public PublishedItemConfiguration(string schema)
            {
                ToTable("PublishedItems", schema);
                HasKey(x => new { x.CacheQueueId, x.ItemId });

                Property(x => x.CacheQueueId).HasColumnName(@"CacheQueueId").HasColumnType("bigint").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
                Property(x => x.ItemId).HasColumnName(@"ItemId").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

                // Foreign keys
                HasRequired(a => a.CacheQueue).WithMany(b => b.PublishedItems).HasForeignKey(c => c.CacheQueueId).WillCascadeOnDelete(false); // FK_PublishedItems_CacheQueue
            }
        }



        private static string GetCreateTempTablesStatement(string suffix)
        {
            var tempTableScript = string.Format(@"
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

            return tempTableScript;
        }

        public static string GetDeleteFromCacheStatement()
        {
            var str = @"
            SET NOCOUNT ON;

	        DECLARE @Counter BIGINT
	        SELECT @Counter = MIN(ID) FROM CacheQueue
	        DECLARE @Suffix char(32)
	        DECLARE @DeleteFromCacheStatement as NVARCHAR(max)
	        DECLARE @MaxId BIGINT
	        SELECT @MaxId = MAX(ID) FROM CacheQueue
	        DECLARE @MessageId BIGINT
	        SELECT @MessageId = MIN(ID) FROM CacheQueue WHERE CacheQueueMessageType_Id = 2

	        SET @DeleteFromCacheStatement = '
		        DELETE FROM CacheKeys
		        WHERE HtmlCacheKey IN
		        (
		        SELECT ck.HtmlCacheKey
		        FROM CacheKeys ck          
		        INNER JOIN CacheKeysItems cki 
		        ON ck.Id = cki.CacheKey_Id 
		        INNER JOIN CacheItems ci
		        ON ci.Id = cki.CacheItem_Id
		        INNER JOIN PublishedItems pi 
		        ON ci.ItemId = pi.ItemId
		        INNER JOIN CacheQueue cq
		        ON pi.CacheQueueId = ' + CAST(@MessageId AS varchar(12)) +')'

            WHILE @Counter < @MaxId
            BEGIN
		        SELECT @Suffix = Suffix FROM CacheQueue with (rowlock updlock) WHERE Id = @Counter AND CacheQueueMessageType_Id = 1
          
  		        SET @DeleteFromCacheStatement =  CONCAT(@DeleteFromCacheStatement, '            
                DELETE FROM CacheKeys_' + @Suffix + '
		        WHERE HtmlCacheKey IN
		        (
		        SELECT ckTemp.HtmlCacheKey
		        FROM CacheKeys_' + @Suffix + ' ckTemp          
		        INNER JOIN CacheKeysItems_' + @Suffix + ' ckiTemp 
		        ON ckTemp.Id = ckiTemp.CacheKey_Id 
		        INNER JOIN CacheItems_' + @Suffix + ' ciTemp
		        ON ciTemp.Id = ckiTemp.CacheItem_Id
		        INNER JOIN PublishedItems pi 
		        ON ciTemp.ItemId = pi.ItemId
		        INNER JOIN CacheQueue cq
		        ON pi.CacheQueueId = ' + CAST(@MessageId AS varchar(12)) +')')

		        SET @Counter = @Counter + 1
	        END

	        SET @DeleteFromCacheStatement = CONCAT(@DeleteFromCacheStatement, '
	        DELETE FROM PublishedItems WHERE CacheQueueId = ' + CAST(@MessageId AS varchar(12)) +'')

	        SET @DeleteFromCacheStatement = CONCAT(@DeleteFromCacheStatement, '
	        DELETE FROM CacheQueue WHERE Id = ' + CAST(@MessageId AS varchar(12)) +'')

	        SELECT @DeleteFromCacheStatement";
            return str;
        }

        public string GetMergeStatement(string suffix)
        {
            return string.Format(@"
            merge into CacheKeys WITH (HOLDLOCK) as T 
            using CacheKeys_{0} as S 
            on (T.HtmlCacheKey = S.HtmlCacheKey) 
            --when matched 
            --then update set T.HtmlCacheKey = S.HtmlCacheKey, T.HtmlCacheResult = S.HtmlCacheResult, T.SiteName = S.SiteName, T.SiteLang = S.SiteLang
            when not matched 
            then insert (Id, HtmlCacheKey, HtmlCacheResult, SiteName, SiteLang) values (S.Id, S.HtmlCacheKey, S.HtmlCacheResult, S.SiteName, S.SiteLang);
            
            merge into CacheItems WITH (HOLDLOCK) as T 
            using CacheItems_{0} as S 
            on (T.ItemId = S.ItemId) 
            --when matched 
            --then update set T.ItemId = S.ItemId
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
            --when matched 
            --then update set T.CacheItem_Id = S.CacheItem_Id, T.CacheKey_Id = S.CacheKey_Id
            when not matched 
            then insert (Id, CacheKey_Id, CacheItem_Id) values (S.Id, S.CacheKey_Id, S.CacheItem_Id);", suffix);
        }

        public string GetDeleteTempTableStatement(string suffix)
        {
            return string.Format(@"
            DROP TABLE CacheKeysItems_{0};
            DROP TABLE CacheItems_{0};
            DROP TABLE CacheKeys_{0};", suffix);
        }
    }
}