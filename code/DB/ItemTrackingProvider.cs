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
        public DbSet<CacheHtml> CacheHtmls { get; set; } // CacheHtml
        public DbSet<CacheHtmlCacheItem> CacheHtmlCacheItems { get; set; } // CacheHtml_CacheItem
        public DbSet<CacheHtmlTemp> CacheHtmlTemps { get; set; } // CacheHtmlTemp
        public DbSet<CacheHtmlTempCacheItemTemp> CacheHtmlTempCacheItemTemps { get; set; } // CacheHtmlTemp_CacheItemTemp
        public DbSet<CacheItem> CacheItems { get; set; } // CacheItem
        public DbSet<CacheItemTemp> CacheItemTemps { get; set; } // CacheItemTemp
        public DbSet<CacheQueue> CacheQueues { get; set; } // CacheQueue
        public DbSet<CacheQueueMessageType> CacheQueueMessageTypes { get; set; } // CacheQueueMessageType
        public DbSet<CacheSite> CacheSites { get; set; } // CacheSite
        public DbSet<CacheSiteTemp> CacheSiteTemps { get; set; } // CacheSiteTemp

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

            modelBuilder.Configurations.Add(new CacheHtmlConfiguration());
            modelBuilder.Configurations.Add(new CacheHtmlCacheItemConfiguration());
            modelBuilder.Configurations.Add(new CacheHtmlTempConfiguration());
            modelBuilder.Configurations.Add(new CacheHtmlTempCacheItemTempConfiguration());
            modelBuilder.Configurations.Add(new CacheItemConfiguration());
            modelBuilder.Configurations.Add(new CacheItemTempConfiguration());
            modelBuilder.Configurations.Add(new CacheQueueConfiguration());
            modelBuilder.Configurations.Add(new CacheQueueMessageTypeConfiguration());
            modelBuilder.Configurations.Add(new CacheSiteConfiguration());
            modelBuilder.Configurations.Add(new CacheSiteTempConfiguration());

            // Indexes        
            modelBuilder.Entity<CacheHtml>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_Cache", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheHtml>()
                .Property(e => e.CacheSiteId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheHtml_CacheSiteId", 1))
                );


            modelBuilder.Entity<CacheHtml>()
                .Property(e => e.HtmlCacheKeyHash)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheHtml_HtmlCacheKeyHash", 1))
                );


            modelBuilder.Entity<CacheHtmlCacheItem>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheHtml_CacheItem", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheHtmlCacheItem>()
                .Property(e => e.CacheHtmlId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheHtml_CacheItem_CacheHtmlId", 1))
                );


            modelBuilder.Entity<CacheHtmlCacheItem>()
                .Property(e => e.CacheItemId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheHtml_CacheItem_CacheItemId", 1))
                );


            modelBuilder.Entity<CacheHtmlTemp>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheHtmlTemp", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheHtmlTemp>()
                .Property(e => e.CacheQueueId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheHtmlTemp_CacheQueueId", 1))
                );


            modelBuilder.Entity<CacheHtmlTemp>()
                .Property(e => e.CacheSiteTempId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheHtmlTemp_CacheSiteTempId", 1))
                );


            modelBuilder.Entity<CacheHtmlTemp>()
                .Property(e => e.HtmlCacheKeyHash)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheHtmlTemp_HtmlCacheKeyHash", 1))
                );


            modelBuilder.Entity<CacheHtmlTempCacheItemTemp>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheHtmlTemp_CacheItemTemp", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheHtmlTempCacheItemTemp>()
                .Property(e => e.CacheQueueId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheHtmlTemp_CacheItemTemp_CacheQueueId", 1))
                );


            modelBuilder.Entity<CacheHtmlTempCacheItemTemp>()
                .Property(e => e.CacheHtmlTempId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheHtmlTemp_CacheItemTemp_CacheHtmlTempId", 1))
                );


            modelBuilder.Entity<CacheHtmlTempCacheItemTemp>()
                .Property(e => e.CacheItemTempId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheHtmlTemp_CacheItemTemp_CacheItemTempId", 1))
                );


            modelBuilder.Entity<CacheItem>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheItem", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheItem>()
                .Property(e => e.ItemId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheItem_ItemId", 1))
                );


            modelBuilder.Entity<CacheItem>()
                .Property(e => e.ItemLang)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheItem_ItemLang", 1))
                );


            modelBuilder.Entity<CacheItemTemp>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheItemTemp", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheItemTemp>()
                .Property(e => e.CacheQueueId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheItemTemp_CacheQueueId", 1))
                );


            modelBuilder.Entity<CacheItemTemp>()
                .Property(e => e.ItemId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheItemTemp_ItemId", 1))
                );


            modelBuilder.Entity<CacheItemTemp>()
                .Property(e => e.ItemLang)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheItemTemp_ItemLang", 1))
                );


            modelBuilder.Entity<CacheQueue>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheQueue", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheQueue>()
                .Property(e => e.CacheQueueMessageTypeId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheQueue_CacheQueueMessageTypeId", 1))
                );


            modelBuilder.Entity<CacheQueue>()
                .Property(e => e.Processing)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheQueue_Processing", 1))
                );


            modelBuilder.Entity<CacheQueue>()
                .Property(e => e.UpdateVersion)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheQueue_UpdateVersion", 1))
                );


            modelBuilder.Entity<CacheQueueMessageType>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheQueueMessageType", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheSite>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheSite", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheSite>()
                .Property(e => e.SiteName)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheSite_SiteName", 1))
                );


            modelBuilder.Entity<CacheSite>()
                .Property(e => e.SiteLang)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheSite_SiteLang", 1))
                );


            modelBuilder.Entity<CacheSiteTemp>()
                .Property(e => e.Id)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("PK_CacheSiteTemp", 1) { IsUnique = true, IsClustered = true })
                );


            modelBuilder.Entity<CacheSiteTemp>()
                .Property(e => e.CacheQueueId)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheSiteTemp_CacheQueueId", 1))
                );


            modelBuilder.Entity<CacheSiteTemp>()
                .Property(e => e.SiteName)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheSiteTemp_SiteName", 1))
                );


            modelBuilder.Entity<CacheSiteTemp>()
                .Property(e => e.SiteLang)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_CacheSiteTemp_SiteLang", 1))
                );

        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new CacheHtmlConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheHtmlCacheItemConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheHtmlTempConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheHtmlTempCacheItemTempConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheItemConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheItemTempConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheQueueConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheQueueMessageTypeConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheSiteConfiguration(schema));
            modelBuilder.Configurations.Add(new CacheSiteTempConfiguration(schema));

            return modelBuilder;
        }

        // Stored Procedures
        public int PurgeDatabase()
        {
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };

            Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "EXEC @procResult = [dbo].[PurgeDatabase] ", procResultParam);

            return (int)procResultParam.Value;
        }

        // PurgeDatabaseAsync() cannot be created due to having out parameters, or is relying on the procedure result (int)

        public List<UspGetCacheForSiteReturnModel> UspGetCacheForSite(string siteName)
        {
            int procResult;
            return UspGetCacheForSite(siteName, out procResult);
        }

        public List<UspGetCacheForSiteReturnModel> UspGetCacheForSite(string siteName, out int procResult)
        {
            var siteNameParam = new SqlParameter { ParameterName = "@SiteName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = siteName, Size = 250 };
            if (siteNameParam.Value == null)
                siteNameParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            var procResultData = Database.SqlQuery<UspGetCacheForSiteReturnModel>("EXEC @procResult = [dbo].[usp_GetCacheForSite] @SiteName", siteNameParam, procResultParam).ToList();
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public async Task<List<UspGetCacheForSiteReturnModel>> UspGetCacheForSiteAsync(string siteName)
        {
            var siteNameParam = new SqlParameter { ParameterName = "@SiteName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = siteName, Size = 250 };
            if (siteNameParam.Value == null)
                siteNameParam.Value = DBNull.Value;

            var procResultData = await Database.SqlQuery<UspGetCacheForSiteReturnModel>("EXEC [dbo].[usp_GetCacheForSite] @SiteName", siteNameParam).ToListAsync();
            return procResultData;
        }

        public List<UspGetStatsReturnModel> UspGetStats()
        {
            int procResult;
            return UspGetStats(out procResult);
        }

        public List<UspGetStatsReturnModel> UspGetStats(out int procResult)
        {
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            var procResultData = Database.SqlQuery<UspGetStatsReturnModel>("EXEC @procResult = [dbo].[usp_GetStats]", procResultParam).ToList();
            procResult = (int) procResultParam.Value;
            return procResultData;
        }

        public async Task<List<UspGetStatsReturnModel>> UspGetStatsAsync()
        {
            var procResultData = await Database.SqlQuery<UspGetStatsReturnModel>("EXEC [dbo].[usp_GetStats]").ToListAsync();
            return procResultData;
        }

        public List<UspLockAndProcessCacheQueueEntryReturnModel> UspLockAndProcessCacheQueueEntry(string processingBy, out long? cacheQueueCount)
        {
            int procResult;
            return UspLockAndProcessCacheQueueEntry(processingBy, out cacheQueueCount, out procResult);
        }

        public List<UspLockAndProcessCacheQueueEntryReturnModel> UspLockAndProcessCacheQueueEntry(string processingBy, out long? cacheQueueCount, out int procResult)
        {
            var processingByParam = new SqlParameter { ParameterName = "@ProcessingBy", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = processingBy, Size = 250 };
            if (processingByParam.Value == null)
                processingByParam.Value = DBNull.Value;

            var cacheQueueCountParam = new SqlParameter { ParameterName = "@CacheQueueCount", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Output, Precision = 19, Scale = 0 };
            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            var procResultData = Database.SqlQuery<UspLockAndProcessCacheQueueEntryReturnModel>("EXEC @procResult = [dbo].[usp_LockAndProcessCacheQueueEntry] @ProcessingBy, @CacheQueueCount OUTPUT", processingByParam, cacheQueueCountParam, procResultParam).ToList();
            if (IsSqlParameterNull(cacheQueueCountParam))
                cacheQueueCount = null;
            else
                cacheQueueCount = (long)cacheQueueCountParam.Value;

            procResult = (int)procResultParam.Value;
            return procResultData;
        }

        // UspLockAndProcessCacheQueueEntryAsync() cannot be created due to having out parameters, or is relying on the procedure result (int)

        public int UspProcessCacheData(long? cacheQueueId)
        {
            var cacheQueueIdParam = new SqlParameter { ParameterName = "@CacheQueueId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = cacheQueueId.GetValueOrDefault(), Precision = 19, Scale = 0 };
            if (!cacheQueueId.HasValue)
                cacheQueueIdParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };

            Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "EXEC @procResult = [dbo].[usp_ProcessCacheData] @CacheQueueId", cacheQueueIdParam, procResultParam);

            return (int)procResultParam.Value;
        }

        // UspProcessCacheDataAsync() cannot be created due to having out parameters, or is relying on the procedure result (int)

        public int UspProcessDeleteHtmlFromCache(long? cacheQueueId, int? cacheQueueMessageTypeId)
        {
            var cacheQueueIdParam = new SqlParameter { ParameterName = "@CacheQueueId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = cacheQueueId.GetValueOrDefault(), Precision = 19, Scale = 0 };
            if (!cacheQueueId.HasValue)
                cacheQueueIdParam.Value = DBNull.Value;

            var cacheQueueMessageTypeIdParam = new SqlParameter { ParameterName = "@CacheQueueMessageTypeId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = cacheQueueMessageTypeId.GetValueOrDefault(), Precision = 10, Scale = 0 };
            if (!cacheQueueMessageTypeId.HasValue)
                cacheQueueMessageTypeIdParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };

            Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "EXEC @procResult = [dbo].[usp_ProcessDeleteHtmlFromCache] @CacheQueueId, @CacheQueueMessageTypeId", cacheQueueIdParam, cacheQueueMessageTypeIdParam, procResultParam);

            return (int)procResultParam.Value;
        }

        // UspProcessDeleteHtmlFromCacheAsync() cannot be created due to having out parameters, or is relying on the procedure result (int)

        public int UspProcessDeleteSiteFromCache(long? cacheQueueId, int? cacheQueueMessageTypeId)
        {
            var cacheQueueIdParam = new SqlParameter { ParameterName = "@CacheQueueId", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = cacheQueueId.GetValueOrDefault(), Precision = 19, Scale = 0 };
            if (!cacheQueueId.HasValue)
                cacheQueueIdParam.Value = DBNull.Value;

            var cacheQueueMessageTypeIdParam = new SqlParameter { ParameterName = "@CacheQueueMessageTypeId", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = cacheQueueMessageTypeId.GetValueOrDefault(), Precision = 10, Scale = 0 };
            if (!cacheQueueMessageTypeId.HasValue)
                cacheQueueMessageTypeIdParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };

            Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "EXEC @procResult = [dbo].[usp_ProcessDeleteSiteFromCache] @CacheQueueId, @CacheQueueMessageTypeId", cacheQueueIdParam, cacheQueueMessageTypeIdParam, procResultParam);

            return (int)procResultParam.Value;
        }

        // UspProcessDeleteSiteFromCacheAsync() cannot be created due to having out parameters, or is relying on the procedure result (int)

        public int UspQueueCacheData(DataTable cacheSiteTvp, DataTable cacheHtmlTvp, DataTable cacheHtmlCacheItemTvp, DataTable cacheItemTvp)
        {
            var cacheSiteTvpParam = new SqlParameter { ParameterName = "@CacheSite_TVP", SqlDbType = SqlDbType.Structured, Direction = ParameterDirection.Input, Value = cacheSiteTvp, TypeName = "dbo.CacheSite_TVP" };
            if (cacheSiteTvpParam.Value == null)
                cacheSiteTvpParam.Value = DBNull.Value;

            var cacheHtmlTvpParam = new SqlParameter { ParameterName = "@CacheHtml_TVP", SqlDbType = SqlDbType.Structured, Direction = ParameterDirection.Input, Value = cacheHtmlTvp, TypeName = "dbo.CacheHtml_TVP" };
            if (cacheHtmlTvpParam.Value == null)
                cacheHtmlTvpParam.Value = DBNull.Value;

            var cacheHtmlCacheItemTvpParam = new SqlParameter { ParameterName = "@CacheHtml_CacheItem_TVP", SqlDbType = SqlDbType.Structured, Direction = ParameterDirection.Input, Value = cacheHtmlCacheItemTvp, TypeName = "dbo.CacheHtml_CacheItem_TVP" };
            if (cacheHtmlCacheItemTvpParam.Value == null)
                cacheHtmlCacheItemTvpParam.Value = DBNull.Value;

            var cacheItemTvpParam = new SqlParameter { ParameterName = "@CacheItem_TVP", SqlDbType = SqlDbType.Structured, Direction = ParameterDirection.Input, Value = cacheItemTvp, TypeName = "dbo.CacheItem_TVP" };
            if (cacheItemTvpParam.Value == null)
                cacheItemTvpParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };

            Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "EXEC @procResult = [dbo].[usp_QueueCacheData] @CacheSite_TVP, @CacheHtml_TVP, @CacheHtml_CacheItem_TVP, @CacheItem_TVP", cacheSiteTvpParam, cacheHtmlTvpParam, cacheHtmlCacheItemTvpParam, cacheItemTvpParam, procResultParam);

            return (int)procResultParam.Value;
        }

        // UspQueueCacheDataAsync() cannot be created due to having out parameters, or is relying on the procedure result (int)

        public int UspQueueDeleteSiteFromCache(string siteName, string siteLang)
        {
            var siteNameParam = new SqlParameter { ParameterName = "@SiteName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = siteName, Size = 250 };
            if (siteNameParam.Value == null)
                siteNameParam.Value = DBNull.Value;

            var siteLangParam = new SqlParameter { ParameterName = "@SiteLang", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = siteLang, Size = 250 };
            if (siteLangParam.Value == null)
                siteLangParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };

            Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "EXEC @procResult = [dbo].[usp_QueueDeleteSiteFromCache] @SiteName, @SiteLang", siteNameParam, siteLangParam, procResultParam);

            return (int)procResultParam.Value;
        }

        // UspQueueDeleteSiteFromCacheAsync() cannot be created due to having out parameters, or is relying on the procedure result (int)

        public int UspQueuePublishData(string siteLang, DataTable ids)
        {
            var siteLangParam = new SqlParameter { ParameterName = "@SiteLang", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = siteLang, Size = 250 };
            if (siteLangParam.Value == null)
                siteLangParam.Value = DBNull.Value;

            var idsParam = new SqlParameter { ParameterName = "@Ids", SqlDbType = SqlDbType.Structured, Direction = ParameterDirection.Input, Value = ids, TypeName = "dbo.ItemMetaData" };
            if (idsParam.Value == null)
                idsParam.Value = DBNull.Value;

            var procResultParam = new SqlParameter { ParameterName = "@procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };

            Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "EXEC @procResult = [dbo].[usp_QueuePublishData] @SiteLang, @Ids", siteLangParam, idsParam, procResultParam);

            return (int)procResultParam.Value;
        }

        // UspQueuePublishDataAsync() cannot be created due to having out parameters, or is relying on the procedure result (int)

    }
}
// </auto-generated>

