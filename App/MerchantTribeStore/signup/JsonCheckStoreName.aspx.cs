using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Web;
using MerchantTribe.Billing;
using MerchantTribe.Commerce.Accounts;

namespace MerchantTribeStore
{

    public partial class signup_JsonCheckStoreName : System.Web.UI.Page
    {

        private class JsonCheckStoreNameRequest
        {
            public string storename { get; set; }
        }

        private class JsonOut
        {
            public string cleanstorename { get; set; }
            public string message { get; set; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            MerchantTribe.Commerce.RequestContext context = new MerchantTribe.Commerce.RequestContext();
            AccountService accountServices = AccountService.InstantiateForDatabase(context);

            JsonCheckStoreNameRequest data = MerchantTribe.Web.Json.ObjectFromJson<JsonCheckStoreNameRequest>(Request.InputStream);

            string clean = "";
            if (data != null)
            {
                clean = data.storename;
                clean = Text.ForceAlphaNumericOnly(clean);
            }
            string msg = "";
            if (accountServices.StoreNameExists(clean))
            {
                msg = "<div class=\"flash-message-failure\"><strong>" + clean + ".bvcommerce.com</strong><br />Store name is already taken.</div>";
            }
            else
            {
                msg = "<div class=\"flash-message-success\"><strong>" + clean + ".bvcommerce.com</strong><br />Store name is available.</div>";
            }

            if (clean == "")
            {
                msg = "<div class=\"flash-message-watermark\">a store name is required<br />&nbsp;</div>";
            }

            JsonOut result = new JsonOut() { cleanstorename = clean, message = msg };

            string json = MerchantTribe.Web.Json.ObjectToJson(result);

            this.litOutput.Text = json;
        }

    }
}