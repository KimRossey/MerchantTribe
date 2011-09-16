using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.AcumaticaTools
{
    public class OrderSummaryData
    {
        public string CustomerId { get; set; }
        public string Number { get; set; }
        public DateTime TimeOfOrder { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
    }
}
