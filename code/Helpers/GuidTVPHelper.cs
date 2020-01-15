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
            dt.Columns.Add("Id");
            dt.Columns.Add("Lang");

            foreach (var trackedItem in itemAccessList)
            {
                dt.Rows.Add(new {trackedItem.Id, trackedItem.Language});
            }

            return dt;
        }
    }
}