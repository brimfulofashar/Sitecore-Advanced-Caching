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
            string tableName = "CacheItem" + suffix;
            builder.Entity<CacheItem>().ToTable(tableName);

            builder.Configurations.Add(new EntityTypeConfiguration<CacheKey>());
            tableName = "CacheKey" + suffix;
            builder.Entity<CacheKey>().ToTable(tableName);
            
            builder.Configurations.Add(new EntityTypeConfiguration<CacheKeyItem>());
            tableName = "CacheKeyItem" + suffix;
            builder.Entity<CacheKeyItem>().ToTable(tableName);

            // Compile ORM object, hard link connection
            DbConnection dummyConnection = dummyContext.Database.Connection;
            DbCompiledModel compiledModel = builder.Build(dummyConnection).Compile();

            // Finally make our database context
            dummyContext = new ItemTrackingProvider(compiledModel, dummyContext.Database.Connection.ConnectionString);

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
    }
}