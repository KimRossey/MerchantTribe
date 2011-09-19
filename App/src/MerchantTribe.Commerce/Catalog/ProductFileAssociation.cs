using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Catalog
{
    public class ProductFileAssociation
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public string FileId { get; set; }
        public string ProductId { get; set; }
        public int AvailableMinutes { get; set; }
        public int MaxDownloads { get; set; }
        public DateTime LastUpdatedUtc { get; set; }

        public ProductFileAssociation()
        {
            Id = 0;
            StoreId = 0;
            FileId = string.Empty;
            ProductId = string.Empty;
            AvailableMinutes = 0;
            MaxDownloads = 0;
            LastUpdatedUtc = DateTime.UtcNow;
        }
    }

    
}
