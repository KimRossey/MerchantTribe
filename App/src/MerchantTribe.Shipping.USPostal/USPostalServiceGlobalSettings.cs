using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.USPostal
{
    public class USPostalServiceGlobalSettings
    {
        public bool DiagnosticsMode {get;set;}
        public bool IgnoreDimensions {get;set;}

        public USPostalServiceGlobalSettings()
        {
            DiagnosticsMode = false;
            IgnoreDimensions = true;
        }
    }
}
