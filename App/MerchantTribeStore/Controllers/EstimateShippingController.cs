using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using BVCommerce.Models;
using MerchantTribe.Web.Geography;
using System.Text;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce.Utilities;

namespace BVCommerce.Controllers
{
    public class EstimateShippingController : Shared.BaseStoreController
    {

        private EstimateShippingViewModel BuildViewModel()
        {            
            EstimateShippingViewModel model = new EstimateShippingViewModel();
            ViewBag.GetRatesButton = this.BVApp.ThemeManager().ButtonUrl("GetRates", Request.IsSecureConnection);
            ViewBag.Countries = BVApp.CurrentStore.Settings.FindActiveCountries();

            if (SessionManager.CurrentUserHasCart())
            {
                Order basket = SessionManager.CurrentShoppingCart(BVApp.OrderServices);
                if (basket.ShippingAddress.CountryBvin != string.Empty)
                {
                    model.CountryId = basket.ShippingAddress.CountryBvin;                    
                }
                if (basket.ShippingAddress.RegionBvin != string.Empty)
                {
                    model.RegionId = basket.ShippingAddress.RegionBvin;                    
                }
                model.City = basket.ShippingAddress.City;
                model.PostalCode = basket.ShippingAddress.PostalCode;                                
            }

            return model;
        }

        //
        // GET: /EstimateShipping/
        public ActionResult Index()
        {
            EstimateShippingViewModel model = BuildViewModel();
            GetRates(model);            

            // Populate Data for DropDownLists
            Country currentCountry = Country.FindByBvin(model.CountryId);
            if (currentCountry != null) ViewBag.Regions = currentCountry.Regions;

            return View(model);
        }

        [HttpPost]
        [ActionName("Index")]        
        public ActionResult IndexPost(EstimateShippingViewModel posted)
        {
            EstimateShippingViewModel model = BuildViewModel();
            if (posted != null)
            {
                model.PostalCode = posted.PostalCode;
                model.CountryId = posted.CountryId;
                model.RegionId = posted.RegionId;
            }
            GetRates(model);

            return View(model);
        }

        private class JsonResponse
        {
            public string Regions = "<option value=\"\">- Not Required -</option>";
        }

        [HttpPost]            
        public ActionResult GetRegions(string id)
        {
            string regionid = Request.Form["regionid"];

            JsonResponse result = new JsonResponse();            
            Country c = Country.FindByBvin(id);
            if ((c != null))
            {
                StringBuilder sb = new StringBuilder();
                result.Regions = string.Empty;

                foreach (Region r in c.Regions)
                {
                    sb.Append("<option ");
                    if (r.Abbreviation == regionid)
                    {
                        sb.Append(" selected=\"selected\" ");
                    }
                    sb.Append(" value=\"" + r.Abbreviation + "\">" + r.Name + "</option>");
                }

                result.Regions = sb.ToString();
            }

            return new JsonResult() { Data = result };
        }

        private void GetRates(EstimateShippingViewModel model)
        {
            if (SessionManager.CurrentUserHasCart() == true)
            {
                Order Basket = SessionManager.CurrentShoppingCart(BVApp.OrderServices);
                if (Basket != null)
                {
                    Basket.ShippingAddress.PostalCode = model.PostalCode;
                    
                    if (model.CountryId != string.Empty)
                    {
                        Country current = MerchantTribe.Web.Geography.Country.FindByBvin(model.CountryId);
                        if (current != null)
                        {
                            Basket.ShippingAddress.CountryBvin = model.CountryId;
                            Basket.ShippingAddress.CountryName = current.DisplayName;
                            Basket.ShippingAddress.RegionBvin = model.RegionId;
                            Basket.ShippingAddress.RegionName = model.RegionId;
                        }                        
                    }                                                            

                    BVApp.OrderServices.Orders.Update(Basket);

                    SortableCollection<ShippingRateDisplay> Rates;
                    Rates = BVApp.OrderServices.FindAvailableShippingRates(Basket);

                    if (Rates.Count < 1)
                    {
                        TempData["message"] = "Unable to estimate at this time";
                    }                    
                    model.Rates = Rates.ToList();
                }
            }
        }
    }
}
