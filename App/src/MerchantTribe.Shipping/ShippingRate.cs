using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping
{
    public class ShippingRate: IShippingRate
    {

        public decimal EstimatedCost {get;set;}
        public string DisplayName {get;set;}
        public string ServiceId {get;set;}
        public string ServiceCodes {get;set;}

        public ShippingRate()
        {
            EstimatedCost = 0;
            DisplayName = string.Empty;
            ServiceId = string.Empty;
            ServiceCodes = string.Empty;           
        }

    }
}
