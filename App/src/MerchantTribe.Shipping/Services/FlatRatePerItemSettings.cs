using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.Services
{
    public class FlatRatePerItemSettings : ServiceSettings
    {
        public decimal Amount
        {
            get { return GetDecimalSetting("Amount"); }
            set { SetDecimalSetting("Amount", value); }
        }

    }
}
