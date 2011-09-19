using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Reporting
{
    public class BVNewsMessage
    {
        public long Id { get; set; }
        public DateTime TimeStampUtc { get; set; }
        public string Message { get; set; }
    }
}
