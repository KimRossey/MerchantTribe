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
using MerchantTribeStore.Models;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce.BusinessRules;
using MerchantTribe.Web.Logging;

namespace MerchantTribeStore.Controllers
{
    public class CheckoutController : BaseStoreController
    {
        private CheckoutViewModel IndexSetup()
        {
            ViewBag.Title = "Checkout";
            ViewBag.BodyClass = "store-checkout-page";

            CheckoutViewModel model = new CheckoutViewModel();
            LoadOrder(model);
            CheckForPoints(model);

            // Buttons
            ThemeManager themes = MTApp.ThemeManager();
            model.ButtonCheckoutUrl = themes.ButtonUrl("PlaceOrder", Request.IsSecureConnection);
            model.ButtonLoginUrl = MTApp.ThemeManager().ButtonUrl("Login", Request.IsSecureConnection);

            // Labels
            model.LabelRewardPoints = MTApp.CurrentStore.Settings.RewardsPointsName;

            // Agree Checkbox
            if (MTApp.CurrentStore.Settings.ForceTermsAgreement)
            {
                model.ShowAgreeToTerms = true;
                model.AgreedToTerms = false;
                model.AgreedToTermsDescription = SiteTerms.GetTerm(SiteTermIds.TermsAndConditionsAgreement);
                model.LabelTerms = SiteTerms.GetTerm(SiteTermIds.TermsAndConditions);                                
            }
            else
            {
                model.ShowAgreeToTerms = false;
                model.AgreedToTerms = true;
            }

            // Populate Countries
            model.Countries = MTApp.CurrentStore.Settings.FindActiveCountries();

            return model;
        }
        private void LoadOrder(CheckoutViewModel model)
        {
            Order result = SessionManager.CurrentShoppingCart(MTApp.OrderServices);
            if (result == null) Response.Redirect("~/cart");           
            model.CurrentOrder = result;

            if (result.Items.Count == 0)
            {
                Response.Redirect("~/cart");
            }

            // Email
            model.IsLoggedIn = false;
            if (SessionManager.IsUserAuthenticated(this.MTApp))
            {
                model.IsLoggedIn = true;
                model.CurrentCustomer = MTApp.CurrentCustomer;
                if (model.CurrentCustomer != null)
                {
                    model.CurrentOrder.UserEmail = model.CurrentCustomer.Email;
                }
                
                // Copy customer addresses to order
                model.CurrentCustomer.ShippingAddress.CopyTo(model.CurrentOrder.ShippingAddress);                
                if (model.BillShipSame == false)
                {
                    Address billAddr = model.CurrentCustomer.BillingAddress;
                    billAddr.CopyTo(model.CurrentOrder.BillingAddress);
                }                                
            }            
            
            // Payment
            //***************Payment.LoadPaymentMethods(result.TotalGrand);

        }
        void CheckForPoints(CheckoutViewModel model)
        {
            model.ShowRewards = false;

            if (model.CurrentCustomer == null || model.CurrentCustomer.Bvin == string.Empty) return;

            string uid = model.CurrentCustomer.Bvin;
            int points = MTApp.CustomerPointsManager.FindAvailablePoints(uid);
            if (points > 0 && MTApp.CurrentStore.Settings.RewardsPointsOnPurchasesActive)
            {
                model.ShowRewards = true;                
                int potentialPointsToUse = MTApp.CustomerPointsManager.PointsNeededForPurchaseAmount(model.CurrentOrder.TotalOrderAfterDiscounts);
                int amountToUse = 0;
                if (points > potentialPointsToUse)
                {
                    amountToUse = potentialPointsToUse;
                }
                else
                {
                    amountToUse = points;
                }               
                model.RewardPointsAvailable = "You have " + points.ToString() + " " + model.LabelRewardPoints + " available.";
                decimal dollarValue = MTApp.CustomerPointsManager.DollarCreditForPoints(amountToUse);
                model.LabelRewardsUse = "Use " + amountToUse.ToString() + " [" + dollarValue.ToString("C") + "] " + model.LabelRewardPoints;                
            }
        }

        // GET: /checkout
        [NonCacheableResponseFilter]
        public ActionResult Index()
        {
            CheckoutViewModel model = IndexSetup();            
            
            return View(model);
        }

        // POST: /checkout
        [NonCacheableResponseFilter]
        [ActionName("Index")]
        [HttpPost]
        public ActionResult IndexPost()
        {
            CheckoutViewModel model = IndexSetup();
            TagOrderWithAffiliate(model);                        
            LoadValuesFromForm(model);            
            if (ValidateOrder(model))
            {
                ProcessOrder(model);
            }

            // Render Error Summary
            foreach (var v in model.Violations)
            {
                FlashFailure(v.ErrorMessage);
            }

            return View(model);
        }
        private void TagOrderWithAffiliate(CheckoutViewModel model)
        {
            string affid = MTApp.ContactServices.GetValidAffiliateId().ToString();
            if (!string.IsNullOrEmpty(affid))
            {
                model.CurrentOrder.AffiliateID = affid;
            }
        }        
        private void LoadValuesFromForm(CheckoutViewModel model)
        {            
            // Email
            model.CurrentOrder.UserEmail = Request.Form["customeremail"];
                 
            // Addresses            
            model.BillShipSame = (Request.Form["chkbillsame"] != null);            
           
            LoadAddressFromForm("shipping", model.CurrentOrder.ShippingAddress);
            if (model.BillShipSame)
            {
                model.CurrentOrder.ShippingAddress.CopyTo(model.CurrentOrder.BillingAddress);
            }
            else
            {
                LoadAddressFromForm("billing", model.CurrentOrder.BillingAddress);
            }
            // Save addresses to customer account
            if (model.IsLoggedIn)
            {
                
                model.CurrentOrder.ShippingAddress.CopyTo(model.CurrentOrder.ShippingAddress);
                if (model.BillShipSame == false)
                {
                    model.CurrentOrder.BillingAddress.CopyTo(model.CurrentCustomer.BillingAddress);
                }
                MTApp.MembershipServices.Customers.Update(model.CurrentCustomer);
            }

            //Shipping
            string shippingRateKey = Request.Form["shippingrate"];
            MTApp.OrderServices.OrdersRequestShippingMethodByUniqueKey(shippingRateKey, model.CurrentOrder);

            // Save Values so far in case of later errors
            MTApp.CalculateOrder(model.CurrentOrder);

            // Save Payment Information                    
            model.UseRewardsPoints = Request.Form["userewardspoints"] == "1";
            ApplyRewardsPoints(model);

            //***************Payment.SavePaymentInfo(model.CurrentOrder);

            model.CurrentOrder.Instructions = Request.Form["specialinstructions"];

            // Save all the changes to the order
            MTApp.OrderServices.Orders.Update(model.CurrentOrder);
            SessionManager.SaveOrderCookies(model.CurrentOrder);
        }
        private void ApplyRewardsPoints(CheckoutViewModel model)
        {
            // Remove any current points info transactions
            foreach (OrderTransaction t in MTApp.OrderServices.Transactions.FindForOrder(model.CurrentOrder.bvin))
            {
                if (t.Action == MerchantTribe.Payment.ActionType.RewardPointsInfo)
                {
                    MTApp.OrderServices.Transactions.Delete(t.Id);
                }
            }

            // Don't add if we're not using points
            if (!model.UseRewardsPoints) return;

            // Apply Info to Order
            OrderPaymentManager payManager = new OrderPaymentManager(model.CurrentOrder, MTApp);
            payManager.RewardsPointsAddInfo(RewardsPotentialCredit(model));
        }
        private decimal RewardsPotentialCredit(CheckoutViewModel model)
        {
            decimal result = 0;
            if (!model.UseRewardsPoints) return result;

            int points = MTApp.CustomerPointsManager.FindAvailablePoints(model.CurrentCustomer.Bvin);
            int potentialPointsToUse = MTApp.CustomerPointsManager.PointsNeededForPurchaseAmount(model.CurrentOrder.TotalOrderAfterDiscounts);
            int amountToUse = 0;
            if (points > potentialPointsToUse)
            {
                amountToUse = potentialPointsToUse;
            }
            else
            {
                amountToUse = points;
            }
            result = MTApp.CustomerPointsManager.DollarCreditForPoints(amountToUse);
            return result;
        }
        private void LoadAddressFromForm(string prefix, Address address)
        {
            address.Bvin = Request.Form[prefix + "addressbvin"] ?? address.Bvin;
            address.CountryBvin = Request.Form[prefix + "country"] ?? address.CountryBvin;
            address.FirstName = Request.Form[prefix + "firstname"] ?? address.FirstName;
            address.LastName = Request.Form[prefix + "lastname"] ?? address.LastName;
            address.Company = Request.Form[prefix + "company"] ?? address.Company;
            address.Line1 = Request.Form[prefix + "address"] ?? address.Line1;
            address.City = Request.Form[prefix + "city"] ?? address.City;
            address.RegionBvin = Request.Form[prefix + "state"] ?? address.RegionBvin;
            address.PostalCode = Request.Form[prefix + "zip"] ?? address.PostalCode;
            address.Phone = Request.Form[prefix + "phone"] ?? address.Phone;                
        }
        private bool ValidateOrder(CheckoutViewModel model)
        {
            bool result = true;

            if (model.AgreedToTerms == false && model.ShowAgreeToTerms == true)
            {
                model.Violations.Add(new MerchantTribe.Web.Validation.RuleViolation("Terms", "Terms", SiteTerms.GetTerm(SiteTermIds.SiteTermsAgreementError)));
                result = false;
            }

            // Validate Email
            MerchantTribe.Web.Validation.ValidationHelper.ValidEmail("Email Address", model.CurrentOrder.UserEmail, model.Violations, "customeremail");

            // Validate Shipping Address
            model.Violations.AddRange(ValidateAddress(model.CurrentOrder.ShippingAddress, "Shipping"));

            // Validate Billing Address
            if (model.BillShipSame == false)
            {
                model.Violations.AddRange(ValidateAddress(model.CurrentOrder.BillingAddress, "Billing"));
            }

            Order Basket = SessionManager.CurrentShoppingCart(MTApp.OrderServices);
            //Collection<GiftCertificate> gcs = Basket.GetGiftCertificates();
            //decimal totalValue = 0m;
            //foreach (GiftCertificate item in gcs) {
            //    totalValue += item.CurrentAmount;
            //}

            // If Gift Certs are Included in the order, we may not need
            // additional payment information
            bool paymentFound = false;
            //Basket.CalculateGrandTotalOnly(false, false);
            //if ((totalValue >= Basket.TotalGrand)) {
            //	paymentFound = true;
            //}

            // Make sure a shipping method is selected
            // Basket validation checks for shipping method unique key
            if (!Basket.IsValid())
            {
                model.Violations.AddRange(Basket.GetRuleViolations());
            }

            //*******************if ((!Payment.IsValid()) && (!paymentFound))
            //*******************{
            //*******************    model.Violations.AddRange(Payment.GetRuleViolations());
            //*******************}

            if ((model.Violations.Count > 0))
            {
                result = false;
            }
            return result;
        }
        private List<MerchantTribe.Web.Validation.RuleViolation> ValidateAddress(Address a, string prefix)
        {
            List<MerchantTribe.Web.Validation.RuleViolation> result = new List<MerchantTribe.Web.Validation.RuleViolation>();

            string pre = prefix.Trim().ToLowerInvariant();

            MerchantTribe.Web.Validation.ValidationHelper.Required(prefix + " Country Name", a.CountryData.Name, result, pre + "countryname");
            MerchantTribe.Web.Validation.ValidationHelper.Required(prefix + " First Name", a.FirstName, result, pre + "firstname");
            MerchantTribe.Web.Validation.ValidationHelper.Required(prefix + " Last Name", a.LastName, result, pre + "lastname");
            MerchantTribe.Web.Validation.ValidationHelper.Required(prefix + " Street", a.Line1, result, pre + "address");
            MerchantTribe.Web.Validation.ValidationHelper.Required(prefix + " City", a.City, result, pre + "city");
            MerchantTribe.Web.Validation.ValidationHelper.Required(prefix + " Postal Code", a.PostalCode, result, pre + "zip");

            MerchantTribe.Web.Validation.ValidationHelper.Required(prefix + " Region/State",
                                                                a.RegionData.Abbreviation,
                                                                result, pre + "state");
            return result;
        }
        private void ProcessOrder(CheckoutViewModel model)
        {
            // Save as Order
            OrderTaskContext c = new OrderTaskContext(MTApp);
            c.UserId = SessionManager.GetCurrentUserId();
            c.Order = model.CurrentOrder;

            // Check for PayPal Request
            //OrderPaymentManager payManager = new OrderPaymentManager(Basket);

            bool paypalCheckoutSelected = false; //************* this.Payment.PayPalSelected;
            
            //bool paypalCheckoutSelected = payManager.PayPalExpressHasInfo();

            if (paypalCheckoutSelected)
            {
                c.Inputs.Add("bvsoftware", "Mode", "PaypalExpress");
                c.Inputs.Add("bvsoftware", "AddressSupplied", "1");
                if (!Workflow.RunByName(c, WorkflowNames.ThirdPartyCheckoutSelected))
                {
                    EventLog.LogEvent("Paypal Express Checkout Failed", "Specific Errors to follow", EventLogSeverity.Error);
                    // Show Errors     
                    List<MerchantTribe.Web.Validation.RuleViolation> violations = new List<MerchantTribe.Web.Validation.RuleViolation>();
                    foreach (WorkflowMessage item in c.GetCustomerVisibleErrors())
                    {
                        violations.Add(new MerchantTribe.Web.Validation.RuleViolation("Workflow", item.Name, item.Description));
                    }
                }
            }
            else
            {
                if (Workflow.RunByName(c, WorkflowNames.ProcessNewOrder) == true)
                {
                    // Clear Cart ID because we're now an order
                    SessionManager.CurrentCartID = string.Empty;

                    // Process Payment
                    if (MerchantTribe.Commerce.BusinessRules.Workflow.RunByName(c, MerchantTribe.Commerce.BusinessRules.WorkflowNames.ProcessNewOrderPayments))
                    {
                        MerchantTribe.Commerce.BusinessRules.Workflow.RunByName(c, MerchantTribe.Commerce.BusinessRules.WorkflowNames.ProcessNewOrderAfterPayments);
                        Order tempOrder = MTApp.OrderServices.Orders.FindForCurrentStore(model.CurrentOrder.bvin);
                        MerchantTribe.Commerce.Integration.Current().OrderReceived(tempOrder, MTApp);
                        Response.Redirect("~/checkout/receipt?id=" + model.CurrentOrder.bvin);
                    }
                    else
                    {
                        // Redirect to Payment Error
                        SessionManager.CurrentPaymentPendingCartId = model.CurrentOrder.bvin;
                        Response.Redirect("~/checkout/paymenterror");
                    }
                }
                else
                {
                    // Show Errors      
                    List<MerchantTribe.Web.Validation.RuleViolation> violations = new List<MerchantTribe.Web.Validation.RuleViolation>();
                    foreach (WorkflowMessage item in c.GetCustomerVisibleErrors())
                    {
                        violations.Add(new MerchantTribe.Web.Validation.RuleViolation("Workflow", item.Name, item.Description));
                    }
                    if (violations.Count < 1)
                    {
                        violations.Add(new MerchantTribe.Web.Validation.RuleViolation("Workflow", "Internal Error",
                            "An internal error occured while attempting to place your order. Please contact the store owner directly to complete your order."));
                    }
                }
            }
        }

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

        [ChildActionOnly]
        public ActionResult DisplayPaymentMethods(Order o)
        {

            return View();
        }
    }
}
