// ------------------------------------------------------------------------------------------------
// This code was generated by EntityFramework Reverse POCO Generator (http://www.reversepoco.co.uk/).
// Created by Simon Hughes (https://about.me/simon.hughes).
//
// Registered to: Ashar Shah
// Company      : N/A
// Licence Type : Commercial
// Licences     : 1
// Valid until  : 11 DEC 2020
//
// Do not make changes directly to this file - edit the template instead.
// ------------------------------------------------------------------------------------------------

// <auto-generated>
// ReSharper disable CheckNamespace
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable EmptyNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable NotAccessedVariable
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantCast
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// ReSharper disable UsePatternMatching

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Spatial;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Foundation.HtmlCache.DB
{
    public class ItemTrackingProvider : DbContext, IItemTrackingProvider
    {
        public DbSet<Cache> Caches { get; set; } // Cache
        public DbSet<CacheQueue> CacheQueues { get; set; } // CacheQueue
        public DbSet<CacheQueueBlocker> CacheQueueBlockers { get; set; } // CacheQueueBlocker
        public DbSet<CacheQueueMessageType> CacheQueueMessageTypes { get; set; } // CacheQueueMessageType
        public DbSet<CacheTemp> CacheTemps { get; set; } // CacheTemp

        static ItemTrackingProvider()
        {
            System.Data.Entity.Database.SetInitializer<ItemTrackingProvider>(null);
        }

        /// <inheritdoc />
        public ItemTrackingProvider()
            : base("Name=HtmlCache")
        {
        }

        /// <inheritdoc />
        public ItemTrackingProvider(string connectionString)
            : base(connectionString)
        {
        }

        /// <inheritdoc />
        public ItemTrackingProvider(string connectionString, DbCompiledModel model)
            : base(connectionString, model)
        {
        }

        /// <inheritdoc />
        public ItemTrackingProvider(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        /// <inheritdoc />
        public ItemTrackingProvider(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }

        /// <inheritdoc />
        public ItemTrackingProvider(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public bool IsSqlParameterNull(SqlParameter param)
        {
            var sqlValue = param.SqlValue;
            var nullableValue = sqlValue as INullable;
            if (nullableValue != null)
                return nullableValue.IsNull;
            return (sqlValue == null || sqlValue == DBNull.Value);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new CacheConfiguration());
            modelBuilder.Configurations.Add(new CacheQueueConfiguration());
            modelBuilder.Configurations.Add(new CacheQueueBlockerConfiguration());
            modelBuilder.Configurations.Add(new CacheQueueMessageTypeConfiguration());
            modelBuilder.Configurations.Add(new CacheTempConfiguration());

            // Indexes        
            modelBuilder.Entity<Cache>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_Cache", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<Cache>()
                .Property(e => e.SiteName)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new[]
                    {
                        new IndexAttribute("IX_Cache", 1) { IsUnique = true },
                        new IndexAttribute("IX_Cache_SiteNameSiteLang", 1)
                    }));


            modelBuilder.Entity<Cache>()
                .Property(e => e.SiteLang)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new[]
                    {
                        new IndexAttribute("IX_Cache", 2) { IsUnique = true },
                        new IndexAttribute("IX_Cache_SiteNameSiteLang", 2)
                    }));


            modelBuilder.Entity<Cache>()
                .Property(e => e.HtmlCacheKeyHash)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new[]
                    {
                        new IndexAttribute("IX_Cache", 3) { IsUnique = true },
                        new IndexAttribute("IX_Cache_HtmlCacheKeyHash", 1)
                    }));


            modelBuilder.Entity<Cache>()
                .Property(e => e.ItemId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new[]
                    {
                        new IndexAttribute("IX_Cache", 4) { IsUnique = true },
                        new IndexAttribute("IX_Cache_ItemId", 1)
                    }));


            modelBuilder.Entity<CacheQueue>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheQueue", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheQueueBlocker>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheQueueBlocker", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheQueueMessageType>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheQueueMessageType", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheTemp>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheTemp", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheTemp>()
                .Property(e => e.SiteName)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new[]
                    {
                        new IndexAttribute("IX_CacheTemp", 1) { IsUnique = true },
                        new IndexAttribute("IX_CacheTemp_SiteNameSiteLang", 1)
                    }));


            modelBuilder.Entity<CacheTemp>()
                .Property(e => e.SiteLang)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new[]
                    {
                        new IndexAttribute("IX_CacheTemp", 2) { IsUnique = true },
                        new IndexAttribute("IX_CacheTemp_SiteNameSiteLang", 2)
                    }));


            modelBuilder.Entity<CacheTemp>()
                .Property(e => e.HtmlCacheKeyHash)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new[]
                    {
                        new IndexAttribute("IX_CacheTemp", 3) { IsUnique = true },
                        new IndexAttribute("IX_CacheTemp_HtmlCacheKeyHash", 1)
                    }));


            modelBuilder.Entity<CacheTemp>()
                .Property(e => e.ItemId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new[]
                    {
                        new IndexAttribute("IX_CacheTemp", 4) { IsUnique = true },
                        new IndexAttribute("IX_CacheTemp_ItemId", 1)
                    }));

        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new CacheConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheQueueConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheQueueBlockerConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheQueueMessageTypeConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheTempConfiguration(schema));

            return modelBuilder;
        }
    }
}
// </auto-generated>

