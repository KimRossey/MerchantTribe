using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.AcumaticaTools
{
    public class OrderShipmentItem
    {
        public string ItemId { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }

        public OrderShipmentItem()
        {
            ItemId = string.Empty;
            Description = string.Empty;
            Quantity = 0;
        }
    }
}
