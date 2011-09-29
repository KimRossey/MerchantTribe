using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Web;

namespace MerchantTribeStore.Areas.signup.Controllers
{
    public class RegisterController : BaseSignupController
    {
        //
        // GET: /signup/Register/

        public ActionResult Index()
        {
            return View();
        }

        private class JsonCheckStoreNameRequest
        {
            public string storename { get; set; }
        }
        private class JsonOut
        {
            public string cleanstorename { get; set; }
            public string message { get; set; }
        }

        public ActionResult JsonCheckStoreName()
        {                        
            JsonCheckStoreNameRequest data = MerchantTribe.Web.Json.ObjectFromJson<JsonCheckStoreNameRequest>(Request.InputStream);

            string clean = "";
            if (data != null)
            {
                clean = data.storename;
                clean = Text.ForceAlphaNumericOnly(clean);
            }
            string msg = "";
            if (MTApp.AccountServices.StoreNameExists(clean))
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

            return new MerchantTribeStore.Controllers.PreJsonResult(json);            
        }

    }
}
