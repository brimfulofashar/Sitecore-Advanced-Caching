using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Foundation.HtmlCache.Arguments;

namespace Foundation.HtmlCache.Helpers
{
    public class GuidTVPHelper
    {

        public static DataTable GetTVPParameter(List<Guid> itemAccessList)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id");

            foreach (var trackedItem in itemAccessList)
            {
                dt.Rows.Add(trackedItem);
            }

            return dt;
        }
    }
}