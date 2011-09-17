
using System;
using MerchantTribe.Payment;

namespace BVCommerce
{
    partial class json_CleanCreditCard : BaseStoreJsonPage
    {

        public override bool RequiresSSL
        {
            get { return true; }
        }

        public class JsonResponse
        {
            public string CardNumber = "";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            JsonResponse result = new JsonResponse();

            string notclean = Request.Form["CardNumber"];
            result.CardNumber = MerchantTribe.Payment.CardValidator.CleanCardNumber(notclean);

            this.litOutput.Text = MerchantTribe.Web.Json.ObjectToJson(result);
        }

    }
}