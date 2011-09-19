using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Membership;

namespace BVCommerce.json
{
    public partial class IsEmailKnown : BaseStoreJsonPage
    {

        public override bool RequiresSSL
        {
            get { return true; }
        }

        public class JsonResponse
        {
            public string success = "0";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            JsonResponse result = new JsonResponse();

            string email = Request.Form["email"];
            CustomerAccount act = BVApp.MembershipServices.Customers.FindByEmail(email);
            if (act != null)
            {
                if (act.Bvin != string.Empty)
                {
                    result.success = "1";
                }
            }

            this.litOutput.Text = MerchantTribe.Web.Json.ObjectToJson(result);
        }

    }
}