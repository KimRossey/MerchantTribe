using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.USPostal
{
    class USPostalConstants
    {        
        public const decimal MaxBoundPrintedWeightInPounds = 15m;
        public const decimal MaxFirstClassLetterWeightInPounds = 0.21875m;
        public const decimal MaxFirstClassLetterThicknessInInches = 0.25m;
        public const decimal MaxFirstClassFlatThicknessInInches = 0.75m;
        public const decimal MaxFirstClassWeightInPounds = 0.8125m;
        public const decimal MaxWeightInPounds = 70m;

        public const string ApiUrl = "http://production.shippingapis.com/ShippingAPI.dll";
        public const string ApiUsername = "643BVSOF1535";
    }
}
