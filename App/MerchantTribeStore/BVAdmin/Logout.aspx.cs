using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Accounts;

namespace BVCommerce.BVAdmin
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AccountService account = AccountService.InstantiateForDatabase(new MerchantTribe.Commerce.RequestContext());            
            account.LogoutAdminUser(Page.Request.RequestContext.HttpContext);

            Response.Redirect("~/");
        }
    }
}