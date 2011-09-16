using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVSoftware.Commerce.Membership;

namespace BVCommerce.BVAdmin.Configuration
{
    public partial class ShippingUpsThanks : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "UPS Online Tools Registration";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected void btnContinue_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Shipping.aspx", true);
        }
    }
}