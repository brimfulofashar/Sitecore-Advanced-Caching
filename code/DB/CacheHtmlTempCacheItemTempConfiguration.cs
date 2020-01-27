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
using System.Data;
using System.Data.Entity.ModelConfiguration;

namespace Foundation.HtmlCache.DB
{
    // CacheHtmlTemp_CacheItemTemp
    public partial class CacheHtmlTempCacheItemTempConfiguration : EntityTypeConfiguration<CacheHtmlTempCacheItemTemp>
    {
        public CacheHtmlTempCacheItemTempConfiguration()
            : this("dbo")
        {
        }

        public CacheHtmlTempCacheItemTempConfiguration(string schema)
        {
            ToTable("CacheHtmlTemp_CacheItemTemp", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("uniqueidentifier").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.CacheQueueId).HasColumnName(@"CacheQueueId").HasColumnType("bigint").IsRequired();
            Property(x => x.CacheHtmlTempId).HasColumnName(@"CacheHtmlTempId").HasColumnType("uniqueidentifier").IsRequired();
            Property(x => x.CacheItemTempId).HasColumnName(@"CacheItemTempId").HasColumnType("uniqueidentifier").IsRequired();

            // Foreign keys
            HasRequired(a => a.CacheHtmlTemp).WithMany(b => b.CacheHtmlTempCacheItemTemps).HasForeignKey(c => c.CacheHtmlTempId).WillCascadeOnDelete(false); // FK_CacheHtmlTemp_CacheItemTemp_CacheHtmlTemp
            HasRequired(a => a.CacheItemTemp).WithMany(b => b.CacheHtmlTempCacheItemTemps).HasForeignKey(c => c.CacheItemTempId).WillCascadeOnDelete(false); // FK_CacheHtmlTemp_CacheItemTemp_CacheItemTemp
            HasRequired(a => a.CacheQueue).WithMany(b => b.CacheHtmlTempCacheItemTemps).HasForeignKey(c => c.CacheQueueId).WillCascadeOnDelete(false); // FK_CacheHtmlTemp_CacheItemTemp_CacheQueue
        }
    }

}
// </auto-generated>
