using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Membership;
using MerchantTribeStore.Controllers.Shared;
using MerchantTribe.Commerce.Orders;
using MerchantTribeStore.Filters;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Orders;

namespace MerchantTribeStore.Controllers
{
    public class CheckoutController : BaseStoreController
    {
        //GET: /checkout/receipt
        [NonCacheableResponseFilter]
        public ActionResult Receipt()
        {
            ViewBag.Title = "Order Receipt";
            ViewBag.BodyClass = "store-receipt-page";
            ViewBag.MetaDescription = "Order Receipt";
            LoadOrder();            
            return View();           
        }
        private void LoadOrder()
        {
            if (Request.Params["id"] != null)
            {
                Order o = MTApp.OrderServices.Orders.FindForCurrentStore(Request.Params["id"]);
                if (o == null)
                {
                    FlashFailure("Order could not be found. Please contact store for assistance.");
                    return;
                }
                                                                               
                ViewBag.Order = o;
                ViewBag.AcumaticaOrder = null;

                OrderPaymentSummary paySummary = MTApp.OrderServices.PaymentSummary(o);
                ViewBag.OrderPaymentSummary = paySummary;

                // File Downloads
                List<ProductFile> fileDownloads = new List<ProductFile>();
                if ((o.PaymentStatus == OrderPaymentStatus.Paid) && (o.StatusCode != OrderStatusCode.OnHold))
                {
                    foreach (LineItem item in o.Items)
                    {
                        if (item.ProductId != string.Empty)
                        {
                            List<ProductFile> productFiles = MTApp.CatalogServices.ProductFiles.FindByProductId(item.ProductId);
                            foreach (ProductFile file in productFiles)
                            {
                                fileDownloads.Add(file);
                            }
                        }
                    }
                }
                ViewBag.FileDownloads = fileDownloads;
                ViewBag.FileDownloadsButtonUrl = MTApp.ThemeManager().ButtonUrl("download", Request.IsSecureConnection);

                RenderAnalytics(o);
            }
            else
            {
                FlashFailure("Order Number missing. Please contact an administrator.");
                return;
            }            
        }
        private void RenderAnalytics(Order o)
        {

            // Reset Analytics for receipt page
            this.ViewData["analyticstop"] = string.Empty;

            // Add Tracker and Maybe Ecommerce Tracker to Top
            if (MTApp.CurrentStore.Settings.Analytics.UseGoogleTracker)
            {
                if (MTApp.CurrentStore.Settings.Analytics.UseGoogleEcommerce)
                {
                    // Ecommerce + Page Tracker
                    this.ViewData["analyticstop"] = MerchantTribe.Commerce.Metrics.GoogleAnalytics.RenderLatestTrackerAndTransaction(
                        MTApp.CurrentStore.Settings.Analytics.GoogleTrackerId,
                        o,
                        MTApp.CurrentStore.Settings.Analytics.GoogleEcommerceStoreName,
                        MTApp.CurrentStore.Settings.Analytics.GoogleEcommerceCategory);
                }
                else
                {
                    // Page Tracker Only
                    this.ViewData["analyticstop"] = MerchantTribe.Commerce.Metrics.GoogleAnalytics.RenderLatestTracker(MTApp.CurrentStore.Settings.Analytics.GoogleTrackerId);
                }
            }


            // Clear Bottom Analytics Tags
            this.ViewData["analyticsbottom"] = string.Empty;

            // Adwords Tracker at bottom if needed
            if (MTApp.CurrentStore.Settings.Analytics.UseGoogleAdWords)
            {
                this.ViewData["analyticsbottom"] = MerchantTribe.Commerce.Metrics.GoogleAnalytics.RenderGoogleAdwordTracker(
                                                        o.TotalGrand,
                                                        MTApp.CurrentStore.Settings.Analytics.GoogleAdWordsId,
                                                        MTApp.CurrentStore.Settings.Analytics.GoogleAdWordsLabel,
                                                        MTApp.CurrentStore.Settings.Analytics.GoogleAdWordsBgColor,
                                                        Request.IsSecureConnection);
            }

            // Add Yahoo Tracker to Bottom if Needed
            if (MTApp.CurrentStore.Settings.Analytics.UseYahooTracker)
            {
                this.ViewData["analyticsbottom"] += MerchantTribe.Commerce.Metrics.YahooAnalytics.RenderYahooTracker(
                    o, MTApp.CurrentStore.Settings.Analytics.YahooAccountId);
            }
        }

        [HttpPost] // POST: /checkout/applyshipping
        public ActionResult ApplyShipping()
        {
            ApplyShippingResponse result = new ApplyShippingResponse();

            string rateKey = Request.Form["MethodId"];
            string orderid = Request.Form["OrderId"];
            if (rateKey == null)
            {
                rateKey = "";
            }
            if (orderid == null)
            {
                orderid = "";
            }


            Order o = MTApp.OrderServices.Orders.FindForCurrentStore(orderid);
            MTApp.OrderServices.OrdersRequestShippingMethodByUniqueKey(rateKey, o);
            MTApp.CalculateOrderAndSave(o);
            SessionManager.SaveOrderCookies(o);

            result.totalsastable = o.TotalsAsTable();

            //System.Threading.Thread.Sleep(500)
            return new PreJsonResult(MerchantTribe.Web.Json.ObjectToJson(result));
        }
        public class ApplyShippingResponse
        {
            public string totalsastable = string.Empty;
        }        
                
        [HttpPost] // POST: /checkout/cleancreditcard
        public ActionResult CleanCreditCard()
        {
            CleanCreditCardResponse result = new CleanCreditCardResponse();
            string notclean = Request.Form["CardNumber"];
            result.CardNumber = MerchantTribe.Payment.CardValidator.CleanCardNumber(notclean);
            return new PreJsonResult(MerchantTribe.Web.Json.ObjectToJson(result));
        }
        public class CleanCreditCardResponse
        {
            public string CardNumber = "";
        }

        [HttpPost] // POST: /checkout/IsEmailKnown
        public ActionResult IsEmailKnown()
        {
            IsEmailKnownResponse result = new IsEmailKnownResponse();
            string email = Request.Form["email"];
            CustomerAccount act = MTApp.MembershipServices.Customers.FindByEmail(email);
            if (act != null)
            {
                if (act.Bvin != string.Empty)
                {
                    result.success = "1";
                }
            }
            return new PreJsonResult(MerchantTribe.Web.Json.ObjectToJson(result));
        }
        public class IsEmailKnownResponse
        {
            public string success = "0";
        }        
    }
}
