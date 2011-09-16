using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Shipping.Services
{
    public class FlatRatePerOrderSettings: ServiceSettings
    {
        public decimal Amount
        {
           get {return GetDecimalSetting("Amount");}
           set { SetDecimalSetting("Amount", value);}
        }

    }
}
