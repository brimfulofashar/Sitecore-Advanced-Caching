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
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Foundation.HtmlCache.DB
{
    // CacheHtml_CacheItem
    [Table("CacheHtml_CacheItem")]
    public partial class CacheHtmlCacheItem
    {
        public long Id { get; set; } // Id (Primary key)
        public long MergeId { get; set; } // MergeId
        public long CacheHtmlId { get; set; } // CacheHtmlId
        public long CacheItemId { get; set; } // CacheItemId

        // Foreign keys

        /// <summary>
        /// Parent CacheHtml pointed by [CacheHtml_CacheItem].([CacheHtmlId]) (FK_CacheHtml_CacheItem_CacheHtml)
        /// </summary>
        public virtual CacheHtml CacheHtml { get; set; } // FK_CacheHtml_CacheItem_CacheHtml

        /// <summary>
        /// Parent CacheItem pointed by [CacheHtml_CacheItem].([CacheItemId]) (FK_CacheHtml_CacheItem_CacheItem)
        /// </summary>
        public virtual CacheItem CacheItem { get; set; } // FK_CacheHtml_CacheItem_CacheItem

        public CacheHtmlCacheItem()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

