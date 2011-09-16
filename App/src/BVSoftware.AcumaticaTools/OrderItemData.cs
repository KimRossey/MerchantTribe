using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.AcumaticaTools
{
    public class OrderItemData
    {
        public decimal Quantity { get; set; }
        public decimal? LineTotal { get; set; }
        public ProductData Product { get; set; }

        public OrderItemData()
        {
            LineTotal = 0;
            Product = new ProductData();
            Quantity = 0;
        }
    }
}
