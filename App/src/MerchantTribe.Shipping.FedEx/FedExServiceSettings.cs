using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.FedEx
{
    public class FedExServiceSettings : MerchantTribe.Shipping.ServiceSettings
    {
        public int Packaging
        {
            get { return GetIntegerSetting("ShippingFedExPackaging"); }
            set { SetIntegerSetting("ShippingFedExPackaging", value); }
        }
        public int ServiceCode
        {
            get { return GetIntegerSetting("ShippingFedExServiceCode"); }
            set { SetIntegerSetting("ShippingFedExServiceCode", value); }
        }
    }
}
