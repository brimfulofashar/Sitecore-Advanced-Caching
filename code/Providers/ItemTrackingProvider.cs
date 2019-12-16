using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Foundation.HtmlCache.Models;
using Foundation.HtmlCache.Queries;

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
        public DbSet<CacheItemTemp> CacheItemTemps { get; set; } // CacheItems
        public DbSet<CacheKeyTemp> CacheKeyTemps { get; set; } // CacheKeys
        public DbSet<CacheKeyItemTemp> CacheKeysItemsTemps { get; set; } // CacheKeysItems

        public DbSet<CacheQueue> CacheQueues { get; set; } // CacheQueue
        public DbSet<CacheQueueMessageType> CacheQueueMessageTypes { get; set; } // CacheQueueMessageType
        public DbSet<PublishedItem> PublishedItems { get; set; } // PublishedItems

        private ItemTrackingProvider(DbCompiledModel compiledModel, string connectionString): base(compiledModel)
        {
            Database.SetInitializer<ItemTrackingProvider>(null);
            Database.Connection.ConnectionString = connectionString;
            this._Database = Database;
        }

//        public static ItemTrackingProvider CreateDummyContext()
//        {
//            ItemTrackingProvider dummyContext = new ItemTrackingProvider();
//
//            DbModelBuilder builder = new DbModelBuilder(DbModelBuilderVersion.Latest);
//
//            builder.Configurations.Add(new CacheItemConfiguration());
//            builder.Configurations.Add(new CacheItemsTempConfiguration());
//            builder.Configurations.Add(new CacheKeyConfiguration());
//            builder.Configurations.Add(new CacheKeysItemConfiguration());
//            builder.Configurations.Add(new CacheKeysItemsTempConfiguration());
//            builder.Configurations.Add(new CacheKeysTempConfiguration());
//            builder.Configurations.Add(new CacheQueueConfiguration());
//            builder.Configurations.Add(new CacheQueueMessageTypeConfiguration());
//            builder.Configurations.Add(new PublishedItemConfiguration());
//
//
//
//            // Compile ORM object, hard link connection
//            DbConnection dummyConnection = dummyContext.Database.Connection;
//            DbCompiledModel compiledModel = builder.Build(dummyConnection).Compile();
//
//            // Finally make our database context
//            dummyContext = new ItemTrackingProvider(compiledModel, dummyContext.Database.Connection.ConnectionString);
//
////            var tempSql = AddToCache.GetCreateTempTableQuery(suffix);
//
////            dummyContext.Database.ExecuteSqlCommand(tempSql);
//            return dummyContext;
//        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new CacheItemConfiguration());
            modelBuilder.Configurations.Add(new CacheItemsTempConfiguration());
            modelBuilder.Configurations.Add(new CacheKeyConfiguration());
            modelBuilder.Configurations.Add(new CacheKeysItemConfiguration());
            modelBuilder.Configurations.Add(new CacheKeysItemsTempConfiguration());
            modelBuilder.Configurations.Add(new CacheKeysTempConfiguration());
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


            modelBuilder.Entity<CacheItemTemp>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheItemsTemp", 1) { IsUnique = true, IsClustered = true })
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


            modelBuilder.Entity<CacheKeyItemTemp>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheKeysItemsTemp", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheKeyTemp>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheKeysTemp", 1) { IsUnique = true, IsClustered = true })
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
            public CacheItemConfiguration()
                : this("dbo")
            {
            }

            public CacheItemConfiguration(string schema)
            {
                ToTable("CacheItems", schema);
                HasKey(x => x.Id);

                Property(x => x.Id).HasColumnName(@"Id").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
                Property(x => x.ItemId).HasColumnName(@"ItemId").HasColumnType("uniqueidentifier").IsRequired();
                Property(x => x.CacheKeyId).HasColumnName(@"CacheKey_Id").HasColumnType("uniqueidentifier").IsRequired();

                // Foreign keys
                HasRequired(a => a.CacheKey).WithMany(b => b.CacheItems).HasForeignKey(c => c.CacheKeyId); // FK_CacheItems_CacheKeys
            }
        }

        // CacheItemsTemp
        public class CacheItemsTempConfiguration : EntityTypeConfiguration<CacheItemTemp>
        {
            public CacheItemsTempConfiguration()
                : this("dbo")
            {
            }

            public CacheItemsTempConfiguration(string schema)
            {
                ToTable("CacheItemsTemp", schema);
                HasKey(x => x.Id);

                Property(x => x.Id).HasColumnName(@"Id").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
                Property(x => x.ItemId).HasColumnName(@"ItemId").HasColumnType("uniqueidentifier").IsRequired();
                Property(x => x.CacheKeyId).HasColumnName(@"CacheKey_Id").HasColumnType("uniqueidentifier").IsRequired();

                // Foreign keys
                HasRequired(a => a.CacheKeysTemp).WithMany(b => b.CacheItemTemp).HasForeignKey(c => c.CacheKeyId); // FK_CacheItemsTemp_CacheKeysTemp
            }
        }

        // CacheKeys
        public class CacheKeyConfiguration : EntityTypeConfiguration<CacheKey>
        {
            public CacheKeyConfiguration()
                : this("dbo")
            {
            }

            public CacheKeyConfiguration(string schema)
            {
                ToTable("CacheKeys", schema);
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
            public CacheKeysItemConfiguration()
                : this("dbo")
            {
            }

            public CacheKeysItemConfiguration(string schema)
            {
                ToTable("CacheKeysItems", schema);
                HasKey(x => x.Id);

                Property(x => x.Id).HasColumnName(@"Id").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
                Property(x => x.CacheKeyId).HasColumnName(@"CacheKey_Id").HasColumnType("uniqueidentifier").IsRequired();
                Property(x => x.CacheItemId).HasColumnName(@"CacheItem_Id").HasColumnType("uniqueidentifier").IsRequired();

                // Foreign keys
                HasRequired(a => a.CacheItem).WithMany(b => b.CacheKeysItems).HasForeignKey(c => c.CacheItemId).WillCascadeOnDelete(false); // FK_CacheKeysItems_CacheItems
                HasRequired(a => a.CacheKey).WithMany(b => b.CacheKeysItems).HasForeignKey(c => c.CacheKeyId); // FK_CacheKeysItems_CacheKeys
            }
        }

        // CacheKeysItemsTemp
        public class CacheKeysItemsTempConfiguration : EntityTypeConfiguration<CacheKeyItemTemp>
        {
            public CacheKeysItemsTempConfiguration()
                : this("dbo")
            {
            }

            public CacheKeysItemsTempConfiguration(string schema)
            {
                ToTable("CacheKeysItemsTemp", schema);
                HasKey(x => x.Id);

                Property(x => x.Id).HasColumnName(@"Id").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
                Property(x => x.CacheKeyId).HasColumnName(@"CacheKey_Id").HasColumnType("uniqueidentifier").IsRequired();
                Property(x => x.CacheItemId).HasColumnName(@"CacheItem_Id").HasColumnType("uniqueidentifier").IsRequired();

                // Foreign keys
                HasRequired(a => a.CacheItemTemp).WithMany(b => b.CacheKeyItemTemp).HasForeignKey(c => c.CacheItemId).WillCascadeOnDelete(false); // FK_CacheKeysItemsTemp_CacheItemsTemp
                HasRequired(a => a.CacheKeyTemp).WithMany(b => b.CacheKeyItemTemp).HasForeignKey(c => c.CacheKeyId); // FK_CacheKeysItemsTemp_CacheKeysTemp
            }
        }

        // CacheKeysTemp
        public class CacheKeysTempConfiguration : EntityTypeConfiguration<CacheKeyTemp>
        {
            public CacheKeysTempConfiguration()
                : this("dbo")
            {
            }

            public CacheKeysTempConfiguration(string schema)
            {
                ToTable("CacheKeysTemp", schema);
                HasKey(x => x.Id);

                Property(x => x.Id).HasColumnName(@"Id").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
                Property(x => x.CacheQueueId).HasColumnName(@"CacheQueue_Id").HasColumnType("bigint").IsRequired();
                Property(x => x.SiteName).HasColumnName(@"SiteName").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(250);
                Property(x => x.SiteLang).HasColumnName(@"SiteLang").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(250);
                Property(x => x.HtmlCacheKey).HasColumnName(@"HtmlCacheKey").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(500);
                Property(x => x.HtmlCacheResult).HasColumnName(@"HtmlCacheResult").HasColumnType("varchar(max)").IsRequired().IsUnicode(false);

                // Foreign keys
                HasRequired(a => a.CacheQueue).WithMany(b => b.CacheKeysTemps).HasForeignKey(c => c.CacheQueueId).WillCascadeOnDelete(false); // FK_CacheKeysTemp_CacheQueue
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
    }
}