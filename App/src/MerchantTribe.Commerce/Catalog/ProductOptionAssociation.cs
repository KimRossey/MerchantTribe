using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Catalog
{
    public class ProductOptionAssociation
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public string ProductBvin { get; set; }
        public string OptionBvin { get; set; }
        public int SortOrder { get; set; }

        public ProductOptionAssociation()
        {
            Id = 0;
            StoreId = 0;
            ProductBvin = string.Empty;
            OptionBvin = string.Empty;
            SortOrder = 0;
        }
    }
}
