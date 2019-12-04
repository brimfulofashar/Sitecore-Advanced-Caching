using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using System.Linq;
using System.Web;
using Foundation.HtmlCache.Models;

namespace Foundation.HtmlCache.Providers
{
    public class ItemTrackingProvider : DbContext
    {

        public ItemTrackingProvider() : base("HtmlCache")
        {
            Database.SetInitializer<ItemTrackingProvider>(null);//Disable initializer
        }

        public DbSet<CacheItem> CacheItems { get; set; }
        public DbSet<CacheKey> CacheKeys { get; set; }
        public DbSet<CacheKeysItem> CacheKeyItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CacheKey>()
                .HasMany<CacheItem>(s => s.CacheItems)
                .WithMany(c => c.CacheKeys)
                .Map(cs =>
                {
                    cs.MapLeftKey("CacheKeyId");
                    cs.MapRightKey("CacheItemId");
                    cs.ToTable("CacheKeysItems");
                });

            modelBuilder.Entity<CacheItem>()
                .HasMany<CacheKey>(s => s.CacheKeys)
                .WithMany(c => c.CacheItems)
                .Map(cs =>
                {
                    cs.MapLeftKey("CacheKeyId");
                    cs.MapRightKey("CacheItemId");
                    cs.ToTable("CacheKeysItems");
                });
        }
    }
}