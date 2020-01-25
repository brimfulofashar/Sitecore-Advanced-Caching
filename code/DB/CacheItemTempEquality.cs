namespace Foundation.HtmlCache.DB
{
    public partial class CacheItemTemp
    {
        public override bool Equals(object obj)
        {
            var item = obj as CacheItemTemp;

            if (item == null)
            {
                return false;
            }

            return this.ItemId.Equals(item.ItemId) && this.ItemLang.Equals(item.ItemLang) && this.IsDeleted.Equals(item.IsDeleted);
        }

        public override int GetHashCode()
        {
            unchecked // disable overflow, for the unlikely possibility that you
            {         // are compiling with overflow-checking enabled
                int hash = 27;
                hash = (13 * hash) + ItemId.GetHashCode();
                hash = (13 * hash) + ItemLang.GetHashCode();
                hash = (13 * hash) + IsDeleted.GetHashCode();
                return hash;
            }
        }
    }

}

