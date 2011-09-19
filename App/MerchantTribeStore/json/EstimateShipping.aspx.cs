
using System;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Web.Geography;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce.Utilities;

namespace BVCommerce
{
    partial class json_EstimateShipping : BaseStoreJsonPage
    {

        public override bool RequiresSSL
        {
            get { return true; }
        }

        public class JsonResponse
        {
            public string totalsastable = string.Empty;
            public string rates = "";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            JsonResponse result = new JsonResponse();

            string country = Request.Form["country"];
            string firstname = Request.Form["firstname"];
            string lastname = Request.Form["lastname"];
            string address = Request.Form["address"];
            string city = Request.Form["city"];
            string state = Request.Form["state"];
            string zip = Request.Form["zip"];
            string orderid = Request.Form["orderid"];

            Order o = MTApp.OrderServices.Orders.FindForCurrentStore(orderid);
            o.ShippingAddress.FirstName = firstname;
            o.ShippingAddress.LastName = lastname;
            o.ShippingAddress.Line1 = address;
            o.ShippingAddress.City = city;
            o.ShippingAddress.PostalCode = zip;
            Country c = Country.FindByBvin(country);
            if ((c != null))
            {
                o.ShippingAddress.CountryBvin = country;
                o.ShippingAddress.CountryName = c.DisplayName;
                foreach (Region r in c.Regions)
                {
                    if ((r.Abbreviation == state))
                    {
                        o.ShippingAddress.RegionBvin = r.Abbreviation;
                        o.ShippingAddress.RegionName = r.Name;
                    }
                }
            }

            SortableCollection<ShippingRateDisplay> rates = MTApp.OrderServices.FindAvailableShippingRates(o);

            string rateKey = o.ShippingMethodUniqueKey;
            bool rateIsAvailable = false;

            // See if rate is available
            if ((rateKey.Length > 0))
            {
                foreach (MerchantTribe.Commerce.Shipping.ShippingRateDisplay r in rates)
                {
                    if ((r.UniqueKey == rateKey))
                    {
                        rateIsAvailable = true;
                        MTApp.OrderServices.OrdersRequestShippingMethod(r, o);                        
                    }
                }
            }

            // if it's not availabe, pick the first one or default
            if ((rateIsAvailable == false))
            {
                if ((rates.Count > 0))
                {
                    MTApp.OrderServices.OrdersRequestShippingMethod(rates[0], o);
                    rateKey = rates[0].UniqueKey;
                }
                else
                {
                    o.ClearShippingPricesAndMethod();
                }
            }

            result.rates = HtmlRendering.ShippingRatesToRadioButtons(rates, 300, o.ShippingMethodUniqueKey);

            MTApp.CalculateOrderAndSave(o);
            SessionManager.SaveOrderCookies(o);

            result.totalsastable = o.TotalsAsTable();

            //System.Threading.Thread.Sleep(500)
            this.litOutput.Text = MerchantTribe.Web.Json.ObjectToJson(result);
        }

    }
}