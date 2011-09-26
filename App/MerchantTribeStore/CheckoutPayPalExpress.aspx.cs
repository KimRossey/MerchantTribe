using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Utilities;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.PaypalWebServices;
using MerchantTribe.Web.Geography;
using com.paypal.soap.api;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore
{

    public partial class CheckoutPayPalExpress : BaseStorePage
    {

        public override bool RequiresSSL
        {
            get { return true; }
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.Title = "Checkout";
        }

        protected void CheckoutImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (Page.IsValid)
            {
                if (!SiteTermsAgreement1.IsValid)
                {
                    MessageBox1.ShowError(SiteTerms.GetTerm(SiteTermIds.SiteTermsAgreementError));
                }
                else
                {
                    Order Basket = SessionManager.CurrentShoppingCart(MTApp.OrderServices);

                    // Save Shipping Selection
                    MerchantTribe.Commerce.Shipping.ShippingRateDisplay r = FindSelectedRate(this.ShippingRatesList.SelectedValue, Basket);
                    MTApp.OrderServices.OrdersRequestShippingMethod(r, Basket);
                    MTApp.CalculateOrder(Basket);
                    SessionManager.SaveOrderCookies(Basket);
                    
                    // Save Payment Information
                    SavePaymentInfo(Basket);

                    MTApp.OrderServices.Orders.Update(Basket);

                    // Save as Order
                    MerchantTribe.Commerce.BusinessRules.OrderTaskContext c 
                        = new MerchantTribe.Commerce.BusinessRules.OrderTaskContext(MTApp);
                    c.UserId = SessionManager.GetCurrentUserId();
                    c.Order = Basket;

                    if (MerchantTribe.Commerce.BusinessRules.Workflow.RunByName(c, MerchantTribe.Commerce.BusinessRules.WorkflowNames.ProcessNewOrder))
                    {                                               
                        // Clear Cart ID because we're now an order
                        SessionManager.CurrentCartID = string.Empty;

                        // Process Payment
                        if (MerchantTribe.Commerce.BusinessRules.Workflow.RunByName(c, MerchantTribe.Commerce.BusinessRules.WorkflowNames.ProcessNewOrderPayments))
                        {                            
                            MerchantTribe.Commerce.BusinessRules.Workflow.RunByName(c, MerchantTribe.Commerce.BusinessRules.WorkflowNames.ProcessNewOrderAfterPayments);
                            Order tempOrder = MTApp.OrderServices.Orders.FindForCurrentStore(Basket.bvin);
                            MerchantTribe.Commerce.Integration.Current().OrderReceived(tempOrder, MTApp);                            
                            Response.Redirect("~/checkout/receipt?id=" + Basket.bvin);
                        }
                        else
                        {
                            // Redirect to Payment Error
                            SessionManager.CurrentPaymentPendingCartId = Basket.bvin;
                            Response.Redirect("~/CheckoutPaymentError.aspx");
                        }                        
                    }
                    else
                    {
                        // Show Errors                
                        foreach (MerchantTribe.Commerce.BusinessRules.WorkflowMessage item in c.GetCustomerVisibleErrors())
                        {
                            MessageBox1.ShowError(item.Description);
                        }
                    }
                }
            }
        }

        private void DisplayPaypalExpressMode()
        {
            if ((Request.QueryString["token"] != null) && (Request.QueryString["token"] != string.Empty))
            {
                PayPalAPI ppAPI = MerchantTribe.Commerce.Utilities.PaypalExpressUtilities.GetPaypalAPI();
                bool failed = false;
                GetExpressCheckoutDetailsResponseType ppResponse = null;
                try
                {
                    if (!GetExpressCheckoutDetails(ppAPI, ref ppResponse))
                    {
                        if (!GetExpressCheckoutDetails(ppAPI, ref ppResponse))
                        {
                            failed = true;
                            EventLog.LogEvent("Paypal Express Checkout", "GetExpressCheckoutDetails call failed. Detailed Errors will follow. ", MerchantTribe.Commerce.Metrics.EventLogSeverity.Error);
                            foreach (ErrorType ppError in ppResponse.Errors)
                            {
                                EventLog.LogEvent("Paypal error number: " + ppError.ErrorCode, "Paypal Error: '" + ppError.ShortMessage + "' Message: '" + ppError.LongMessage + "' " + " Values passed to GetExpressCheckoutDetails: Token: " + Request.QueryString["token"], MerchantTribe.Commerce.Metrics.EventLogSeverity.Error);
                            }
                            MessageBox1.ShowError("An error occurred during the Paypal Express checkout. No charges have been made. Please try again.");
                            CheckoutImageButton.Visible = false;
                        }
                    }
                }
                finally
                {
                    Order o = SessionManager.CurrentShoppingCart(MTApp.OrderServices);
                    EditAddressLinkButton.Visible = true;
                    if (o.CustomProperties["PaypalAddressOverride"] != null)
                    {
                        if (o.CustomProperties["PaypalAddressOverride"].Value == "1")
                        {
                            EditAddressLinkButton.Visible = false;
                        }
                    }

                    o.CustomProperties.Add("bvsoftware", "PayerID", Request.QueryString["PayerID"]);
                    if (!failed)
                    {
                        if (ppResponse != null && ppResponse.GetExpressCheckoutDetailsResponseDetails != null && ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo != null)
                        {
                            o.UserEmail = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Payer;
                            if (string.IsNullOrEmpty(o.ShippingAddress.Phone))
                            {
                                o.ShippingAddress.Phone = ppResponse.GetExpressCheckoutDetailsResponseDetails.ContactPhone;
                            }
                        }
                    }
                    MTApp.OrderServices.Orders.Update(o);
                    ppAPI = null;
                }
            }
            else
            {
                Response.Redirect(MTApp.CurrentStore.RootUrl());
            }

        }

        protected bool GetExpressCheckoutDetails(PayPalAPI ppAPI, ref GetExpressCheckoutDetailsResponseType ppResponse)
        {
            ppResponse = ppAPI.GetExpressCheckoutDetails(Request.QueryString["token"]);
            if (ppResponse.Ack == AckCodeType.Success || ppResponse.Ack == AckCodeType.SuccessWithWarning)
            {
                EmailLabel.Text = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Payer;
                FirstNameLabel.Text = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerName.FirstName;
                if (ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerName.MiddleName.Length > 0)
                {
                    MiddleInitialLabel.Text = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerName.MiddleName.Substring(0, 1);
                }
                else
                {
                    MiddleInitialLabel.Text = string.Empty;
                }
                LastNameLabel.Text = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerName.LastName;
                CompanyLabel.Text = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerBusiness;
                StreetAddress1Label.Text = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.Street1;
                StreetAddress2Label.Text = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.Street2;
                CountryLabel.Text = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.CountryName;
                ViewState["CountryCode"] = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.Country.ToString();
                CityLabel.Text = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.CityName;
                StateLabel.Text = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.StateOrProvince;
                ZipLabel.Text = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.PostalCode;
                PhoneNumberLabel.Text = ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.ContactPhone;

                if (ppResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.AddressStatus == AddressStatusCodeType.Confirmed)
                {
                    AddressStatusLabel.Text = "Confirmed";
                }
                else
                {
                    AddressStatusLabel.Text = "Unconfirmed";
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            MerchantTribe.Commerce.Utilities.WebForms.MakePageNonCacheable(this);
            this.CheckoutImageButton.Visible = true;
            if (!Page.IsPostBack)
            {
                ThemeManager themes = MTApp.ThemeManager();
                CheckoutImageButton.ImageUrl = themes.ButtonUrl("PlaceOrder", Request.IsSecureConnection);
                this.btnKeepShopping.ImageUrl = themes.ButtonUrl("keepshopping", Request.IsSecureConnection);
                DisplayPaypalExpressMode();
                LoadShippingMethodsForOrder();
            }
        }

        private void LoadShippingMethodsForOrder()
        {
            Order o = SessionManager.CurrentShoppingCart(MTApp.OrderServices);

            MerchantTribe.Commerce.Contacts.Address address = GetAddress();
            if ((address != null))
            {
                o.ShippingAddress = address;
                o.BillingAddress = address;
                MTApp.CalculateOrderAndSave(o);
                SessionManager.SaveOrderCookies(o);
            }

            LoadShippingMethodsForOrder(o);
        }
        private void LoadShippingMethodsForOrder(Order o)
        {

            SortableCollection<ShippingRateDisplay> Rates = new SortableCollection<ShippingRateDisplay>();

            if (o.HasShippingItems == false)
            {
                ShippingRateDisplay r = new ShippingRateDisplay();
                r.DisplayName = SiteTerms.GetTerm(SiteTermIds.NoShippingRequired);
                r.ProviderId = "";
                r.ProviderServiceCode = "";
                r.Rate = 0;
                r.ShippingMethodId = "NOSHIPPING";
                Rates.Add(r);
            }
            else
            {
                // Shipping Methods

                Rates = MTApp.OrderServices.FindAvailableShippingRates(o);

                if ((Rates.Count < 1))
                {
                    ShippingRateDisplay result = new ShippingRateDisplay();
                    result.DisplayName = "Shipping can not be calculated at this time. We will contact you after receiving your order with the exact shipping charges.";
                    result.ShippingMethodId = "TOBEDETERMINED";
                    result.Rate = 0;
                    Rates.Add(result);
                }

            }

            // Shipping Methods
            SessionManager.LastShippingRates = Rates;
            this.ShippingRatesList.DataTextField = "RateAndNameForDisplay";
            this.ShippingRatesList.DataValueField = "UniqueKey";
            this.ShippingRatesList.DataSource = Rates;
            this.ShippingRatesList.DataBind();
            //this.litMain.Text = MerchantTribe.Commerce.Utilities.HtmlRendering.ShippingRatesToRadioButtons(Rates, this.TabIndex, o.ShippingMethodUniqueKey);
        }

        private MerchantTribe.Commerce.Contacts.Address GetAddress()
        {
            MerchantTribe.Commerce.Contacts.Address a = new MerchantTribe.Commerce.Contacts.Address();
            Country country = Country.FindByISOCode((string)ViewState["CountryCode"]);
            if (country.Bvin == string.Empty)
            {
                MessageBox1.ShowError("Could not retreive address properly, country could not be found.");
                CheckoutImageButton.Enabled = false;
            }
            else
            {
                CheckoutImageButton.Enabled = true;
            }

            //if (!country.Active) {
            //    MessageBox1.ShowError("This country is not active for this store.");
            //    CheckoutImageButton.Enabled = false;
            //}
            //else {
            CheckoutImageButton.Enabled = true;
            //}

            if (country.Bvin != string.Empty)
            {
                a.CountryBvin = country.Bvin;
                a.CountryName = country.DisplayName;
                a.RegionName = StateLabel.Text;
                foreach (Region region in Country.FindByBvin(country.Bvin).Regions)
                {
                    if ((string.Compare(region.Abbreviation, a.RegionName, true) == 0) || (string.Compare(region.Name, a.RegionName, true) == 0))
                    {
                        a.RegionBvin = region.Abbreviation;
                        a.RegionName = region.Name;
                    }
                }
                a.FirstName = FirstNameLabel.Text;
                a.MiddleInitial = MiddleInitialLabel.Text;
                a.LastName = LastNameLabel.Text;
                a.Company = CompanyLabel.Text;
                a.Line1 = StreetAddress1Label.Text;
                a.Line2 = StreetAddress2Label.Text;
                a.City = CityLabel.Text;
                a.PostalCode = ZipLabel.Text;
                a.Phone = PhoneNumberLabel.Text;
                a.Fax = "";
                a.WebSiteUrl = "";
                return a;
            }
            else
            {
                return null;
            }
        }

        private MerchantTribe.Commerce.Shipping.ShippingRateDisplay FindSelectedRate(string uniqueKey, Order o)
        {
            MerchantTribe.Commerce.Shipping.ShippingRateDisplay result = null;

            MerchantTribe.Commerce.Utilities.SortableCollection<MerchantTribe.Commerce.Shipping.ShippingRateDisplay> rates = SessionManager.LastShippingRates;
            if ((rates == null) | (rates.Count < 1))
            {
                rates = MTApp.OrderServices.FindAvailableShippingRates(o);
            }

            foreach (MerchantTribe.Commerce.Shipping.ShippingRateDisplay r in rates)
            {
                if (r.UniqueKey == uniqueKey)
                {
                    result = r;
                    break;
                }
            }

            return result;
        }

        private void SavePaymentInfo(Order o)
        {
            OrderPaymentManager payManager = new OrderPaymentManager(o, MTApp);
            payManager.ClearAllTransactions();

            string token = Request.QueryString["Token"];
            string payerId = Request.QueryString["PayerId"];
            if (!string.IsNullOrEmpty(payerId))
            {
                // This is to fix a bug with paypal returning multiple payerId's
                payerId = payerId.Split(',')[0];
            }

            payManager.PayPalExpressAddInfo(o.TotalGrand, token, payerId);
        }

        protected void CustomValidator1_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            if (!MTApp.CurrentStore.Settings.PayPal.AllowUnconfirmedAddresses)
            {
                if (string.Compare(AddressStatusLabel.Text, "Unconfirmed", true) == 0)
                {
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
                }
            }
        }

        protected void btnKeepShopping_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string destination = "~";

            if (SessionManager.CategoryLastId != string.Empty)
            {
                MerchantTribe.Commerce.Catalog.Category c = MTApp.CatalogServices.Categories.Find(SessionManager.CategoryLastId);
                if (c != null)
                {
                    if (c.Bvin != string.Empty)
                    {
                        destination = MerchantTribe.Commerce.Utilities.UrlRewriter.BuildUrlForCategory(new CategorySnapshot(c), MTApp.CurrentRequestContext.RoutingContext);
                    }
                }
            }

            Response.Redirect(destination);
        }

        protected void EditAddressLinkButton_Click(object sender, System.EventArgs e)
        {
            Order o = SessionManager.CurrentShoppingCart(MTApp.OrderServices);

            PayPalAPI ppAPI = MerchantTribe.Commerce.Utilities.PaypalExpressUtilities.GetPaypalAPI();
            try
            {
                string cartReturnUrl = MTApp.CurrentStore.RootUrlSecure() + "paypalexpresscheckout";
                string cartCancelUrl = MTApp.CurrentStore.RootUrlSecure() + "checkout";

                SetExpressCheckoutResponseType expressResponse;
                if (MTApp.CurrentStore.Settings.PayPal.ExpressAuthorizeOnly)
                {
                    expressResponse = ppAPI.SetExpressCheckout(string.Format("{0:N}", o.TotalOrderBeforeDiscounts), cartReturnUrl, cartCancelUrl, PaymentActionCodeType.Order, PayPalAPI.GetCurrencyCodeType(MTApp.CurrentStore.Settings.PayPal.Currency), o.OrderNumber);
                }
                else
                {
                    expressResponse = ppAPI.SetExpressCheckout(string.Format("{0:N}", o.TotalOrderBeforeDiscounts), cartReturnUrl, cartCancelUrl, PaymentActionCodeType.Sale, PayPalAPI.GetCurrencyCodeType(MTApp.CurrentStore.Settings.PayPal.Currency), o.OrderNumber);
                }

                if (expressResponse.Ack == AckCodeType.Success || expressResponse.Ack == AckCodeType.SuccessWithWarning)
                {
                    o.ThirdPartyOrderId = expressResponse.Token;
                    if (MTApp.OrderServices.Orders.Update(o))
                    {
                        if (string.Compare(MTApp.CurrentStore.Settings.PayPal.Mode, "Live", true) == 0)
                        {
                            System.Web.HttpContext.Current.Response.Redirect("https://www.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=" + expressResponse.Token, false);
                        }
                        else
                        {
                            System.Web.HttpContext.Current.Response.Redirect("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=" + expressResponse.Token, false);
                        }

                    }
                }
                else
                {
                    foreach (ErrorType ppError in expressResponse.Errors)
                    {
                        EventLog.LogEvent("Paypal error number: " + ppError.ErrorCode, "Paypal Error: '" + ppError.ShortMessage + "' Message: '" + ppError.LongMessage + "' " + " Values passed to SetExpressCheckout: Total=" + string.Format("{0:c}", o.TotalOrderBeforeDiscounts) + " Cart Return Url: " + cartReturnUrl + " Cart Cancel Url: " + cartCancelUrl, MerchantTribe.Commerce.Metrics.EventLogSeverity.Error);
                    }
                }
            }
            finally
            {
                ppAPI = null;
            }
        }

        protected void BVRequiredFieldValidator1_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            if ((this.ShippingRatesList.SelectedIndex == -1))
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }
    }
}