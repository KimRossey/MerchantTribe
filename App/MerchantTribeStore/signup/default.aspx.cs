using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BVCommerce
{

    public partial class signup_default : BaseSignupPage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Title = "BV Commerce Pricing & Sign Up : Online hosted stores and ecommerce software";

            bool IsPayPalLead = false;
            if (MerchantTribe.Commerce.SessionManager.GetCookieString("PayPalLead") != string.Empty)
            {
                IsPayPalLead = true;
                this.trCC.Visible = false;
                this.trPO.Visible = false;
                this.trCOD.Visible = false;
            }

            ViewData["IsPayPalLead"] = IsPayPalLead;
        }
    }
}