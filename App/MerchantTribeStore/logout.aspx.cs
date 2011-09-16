using System;
using System.Web.UI;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Utilities;

namespace BVCommerce
{

    partial class logout : BaseStorePage
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            BVApp.MembershipServices.LogoutCustomer(this.Request.RequestContext.HttpContext);
            Response.Redirect("~/");
                                    
        }

    }
}