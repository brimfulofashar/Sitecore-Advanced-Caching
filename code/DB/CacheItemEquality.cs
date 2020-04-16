namespace Foundation.HtmlCache.DB
{
    public partial class CacheItem
    {
        public override bool Equals(object obj)
        {
            var item = obj as CacheItem;

            if (item == null)
            {
                return false;
            }

            return this.ItemId.Equals(item.ItemId) && this.ItemLang.Equals(item.ItemLang);
        }

        public override int GetHashCode()
        {
            unchecked // disable overflow, for the unlikely possibility that you
            {         // are compiling with overflow-checking enabled
                int hash = 27;
                hash = (13 * hash) + ItemId.GetHashCode();
                hash = (13 * hash) + ItemLang.GetHashCode();
                return hash;
            }
        }
    }

}

