using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts
{
    public class StoreSettingsTwitter
    {
        private StoreSettings parent = null;

        public StoreSettingsTwitter(StoreSettings s)
        {
            parent = s;
        }
        
        public bool UseTwitter
        {
            get { return parent.GetPropBoolWithDefault("UseTwitter", true); }
            set { parent.SetProp("UseTwitter", value); }
        }
        public string TwitterHandle
        {
            get { return parent.GetProp("TwitterHandle"); }
            set { parent.SetProp("TwitterHandle", value); }
        }
        public string DefaultTweetText
        {
            get { 
                string prop = parent.GetProp("DefaultTweetText");
                if (prop == string.Empty)
                { prop = "Check This Out: {0} "; }
                return prop;
            }
            set { parent.SetProp("DefaultTweetText", value); }
        }
    }
}
