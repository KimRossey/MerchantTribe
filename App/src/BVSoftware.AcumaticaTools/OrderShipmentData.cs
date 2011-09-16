using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools.Acumatica;

namespace BVSoftware.AcumaticaTools
{
    public class OrderShipmentData
    {
        public string ShipmentNumber { get; set; }
        public decimal QuantityShipped { get; set; }
        public string StatusName { get; set; }
        public string StatusCode { get; set; }
        public DateTime? ShipDate { get; set; }
        public List<string> TrackingNumber { get; set; }
        public List<OrderShipmentItem> Items { get; set; }

        public OrderShipmentData()
        {
            Init();
        }

        private void Init()
        {
            this.QuantityShipped = 0;
            this.ShipDate = null;
            this.ShipmentNumber = string.Empty;
            this.StatusCode = string.Empty;
            this.StatusName = string.Empty;
            this.TrackingNumber = new List<string>();
            this.Items = new List<OrderShipmentItem>();
        }
     
    }
}
