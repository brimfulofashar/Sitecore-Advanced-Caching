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
    // CacheItem
    [Table("CacheItem")]
    public class CacheItem
    {
        public Guid Id { get; set; } // Id (Primary key)
        public Guid ItemId { get; set; } // ItemId

        // Reverse navigation

        /// <summary>
        /// Child CacheHtmlCacheItems where [CacheHtml_CacheItem].[CacheItemId] point to this entity (FK_CacheHtml_CacheItem_CacheItem)
        /// </summary>
        public virtual ICollection<CacheHtmlCacheItem> CacheHtmlCacheItems { get; set; } // CacheHtml_CacheItem.FK_CacheHtml_CacheItem_CacheItem

        public CacheItem()
        {
            Id = Guid.NewGuid();
            CacheHtmlCacheItems = new List<CacheHtmlCacheItem>();
        }
    }

}
// </auto-generated>

