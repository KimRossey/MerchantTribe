using System;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Payment.Method
{

    public class CreditCard : DisplayPaymentMethod
    {
        public static string Id()
        {
            return "4A807645-4B9D-43f1-BC07-9F233B4E713C";
        }

        public override string MethodId
        {
            get { return CreditCard.Id(); }
        }
        public override string MethodName
        {
            get { return "Credit Card"; }
        }    
    }
}
