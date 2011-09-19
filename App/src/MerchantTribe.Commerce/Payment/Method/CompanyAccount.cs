using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Payment.Method
{
    public class CompanyAccount : DisplayPaymentMethod
    {
        public static string Id() { return "43AE5D2D-A62B-4EB3-BAAF-176EB509C9B5"; }
        public override string MethodId
        {
            get { return Id(); }
        }

        public override string MethodName
        {
            get { return "Company Account"; }
        }
    }
}
