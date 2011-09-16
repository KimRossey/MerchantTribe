using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Shipping
{
    public interface IShippingRate
    {
        decimal EstimatedCost { get; set; }
        string DisplayName { get; set; }
        string ServiceId { get; set; }
        string ServiceCodes { get; set; }

    }
}
