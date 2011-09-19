using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts
{
    public class StoreIntegrationShippingMapping
    {
        public string BVShippingMethodId { get; set; }
        public string BVServiceCode { get; set; }
        public string IntegrationShippingMethodId { get; set; }

        public StoreIntegrationShippingMapping()
        {
            BVShippingMethodId = string.Empty;
            BVServiceCode = string.Empty;
            IntegrationShippingMethodId = string.Empty;
        }

        public string Serialize()
        {
            string output = BVShippingMethodId + "," + BVServiceCode + "," + IntegrationShippingMethodId;
            return output;
        }
        public bool Deserialize(string data)
        {
            string[] parts = data.Split(',');
            if (parts.Length > 2)
            {
                BVShippingMethodId = parts[0];
                BVServiceCode = parts[1];
                IntegrationShippingMethodId = parts[2];
                return true;
            }
            return false;
        }
    }
}
