using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Foundation.HtmlCache.DB
{
    public partial class CacheSiteTemp
    {
        public override bool Equals(object obj)
        {
            var item = obj as CacheSiteTemp;

            if (item == null)
            {
                return false;
            }

            return this.SiteName.Equals(item.SiteName) && this.SiteLang.Equals(item.SiteLang);
        }

        public override int GetHashCode()
        {
            unchecked // disable overflow, for the unlikely possibility that you
            {         // are compiling with overflow-checking enabled
                int hash = 27;
                hash = (13 * hash) + SiteName.GetHashCode();
                hash = (13 * hash) + SiteLang.GetHashCode();
                return hash;
            }
        }
    }

}

