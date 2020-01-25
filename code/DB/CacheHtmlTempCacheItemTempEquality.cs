namespace Foundation.HtmlCache.DB
{
    public partial class CacheHtmlTempCacheItemTemp
    {
        public override bool Equals(object obj)
        {
            var item = obj as CacheHtmlTempCacheItemTemp;

            if (item == null)
            {
                return false;
            }

            return this.CacheHtmlTempId.Equals(item.CacheHtmlTempId) && this.CacheItemTempId.Equals(item.CacheItemTempId);
        }

        public override int GetHashCode()
        {
            unchecked // disable overflow, for the unlikely possibility that you
            {         // are compiling with overflow-checking enabled
                int hash = 27;
                hash = (13 * hash) + CacheHtmlTempId.GetHashCode();
                hash = (13 * hash) + CacheItemTempId.GetHashCode();
                return hash;
            }
        }
    }

}

