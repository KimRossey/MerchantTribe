using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MerchantTribeStore
{

    public partial class signup_about : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "About MerchantTribe | Free Open Source Online Stores & Hosted Shopping Carts";
            this.MetaDescription = "About MerchantTribe, makers of hosted shopping cart software and solutions in Richmond, VA. Small business ecommerce solutions by BV Software including BV Commerce.";
            this.MetaKeywords = "MerchantTribe, BV Software, BVSoftware, BV Commerce, BVCommerce, About, Richmond, Virginia, VA, Small Business, Ecommerce, Hosted, Shopping, Cart";
        }
    }
}