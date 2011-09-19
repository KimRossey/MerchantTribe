using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts
{
    public class StoreIntegrationPaymentMapping
    {
        public string BVPaymentMethodId { get; set; }
        public string BVCardType { get; set; }
        public string IntegrationPaymentMethodId { get; set; }

        public StoreIntegrationPaymentMapping()
        {
            BVPaymentMethodId = string.Empty;
            BVCardType = string.Empty;
            IntegrationPaymentMethodId = string.Empty;
        }

        public string Serialize()
        {
            string output = BVPaymentMethodId + "," + BVCardType + "," + IntegrationPaymentMethodId;
            return output;
        }
        public bool Deserialize(string data)
        {
            string[] parts = data.Split(',');
            if (parts.Length > 2)
            {
                BVPaymentMethodId = parts[0];
                BVCardType = parts[1];
                IntegrationPaymentMethodId = parts[2];
                return true;
            }
            return false;
        }
    }

}
