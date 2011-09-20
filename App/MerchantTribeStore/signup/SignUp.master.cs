using MerchantTribe.Commerce;
using System;

namespace MerchantTribeStore
{

    partial class signup_SignUp : System.Web.UI.MasterPage
    {

        public string PhoneNumber { get; set; }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            string payPalLead = SessionManager.GetCookieString("PayPalLead");
            if (!String.IsNullOrEmpty(payPalLead))
            {
                // PayPal Phone Number
                PhoneNumber = "1-877-896-0295";
            }
            else
            {
                // Main Phone Number
                PhoneNumber = "1-804-476-0030";
            }
        }
    }

}