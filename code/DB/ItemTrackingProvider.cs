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
        public DbSet<CacheItem> CacheItems { get; set; } // CacheItem
        public DbSet<CacheItemTemp> CacheItemTemps { get; set; } // CacheItemTemp
        public DbSet<CacheKey> CacheKeys { get; set; } // CacheKey
        public DbSet<CacheKeyItem> CacheKeyItems { get; set; } // CacheKeyItem
        public DbSet<CacheKeyTemp> CacheKeyTemps { get; set; } // CacheKeyTemp
        public DbSet<CacheQueue> CacheQueues { get; set; } // CacheQueue
        public DbSet<CacheQueueBlocker> CacheQueueBlockers { get; set; } // CacheQueueBlocker
        public DbSet<CacheQueueMessageType> CacheQueueMessageTypes { get; set; } // CacheQueueMessageType
        public DbSet<CacheSiteLang> CacheSiteLangs { get; set; } // CacheSiteLang
        public DbSet<CacheSiteLangTemp> CacheSiteLangTemps { get; set; } // CacheSiteLangTemp

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

            modelBuilder.Configurations.Add(new CacheItemConfiguration());
            modelBuilder.Configurations.Add(new CacheItemTempConfiguration());
            modelBuilder.Configurations.Add(new CacheKeyConfiguration());
            modelBuilder.Configurations.Add(new CacheKeyItemConfiguration());
            modelBuilder.Configurations.Add(new CacheKeyTempConfiguration());
            modelBuilder.Configurations.Add(new CacheQueueConfiguration());
            modelBuilder.Configurations.Add(new CacheQueueBlockerConfiguration());
            modelBuilder.Configurations.Add(new CacheQueueMessageTypeConfiguration());
            modelBuilder.Configurations.Add(new CacheSiteLangConfiguration());
            modelBuilder.Configurations.Add(new CacheSiteLangTempConfiguration());

            // Indexes        
            modelBuilder.Entity<CacheItem>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheItem", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheItemTemp>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheItemTemp", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheItemTemp>()
                .Property(e => e.ItemId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheItemTemp", 1))
                );


            modelBuilder.Entity<CacheKey>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheKey", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheKeyItem>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheKeyItem", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheKeyTemp>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheKeyTemp", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheKeyTemp>()
                .Property(e => e.HtmlCacheKey)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheKeyTemp", 1))
                );


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


            modelBuilder.Entity<CacheSiteLang>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheSiteLang", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheSiteLangTemp>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheSiteLangTemp", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheSiteLangTemp>()
                .Property(e => e.Name)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheSiteLangTemp", 1))
                );


            modelBuilder.Entity<CacheSiteLangTemp>()
                .Property(e => e.Lang)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheSiteLangTemp", 2))
                );

        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new CacheItemConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheItemTempConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheKeyConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheKeyItemConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheKeyTempConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheQueueConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheQueueBlockerConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheQueueMessageTypeConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheSiteLangConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheSiteLangTempConfiguration(schema));

            return modelBuilder;
        }
    }
}
// </auto-generated>
