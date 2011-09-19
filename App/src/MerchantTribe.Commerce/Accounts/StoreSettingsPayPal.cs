using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts
{
    public class StoreSettingsPayPal
    {
        private StoreSettings parent = null;

        public StoreSettingsPayPal(StoreSettings s)
        {
            parent = s;
        }

        public string UserName
        {
            get { return parent.GetPropEncrypted("PaypalUserName"); }
            set { parent.SetPropEncrypted("PaypalUserName", value); }
        }
        public string Password
        {
            get { return parent.GetPropEncrypted("PaypalPassword"); }
            set { parent.SetPropEncrypted("PaypalPassword", value); }
        }
        public string Signature
        {
            get { return parent.GetPropEncrypted("PaypalSignature"); }
            set { parent.SetPropEncrypted("PaypalSignature", value); }
        }
        public bool ExpressAuthorizeOnly
        {
            get { return parent.GetPropBool("PaypalExpressAuthorizeOnly"); }
            set { parent.SetProp("PaypalExpressAuthorizeOnly", value); }
        }
        public bool AllowUnconfirmedAddresses
        {
            get { return parent.GetPropBool("PaypalAllowUnconfirmedAddresses"); }
            set { parent.SetProp("PaypalAllowUnconfirmedAddresses", value); }
        }
        public string Mode
        {
            get { return parent.GetProp("PaypalMode"); }
            set { parent.SetProp("PaypalMode", value); }
        }
        public string Currency
        {
            get { return parent.GetProp("PaypalCurrency"); }
            set { parent.SetProp("PaypalCurrency", value); }
        }
        public string FastSignupEmail
        {
            get { return parent.GetProp("PayPalFastSignupEmail"); }
            set { parent.SetProp("PayPalFastSignupEmail", value); }
        }
    }
}
