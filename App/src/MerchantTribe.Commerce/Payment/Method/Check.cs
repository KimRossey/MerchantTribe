using System;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Payment.Method
{

    public class Check : DisplayPaymentMethod
    {
        public static string Id() { return "494A61C8-D7E7-457f-B293-4838EF010C32"; }
        public override string MethodId
        {
            get { return Id(); }
        }

        public override string MethodName
        {
            get { return "Check"; }
        }
    }
}

