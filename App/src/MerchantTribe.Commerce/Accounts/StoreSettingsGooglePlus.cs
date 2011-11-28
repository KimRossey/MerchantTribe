using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts
{
    public class StoreSettingsGooglePlus
    {
        private StoreSettings parent = null;

        public StoreSettingsGooglePlus(StoreSettings s)
        {
            parent = s;
        }
        
        public bool UseGooglePlus
        {
            get { return parent.GetPropBoolWithDefault("UseGooglePlus", true); }
            set { parent.SetProp("UseGooglePlus", value); }
        }
    }
}
