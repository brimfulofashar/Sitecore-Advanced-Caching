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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Foundation.HtmlCache.DB
{
    public interface IItemTrackingProvider : IDisposable
    {
        DbSet<CacheHtml> CacheHtmls { get; set; } // CacheHtml
        DbSet<CacheHtmlCacheItem> CacheHtmlCacheItems { get; set; } // CacheHtml_CacheItem
        DbSet<CacheItem> CacheItems { get; set; } // CacheItem
        DbSet<CacheSite> CacheSites { get; set; } // CacheSite

        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DbChangeTracker ChangeTracker { get; }
        DbContextConfiguration Configuration { get; }
        Database Database { get; }
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbEntityEntry Entry(object entity);
        IEnumerable<DbEntityValidationResult> GetValidationErrors();
        DbSet Set(Type entityType);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        string ToString();

        // Stored Procedures
        int PurgeDatabase();
        // PurgeDatabaseAsync() cannot be created due to having out parameters, or is relying on the procedure result (int)

        List<UspDeleteCacheDataReturnModel> UspDeleteCacheData(DataTable cacheItemTvp);
        // UspDeleteCacheDataAsync() cannot be created due to having out parameters, or is relying on the procedure result (int)

        List<UspDeleteCacheDataForSiteReturnModel> UspDeleteCacheDataForSite(string siteName, string siteLang);
        List<UspDeleteCacheDataForSiteReturnModel> UspDeleteCacheDataForSite(string siteName, string siteLang, out int procResult);
        Task<List<UspDeleteCacheDataForSiteReturnModel>> UspDeleteCacheDataForSiteAsync(string siteName, string siteLang);

        List<UspGetCacheForSiteReturnModel> UspGetCacheForSite(string siteName);
        List<UspGetCacheForSiteReturnModel> UspGetCacheForSite(string siteName, out int procResult);
        Task<List<UspGetCacheForSiteReturnModel>> UspGetCacheForSiteAsync(string siteName);

        List<UspGetStatsReturnModel> UspGetStats();
        List<UspGetStatsReturnModel> UspGetStats(out int procResult);
        Task<List<UspGetStatsReturnModel>> UspGetStatsAsync();

        int UspMergeCacheData(DataTable cacheSiteTvp, DataTable cacheHtmlTvp, DataTable cacheHtmlCacheItemTvp, DataTable cacheItemTvp);
        // UspMergeCacheDataAsync() cannot be created due to having out parameters, or is relying on the procedure result (int)

    }
}
// </auto-generated>

