using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.BusinessRules;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Utilities;
using MerchantTribe.Commerce.Payment;

namespace MerchantTribeStore
{
    public partial class checkoutpaymenterror : BaseStorePage
    {      
        public override bool RequiresSSL
        {
            get { return true; }
        }

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.Title = "Checkout Payment Error";
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.AddBodyClass("store-checkout-page");
        }
        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            WebForms.MakePageNonCacheable(this);
            
            if (!Page.IsPostBack)
            {
                ThemeManager themes = MTApp.ThemeManager();
                this.btnSubmit.ImageUrl = themes.ButtonUrl("PlaceOrder", Request.IsSecureConnection);
                Order Basket = LoadBasket();             
            }

            string scriptPath = GetRouteUrl("js", new { filename = "checkoutpaymenterror.js" });
            Page.ClientScript.RegisterClientScriptInclude("checkoutscripts", scriptPath);
        }
       
        private MerchantTribe.Commerce.Contacts.Address GetBillingAddress()
        {
            return null; //**************** this.AddressBilling1.GetAsAddress();
        }

        private Order LoadBasket()
        {
            string bvin = SessionManager.CurrentPaymentPendingCartId;
            if (bvin.Trim().Length < 1) Response.Redirect("~/cart");

            Order Basket = MTApp.OrderServices.Orders.FindForCurrentStore(bvin);
            if (Basket != null)
            {
                if (Basket.Items.Count == 0)
                {
                    Response.Redirect("~/cart");
                }

                //*******************this.AddressBilling1.LoadFromAddress(Basket.BillingAddress);                                
                //*******************Payment.LoadPaymentMethods(Basket.TotalGrand);                
            }
            return Basket;
        }
        
    
        protected void btnSubmit_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string bvin = SessionManager.CurrentPaymentPendingCartId;
            Order Basket = MTApp.OrderServices.Orders.FindForCurrentStore(bvin);
            if (Basket == null)
            {
                Response.Redirect("~/cart");
            }

            // Update Order Info
            Basket.BillingAddress = GetBillingAddress();            
            //***************************Payment.SavePaymentInfo(Basket);
            MTApp.OrderServices.Orders.Update(Basket);

            if ((!Page.IsValid))
            {
                return;
            }

            if ((!ValidateSelections()))
            {
                return;
            }

            // Save as Order
            OrderTaskContext c = new OrderTaskContext(MTApp);
            c.UserId = SessionManager.GetCurrentUserId();
            c.Order = Basket;
           
          
                if (Workflow.RunByName(c, WorkflowNames.ProcessNewOrderPayments) == true)
                {
                    // Clear Pending Cart ID because payment is good
                    SessionManager.CurrentPaymentPendingCartId = string.Empty;

                    // Process Post Payment Stuff                    
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

            //**********************violations.AddRange(this.AddressBilling1.GetRuleViolations());
                                    
            //************************if (!Payment.IsValid())
            //************************{
            //************************    violations.AddRange(Payment.GetRuleViolations());
            //************************}

            if ((violations.Count > 0))
            {
                result = false;
                RenderViolations(violations);
            }

            return result;
        }

        protected void lnkCancel_Click(object sender, System.EventArgs e)
        {
            string bvin = SessionManager.CurrentPaymentPendingCartId;
            if (bvin.Trim().Length < 1) Response.Redirect("~/cart");

            Order Basket = MTApp.OrderServices.Orders.FindForCurrentStore(bvin);
            if (Basket != null)
            {
                Basket.StatusCode = OrderStatusCode.Cancelled;
                Basket.StatusName = "Cancelled";

                OrderNote n = new OrderNote();
                n.IsPublic = true;
                n.Note = "Cancelled by Customer";
                Basket.Notes.Add(n);

                MTApp.OrderServices.Orders.Update(Basket);
                SessionManager.CurrentPaymentPendingCartId = string.Empty;
                Response.Redirect("~/cart");                
            }            
        }
     
    }

}