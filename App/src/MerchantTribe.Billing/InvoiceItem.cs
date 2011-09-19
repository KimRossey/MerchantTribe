using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Billing
{
    public class InvoiceItem
    {
        public string Sku { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal
        {
            get { return UnitPrice * Quantity; }
        }
        public bool Taxable { get; set; }

        public InvoiceItem()
        {
            Sku = string.Empty;
            Description = string.Empty;
            Quantity = 1;
            UnitPrice = 0m;
            Taxable = false;
        }

    }
}
