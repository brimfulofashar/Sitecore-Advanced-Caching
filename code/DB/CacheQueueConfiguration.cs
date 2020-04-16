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
    // CacheQueue
    public partial class CacheQueueConfiguration : EntityTypeConfiguration<CacheQueue>
    {
        public CacheQueueConfiguration()
            : this("dbo")
        {
        }

        public CacheQueueConfiguration(string schema)
        {
            ToTable("CacheQueue", schema);
            HasKey(x => new { x.HashId, x.Id });

            Property(x => x.HashId).HasColumnName(@"HashId").HasColumnType("tinyint").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("bigint").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CacheQueueMessageTypeId).HasColumnName(@"CacheQueueMessageTypeId").HasColumnType("tinyint").IsRequired();
            Property(x => x.Processing).HasColumnName(@"Processing").HasColumnType("bit").IsRequired();
            Property(x => x.ProcessingBy).HasColumnName(@"ProcessingBy").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(250);
            Property(x => x.UpdateVersion).HasColumnName(@"UpdateVersion").HasColumnType("timestamp").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion();

            // Foreign keys
            HasOptional(a => a.CacheQueueMessageType).WithMany(b => b.CacheQueues).HasForeignKey(c => c.CacheQueueMessageTypeId).WillCascadeOnDelete(false); // FK_CacheQueue_CacheQueueMessageType
        }
    }

}
// </auto-generated>

