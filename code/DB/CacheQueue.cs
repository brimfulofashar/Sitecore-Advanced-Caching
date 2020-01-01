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
        public string Site { get; set; } // Site (length: 250)
        public string Language { get; set; } // Language (length: 250)
        public bool Processing { get; set; } // Processing
        public byte[] UpdateVersion { get; set; } // UpdateVersion (length: 8)

        // Reverse navigation

        /// <summary>
        /// Child CacheTemps where [CacheTemp].[CacheQueueId] point to this entity (FK_CacheTemp_CacheQueue)
        /// </summary>
        public virtual ICollection<CacheTemp> CacheTemps { get; set; } // CacheTemp.FK_CacheTemp_CacheQueue

        // Foreign keys

        /// <summary>
        /// Parent CacheQueueMessageType pointed by [CacheQueue].([CacheQueueMessageTypeId]) (FK_CacheQueue_CacheQueueMessageType)
        /// </summary>
        public virtual CacheQueueMessageType CacheQueueMessageType { get; set; } // FK_CacheQueue_CacheQueueMessageType

        public CacheQueue()
        {
            CacheTemps = new List<CacheTemp>();
        }
    }

}
// </auto-generated>

