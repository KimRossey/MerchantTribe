using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.USPostal.v4
{
    public enum DomesticSpecialServiceType
    {
        NotSet = -1,
        Certified=0,
        Insurance=1,
        RestrictedDelivery=3,
        RegisteredWithoutInsurance=4,
        RegisteredWithInsurance=5,
        CollectOnDelivery=6,
        ReturnReceiptForMerchandise=7,
        ReturnReceipt=8,
        CertificateOfMailingArticles=9,
        CertificateOfMailingBooks=10,
        ExpressMailInsurance=11,
        DeliveryConfirmation=13,
        SignatureConfirmation=15,
        ReturnReceiptElectronic=16
    }
}
