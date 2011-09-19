using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.Services
{
    public class RatePerWeightFormulaSettings : ServiceSettings
    {
        public decimal BaseAmount
        {
            get { return GetDecimalSetting("BaseAmount"); }
            set { SetDecimalSetting("BaseAmount", value); }
        }

        public decimal BaseWeight
        {
            get { return GetDecimalSetting("BaseWeight"); }
            set { SetDecimalSetting("BaseWeight", value); }
        }

        public decimal AdditionalWeightCharge
        {
            get { return GetDecimalSetting("AdditionalWeightCharge"); }
            set { SetDecimalSetting("AdditionalWeightCharge", value); }
        }

        public decimal MinWeight
        {
            get { return GetDecimalSetting("MinWeight"); }
            set { SetDecimalSetting("MinWeight", value); }
        }
        public decimal MaxWeight
        {
            get { return GetDecimalSetting("MaxWeight"); }
            set { SetDecimalSetting("MaxWeight", value); }
        }

    }
}

