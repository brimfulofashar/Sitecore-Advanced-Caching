using System;
using System.Collections.Generic;
using System.Data;
using Foundation.HtmlCache.Arguments;

namespace Foundation.HtmlCache.Helpers
{
    public class GuidTVPHelper
    {

        public static DataTable GetTVPParameter(List<ItemMetaData> itemAccessList)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(Guid));
            dt.Columns.Add("Lang", typeof(string));

            foreach (var trackedItem in itemAccessList)
            {
                dt.Rows.Add(trackedItem.Id, trackedItem.Language);
            }

            return dt;
        }
    }
}