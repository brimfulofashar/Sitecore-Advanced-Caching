using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundation.HtmlCache.Models
{
    public class ItemMetaData
    {
        public ItemMetaData(Guid id, Guid templateId)
        {
            Id = id;
            TempalteId = templateId;
        }

        public readonly Guid Id;
        public readonly Guid TempalteId;

        public override bool Equals(object obj)
        {
            var itemMetaData = obj as ItemMetaData;
            if (itemMetaData == null)
                return false;
            return itemMetaData.Id == Id && itemMetaData.TempalteId == TempalteId;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ TempalteId.GetHashCode();
        }
    }
}