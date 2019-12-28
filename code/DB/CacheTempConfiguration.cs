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
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Foundation.HtmlCache.DB
{
    // CacheTemp
    public class CacheTempConfiguration : EntityTypeConfiguration<CacheTemp>
    {
        public CacheTempConfiguration()
            : this("dbo")
        {
        }

        public CacheTempConfiguration(string schema)
        {
            ToTable("CacheTemp", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.CacheQueueId).HasColumnName(@"CacheQueueId").HasColumnType("bigint").IsRequired();
            Property(x => x.SiteName).HasColumnName(@"SiteName").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(250);
            Property(x => x.SiteLang).HasColumnName(@"SiteLang").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(250);
            Property(x => x.HtmlCacheKey).HasColumnName(@"HtmlCacheKey").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(5000);
            Property(x => x.HtmlCacheKeyHash).HasColumnName(@"HtmlCacheKeyHash").HasColumnType("varbinary").IsOptional().HasMaxLength(8000).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.HtmlCacheResult).HasColumnName(@"HtmlCacheResult").HasColumnType("varchar(max)").IsOptional().IsUnicode(false);
            Property(x => x.ItemId).HasColumnName(@"ItemId").HasColumnType("uniqueidentifier").IsOptional();

            // Foreign keys
            HasRequired(a => a.CacheQueue).WithMany(b => b.CacheTemps).HasForeignKey(c => c.CacheQueueId); // FK_CacheTemp_CacheQueue
        }
    }

}
// </auto-generated>

