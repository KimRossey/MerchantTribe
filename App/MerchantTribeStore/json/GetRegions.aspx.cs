
using System;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Content;
using MerchantTribe.Web.Geography;

namespace BVCommerce
{
    partial class json_GetRegions : BaseStoreJsonPage
    {

        public class JsonResponse
        {
            public string Regions = "<option value=\"\">- Not Required -</option>";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            JsonResponse result = new JsonResponse();

            string bvin = Request.Form["bvin"];
            Country c = Country.FindByBvin(bvin);
            if ((c != null))
            {
                result.Regions = string.Empty;
                foreach (Region r in c.Regions)
                {
                    result.Regions += "<option value=\"" + r.Abbreviation + "\">" + r.Name + "</option>";
                }
            }

            this.litOutput.Text = MerchantTribe.Web.Json.ObjectToJson(result);
        }

    }
}