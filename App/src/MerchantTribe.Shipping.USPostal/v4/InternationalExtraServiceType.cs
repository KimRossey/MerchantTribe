using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.USPostal.v4
{
    public enum InternationalExtraServiceType
    {
        NotSet = -1,
        RegisteredMail = 0,
        Insurance = 1,
        ReturnReceipt = 2,
        RestrictedDelivery = 3,
        PickUpOnDemand = 5,
        CertificateOfMailing = 6
    }
}
