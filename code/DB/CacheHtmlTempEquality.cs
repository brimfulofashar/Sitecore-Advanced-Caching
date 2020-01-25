namespace Foundation.HtmlCache.DB
{
    public partial class CacheHtmlTemp
    {
        public override bool Equals(object obj)
        {
            var item = obj as CacheHtmlTemp;

            if (item == null)
            {
                return false;
            }

            return this.HtmlCacheKey.Equals(item.HtmlCacheKey);
        }

        public override int GetHashCode()
        {
            unchecked // disable overflow, for the unlikely possibility that you
            {         // are compiling with overflow-checking enabled
                int hash = 27;
                hash = (13 * hash) + HtmlCacheKey.GetHashCode();
                return hash;
            }
        }
    }

}

