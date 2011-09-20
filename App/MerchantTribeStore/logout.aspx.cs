using System;
using System.Web.UI;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore
{

    partial class logout : BaseStorePage
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            MTApp.MembershipServices.LogoutCustomer(this.Request.RequestContext.HttpContext);
            Response.Redirect("~/");
                                    
        }

    }
}