using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts
{
    public class StoreSettingsFaceBook
    {
        private StoreSettings parent = null;

        public StoreSettingsFaceBook(StoreSettings s)
        {
            parent = s;
        }
        
        public bool UseFaceBook
        {
            get { return parent.GetPropBoolWithDefault("UseFaceBook", true); }
            set { parent.SetProp("UseFaceBook", value); }
        }
        public string AppId
        {
            get { return parent.GetProp("FaceBookAppId"); }
            set { parent.SetProp("FaceBookAppId", value); }
        }
        public string Admins
        {
            get { return parent.GetProp("FaceBookAdmins"); }
            set { parent.SetProp("FaceBookAdmins", value); }
        }
    }
}
