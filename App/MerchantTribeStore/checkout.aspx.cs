using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using BVSoftware.Commerce;
using BVSoftware.Commerce.BusinessRules;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Contacts;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce.Membership;
using BVSoftware.Commerce.Orders;
using BVSoftware.Commerce.Utilities;
using BVSoftware.Commerce.Payment;

namespace BVCommerce
{

    partial class checkout : BaseStorePage
    {

        public override bool RequiresSSL
        {
            get { return true; }
        }

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.Title = "Checkout";
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.AddBodyClass("store-checkout-page");
        }
        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.IsPostBack)
            {
                this.customeremail.Focus();
            }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            WebForms.MakePageNonCacheable(this);

            

            if (SessionManager.IsUserAuthenticated(this.BVApp))
            {
                this.pnlLoggedIn.Visible = true;
                this.pnlNotLoggedIn.Visible = false;
            }
            else
            {
                this.pnlLoggedIn.Visible = false;
                this.pnlNotLoggedIn.Visible = true;                
            }

            this.LoginControl1.LoginCompleted += new BVModules_Controls_LoginControl.LoginCompletedDelegate(LoginControl1_LoginCompleted);

            if (!Page.IsPostBack)
            {
                ThemeManager themes = BVApp.ThemeManager();
                this.btnSubmit.ImageUrl = themes.ButtonUrl("PlaceOrder", Request.IsSecureConnection);
                
                Order Basket = LoadBasket();
                CheckForPoints(Basket);

                //set affiliate id
                string affid = BVApp.ContactServices.GetValidAffiliateId().ToString();
                if (!string.IsNullOrEmpty(affid))
                {
                    Basket.AffiliateID = affid;
                    BVApp.OrderServices.Orders.Update(Basket);
                }

            }

            string scriptPath = GetRouteUrl("js", new { filename = "checkout.js" });
            Page.ClientScript.RegisterClientScriptInclude("checkoutscripts", scriptPath);
        }

        void CheckForPoints(Order o)
        {
            this.pnlRewardsPoints.Visible = false;

            string uid = SessionManager.GetCurrentUserId();
            CustomerPointsManager pointsManager = CustomerPointsManager.InstantiateForDatabase(BVApp.CurrentStore.Settings.RewardsPointsIssuedPerDollarSpent,
                                                                            BVApp.CurrentStore.Settings.RewardsPointsNeededPerDollarCredit,
                                                                            BVApp.CurrentStore.Id);
            if (pointsManager.FindAvailablePoints(uid) > 0 && BVApp.CurrentStore.Settings.RewardsPointsOnPurchasesActive)
            {                
                this.pnlRewardsPoints.Visible = true;
                this.lblRewardsPointsName.Text = BVApp.CurrentStore.Settings.RewardsPointsName;
                this.PaymentRewardsPoints1.Populate(o);
            }
        }

        void LoginControl1_LoginCompleted(object sender, BVSoftware.Commerce.Controls.LoginCompleteEventArgs args)
        {
            Order Basket = LoadBasket();
            Basket.UserID = args.UserId;
            Basket.UserEmail = args.UserEmail;
            BVApp.CalculateOrderAndSave(Basket);            
            SessionManager.SaveOrderCookies(Basket);
            Response.Redirect(GetRouteUrl("checkout-route", new { }));
        }

        private BVSoftware.Commerce.Contacts.Address GetBillingAddress()
        {
            if (this.chkBillSame.Checked)
            {
                return this.AddressShipping1.GetAsAddress();
            }
            else
            {
                return this.AddressBilling1.GetAsAddress();
            }
        }

        private Order LoadBasket()
        {
            Order Basket = SessionManager.CurrentShoppingCart(BVApp.OrderServices);
            if (Basket != null)
            {

                this.orderbvin.Value = Basket.bvin;

                if (Basket.Items.Count == 0)
                {
                    Response.Redirect("~/Default.aspx");
                }

                // Email
                this.customeremail.Text = Basket.UserEmail;
                if (SessionManager.IsUserAuthenticated(this.BVApp))
                {
                    CustomerAccount currentCustomer = BVApp.CurrentCustomer;
                    if (currentCustomer != null) this.customeremail.Text = currentCustomer.Email;
                }

                // Addresses
                LoadAddressesForCurrentUser(Basket);

                // Shipping Method
                // Shipping.LoadShippingMethodsForOrder(Basket)

                // Payment
                Payment.LoadPaymentMethods(Basket.TotalGrand);

                // Special Instructions
                this.SpecialInstructions.Text = Basket.Instructions;

                // Totals
                LoadTotals(Basket);

                // Cart Preview
                this.ViewOrderItems1.LoadItems(Basket.Items);
            }
            return Basket;
        }

        //Private Sub ReloadShipping(ByVal Basket As Orders.Order)
        //If Basket Is Nothing Then
        //Basket = SessionManager.CurrentShoppingCart
        //End If
        //If Basket IsNot Nothing Then
        //Dim shipAdd As BVSoftware.Commerce.Contacts.Address = Me.AddressShipping1.GetAsAddress()
        //Basket.SetShippingAddress(shipAdd)
        //Orders.Order.Update(Basket)
        //Shipping.LoadShippingMethodsForOrder(Basket, shipAdd.PostalCode)
        //If Basket.ShippingMethodId <> String.Empty Then
        //Shipping.SetShippingMethod(Basket.ShippingMethodUniqueKey)
        //End If
        //End If
        //End Sub

        private void LoadAddressesForCurrentUser(Order Basket)
        {
            CustomerAccount u = null;

            if (SessionManager.IsUserAuthenticated(this.BVApp) == true)
            {
                u = BVApp.CurrentCustomer;
                if (u != null)
                {
                    this.AddressBilling1.LoadFromAddress(u.GetBillingAddress());
                    
                    
                    this.AddressShipping1.LoadFromAddress(u.GetShippingAddress());
                    
                    //ReloadShipping(Basket)                            
                }
            }

            if ((u == null))
            {
                this.AddressBilling1.LoadFromAddress(Basket.BillingAddress);
                this.AddressShipping1.LoadFromAddress(Basket.ShippingAddress);
            }

            // populate temp fields so that ajax loader will correctly set
            // region after "change" event on country dropdown
            this.TempBillingRegion.Value = this.AddressBilling1.GetAsAddress().RegionBvin;
            this.TempShippingRegion.Value = this.AddressShipping1.GetAsAddress().RegionBvin;
        }

        protected void btnSubmit_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {            
            Order Basket = SessionManager.CurrentShoppingCart(BVApp.OrderServices);
            if (Basket == null)
            {
                Response.Redirect("~/cart");
            }

            // Save Information to Basket in Case Save as Order Fails
            Basket.BillingAddress = GetBillingAddress();
            Basket.ShippingAddress = this.AddressShipping1.GetAsAddress();
            // Email
            Basket.UserEmail = this.customeremail.Text.Trim();

            // Save Shipping Selection
            string shippingRateKey = Request.Form["shippingrate"];
            BVApp.OrderServices.OrdersRequestShippingMethodByUniqueKey(shippingRateKey, Basket);
            BVApp.CalculateOrder(Basket);
            SessionManager.SaveOrderCookies(Basket);

            // Save Payment Information                    
            PaymentRewardsPoints1.ApplyInfoToOrder(Basket);
            Payment.SavePaymentInfo(Basket);
                        
            Basket.AffiliateID = BVApp.ContactServices.GetValidAffiliateId().ToString();
            Basket.Instructions = this.SpecialInstructions.Text.Trim();

            // Save all the changes to the order
            BVApp.OrderServices.Orders.Update(Basket);

            if ((!Page.IsValid))
            {
                return;
            }

            if (!this.SiteTermsAgreement1.IsValid)
            {
                List<MerchantTribe.Web.Validation.RuleViolation> violations = new List<MerchantTribe.Web.Validation.RuleViolation>();
                violations.Add(new MerchantTribe.Web.Validation.RuleViolation("Terms", "Terms", SiteTerms.GetTerm(SiteTermIds.SiteTermsAgreementError)));
                RenderViolations(violations);
                return;
            }

            if ((!ValidateSelections()))
            {
                return;
            }

            // Save as Order
            OrderTaskContext c = new OrderTaskContext(BVApp);
            c.UserId = SessionManager.GetCurrentUserId();
            c.Order = Basket;

            // Check for PayPal Request
            //OrderPaymentManager payManager = new OrderPaymentManager(Basket);
            bool paypalCheckoutSelected = this.Payment.PayPalSelected;
            //bool paypalCheckoutSelected = payManager.PayPalExpressHasInfo();

            if (paypalCheckoutSelected)
            {
                c.Inputs.Add("bvsoftware", "Mode", "PaypalExpress");
                c.Inputs.Add("bvsoftware", "AddressSupplied", "1");
                if (!Workflow.RunByName(c, WorkflowNames.ThirdPartyCheckoutSelected))
                {
                    EventLog.LogEvent("Paypal Express Checkout Failed", "Specific Errors to follow", BVSoftware.Commerce.Metrics.EventLogSeverity.Error);
                    // Show Errors     
                    List<MerchantTribe.Web.Validation.RuleViolation> violations = new List<MerchantTribe.Web.Validation.RuleViolation>();
                    foreach (WorkflowMessage item in c.GetCustomerVisibleErrors())
                    {
                        violations.Add(new MerchantTribe.Web.Validation.RuleViolation("Workflow", item.Name, item.Description));
                    }
                    RenderViolations(violations);
                }
            }
            else
            {
                if (Workflow.RunByName(c, WorkflowNames.ProcessNewOrder) == true)
                {
                    // Clear Cart ID because we're now an order
                    SessionManager.CurrentCartID = string.Empty;

                    // Process Payment
                    if (BVSoftware.Commerce.BusinessRules.Workflow.RunByName(c, BVSoftware.Commerce.BusinessRules.WorkflowNames.ProcessNewOrderPayments))
                    {
                        BVSoftware.Commerce.BusinessRules.Workflow.RunByName(c, BVSoftware.Commerce.BusinessRules.WorkflowNames.ProcessNewOrderAfterPayments);
                        Order tempOrder = BVApp.OrderServices.Orders.FindForCurrentStore(Basket.bvin);
                        BVSoftware.Commerce.Integration.Current().OrderReceived(tempOrder, BVApp);
                        Response.Redirect("~/Receipt.aspx?id=" + Basket.bvin);
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
                    RenderViolations(violations);
                }
            }

            LoadTotals(Basket);
        }

        private void RenderViolations(List<MerchantTribe.Web.Validation.RuleViolation> violations)
        {
            if (violations != null)
            {
                if (violations.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<ul class=\"validation-summary-errors\">");
                    foreach (MerchantTribe.Web.Validation.RuleViolation v in violations)
                    {
                        sb.Append("<li>" + v.ErrorMessage + "</li>");
                    }
                    sb.Append("</ul>");
                    this.litValidationSummary.Text += sb.ToString();
                }
            }

        }

        private bool ValidateSelections()
        {
            bool result = true;
            this.litValidationSummary.Text = string.Empty;

            List<MerchantTribe.Web.Validation.RuleViolation> violations = new List<MerchantTribe.Web.Validation.RuleViolation>();

            // Validate Email
            MerchantTribe.Web.Validation.ValidationHelper.ValidEmail("Email Address", this.customeremail.Text, violations, "customeremail");

            // Validate Shipping Address
            violations.AddRange(this.AddressShipping1.GetRuleViolations());

            // Validate Billing Address
            if (!this.chkBillSame.Checked)
            {
                violations.AddRange(this.AddressBilling1.GetRuleViolations());
            }

            Order Basket = SessionManager.CurrentShoppingCart(BVApp.OrderServices);
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
                violations.AddRange(Basket.GetRuleViolations());
            }

            if ((!Payment.IsValid()) && (!paymentFound))
            {
                violations.AddRange(Payment.GetRuleViolations());
            }

            if ((violations.Count > 0))
            {
                result = false;
                RenderViolations(violations);
            }

            return result;
        }

        protected void LoadTotals(Order Basket)
        {
            if (Basket == null)
            {
                Basket = SessionManager.CurrentShoppingCart(BVApp.OrderServices);
            }
            BVApp.OrderServices.Orders.Update(Basket);
            this.litTotals.Text = Basket.TotalsAsTable();                       
        }
     
    }

}