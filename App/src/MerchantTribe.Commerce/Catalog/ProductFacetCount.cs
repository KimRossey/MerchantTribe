using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Catalog
{
    public class ProductFacetCount
    {
        public string Key { get; set; }
        public int ProductCount { get; set; }

        public ProductFacetCount()
        {
            Key = string.Empty;
            ProductCount = 0;
        }

        public static int FindCountForKey(string key, List<ProductFacetCount> counts)
        {
            string sqlKey = CategoryFacetKeyHelper.ParseKeyToSqlList(key);
            return FindCountForSqlKey(sqlKey, counts);
        }

        public static int FindCountForSqlKey(string sqlKey, List<ProductFacetCount> counts)
        {
            foreach (ProductFacetCount c in counts)
            {
                if (c.Key == sqlKey)
                {
                    return c.ProductCount;
                }
            }

            return 0;
        }
    }
}
