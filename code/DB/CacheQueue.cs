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
    // CacheQueue
    [Table("CacheQueue")]
    public class CacheQueue
    {
        public long Id { get; set; } // Id (Primary key)
        public int CacheQueueMessageTypeId { get; set; } // CacheQueueMessageTypeId
        public bool Processing { get; set; } // Processing
        public string ProcessingBy { get; set; } // ProcessingBy (length: 250)
        public byte[] UpdateVersion { get; set; } // UpdateVersion (length: 8)

        // Reverse navigation

        /// <summary>
        /// Child CacheHtmlTemps where [CacheHtmlTemp].[CacheQueueId] point to this entity (FK_CacheHtmlTemp_CacheQueue)
        /// </summary>
        public virtual ICollection<CacheHtmlTemp> CacheHtmlTemps { get; set; } // CacheHtmlTemp.FK_CacheHtmlTemp_CacheQueue

        /// <summary>
        /// Child CacheItemTemps where [CacheItemTemp].[CacheQueueId] point to this entity (FK_CacheItemTemp_CacheQueue)
        /// </summary>
        public virtual ICollection<CacheItemTemp> CacheItemTemps { get; set; } // CacheItemTemp.FK_CacheItemTemp_CacheQueue

        /// <summary>
        /// Child CacheSiteTemps where [CacheSiteTemp].[CacheQueueId] point to this entity (FK_CacheSiteTemp_CacheQueue)
        /// </summary>
        public virtual ICollection<CacheSiteTemp> CacheSiteTemps { get; set; } // CacheSiteTemp.FK_CacheSiteTemp_CacheQueue

        // Foreign keys

        /// <summary>
        /// Parent CacheQueueMessageType pointed by [CacheQueue].([CacheQueueMessageTypeId]) (FK_CacheQueue_CacheQueueMessageType)
        /// </summary>
        public virtual CacheQueueMessageType CacheQueueMessageType { get; set; } // FK_CacheQueue_CacheQueueMessageType

        public CacheQueue()
        {
            CacheHtmlTemps = new List<CacheHtmlTemp>();
            CacheItemTemps = new List<CacheItemTemp>();
            CacheSiteTemps = new List<CacheSiteTemp>();
        }
    }

}
// </auto-generated>

