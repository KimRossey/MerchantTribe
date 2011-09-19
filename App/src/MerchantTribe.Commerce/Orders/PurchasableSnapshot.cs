using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Orders
{
    public class PurchasableSnapshot
    {
        public string ProductId { get; set; }
        public string VariantId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsTaxExempt { get; set; }
        public long TaxScheduleId { get; set; }
        public Catalog.OptionSelectionList SelectionData {get;set;}
        public Shipping.ShippableItem ShippingDetails { get; set; }
        
        public PurchasableSnapshot()
        {
            this.ProductId = string.Empty;
            this.VariantId = string.Empty;
            this.Name = string.Empty;
            this.Sku = string.Empty;
            this.Description = string.Empty;
            this.BasePrice = 0m;
            this.IsTaxExempt = false;
            this.TaxScheduleId = 0;
            this.SelectionData = new Catalog.OptionSelectionList();
            this.ShippingDetails = new Shipping.ShippableItem();
        }

    }
}
