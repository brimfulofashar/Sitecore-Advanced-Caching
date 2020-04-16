namespace Foundation.HtmlCache.DB
{
    public partial class CacheHtmlCacheItem
    {
        public override bool Equals(object obj)
        {
            var item = obj as CacheHtmlCacheItem;

            if (item == null)
            {
                return false;
            }

            return this.CacheHtmlId.Equals(item.CacheHtmlId) && this.CacheItemId.Equals(item.CacheItemId);
        }

        public override int GetHashCode()
        {
            unchecked // disable overflow, for the unlikely possibility that you
            {         // are compiling with overflow-checking enabled
                int hash = 27;
                hash = (13 * hash) + CacheHtmlId.GetHashCode();
                hash = (13 * hash) + CacheItemId.GetHashCode();
                return hash;
            }
        }
    }

}

