using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVSoftware.Commerce;

namespace BVCommerce
{

    public partial class signup_PayPalOffer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "Online Stores Hosted Shopping Carts | PayPal";
            this.MetaDescription = "Special offer for PayPal Shopping Cart Users and PayPal Online Store Customers";
            this.MetaKeywords = "PayPal, Ecommerce, BV Commerce, BVCommerce, hosted, shopping cart, carts, shopping, stores";

            ((signup_SignUp)this.Master).PhoneNumber = "1-877-896-0295";

            SessionManager.SetCookieString("PayPalLead", "PayPalLead");
        }
    }
}