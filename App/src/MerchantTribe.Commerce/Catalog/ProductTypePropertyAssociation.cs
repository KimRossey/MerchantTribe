using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Catalog
{
    public class ProductTypePropertyAssociation
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public string ProductTypeBvin { get; set; }
        public long PropertyId { get; set; }
        public int SortOrder { get; set; }

        public ProductTypePropertyAssociation()
        {
            this.Id = 0;
            this.StoreId = 0;
            this.ProductTypeBvin = string.Empty;
            this.PropertyId = 0;
            this.SortOrder = 0;
        }
    }
}
