using System.Collections.ObjectModel;
using System.Web.UI;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Metrics;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Payment;

namespace MerchantTribeStore
{

    partial class BVModules_Controls_PaypalExpressCheckoutButton : MerchantTribe.Commerce.Content.BVUserControl
    {

        public bool DisplayText
        {
            get
            {
                object obj = ViewState["DisplayText"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                else
                {
                    return true;
                }
            }
            set { ViewState["DisplayText"] = value; }
        }

        public delegate void WorlflowFailedDelegate(string message);
        public event WorlflowFailedDelegate WorkflowFailed;
        public delegate void CheckoutButtonClickedDelegate(PaypalExpressCheckoutArgs args);
        public event CheckoutButtonClickedDelegate CheckoutButtonClicked;

        protected void PaypalImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            PaypalExpressCheckoutArgs args = new PaypalExpressCheckoutArgs();
            if (CheckoutButtonClicked != null)
            {
                CheckoutButtonClicked(args);
            }
            if (!args.Failed)
            {
                Order Basket = SessionManager.CurrentShoppingCart(MyPage.MTApp.OrderServices);
                // Save as Order
                MerchantTribe.Commerce.BusinessRules.OrderTaskContext c 
                    = new MerchantTribe.Commerce.BusinessRules.OrderTaskContext(MyPage.MTApp);
                c.UserId = SessionManager.GetCurrentUserId();
                c.Order = Basket;
                bool checkoutFailed = false;
                if (!MerchantTribe.Commerce.BusinessRules.Workflow.RunByName(c, MerchantTribe.Commerce.BusinessRules.WorkflowNames.CheckoutSelected))
                {
                    checkoutFailed = true;
                    bool customerMessageFound = false;
                    foreach (MerchantTribe.Commerce.BusinessRules.WorkflowMessage msg in c.Errors)
                    {
                        EventLog.LogEvent(msg.Name, msg.Description, EventLogSeverity.Error);
                        if (msg.CustomerVisible)
                        {
                            customerMessageFound = true;
                            if (WorkflowFailed != null)
                            {
                                WorkflowFailed(msg.Description);
                            }
                        }
                    }
                    if (!customerMessageFound)
                    {
                        EventLog.LogEvent("Checkout Selected Workflow", "Checkout failed but no errors were recorded.", EventLogSeverity.Error);
                        if (WorkflowFailed != null)
                        {
                            WorkflowFailed("Checkout Failed. If problem continues, please contact customer support.");
                        }
                    }
                }

                if (!checkoutFailed)
                {
                    c.Inputs.Add("bvsoftware", "Mode", "PaypalExpress");
                    if (!MerchantTribe.Commerce.BusinessRules.Workflow.RunByName(c, MerchantTribe.Commerce.BusinessRules.WorkflowNames.ThirdPartyCheckoutSelected))
                    {
                        bool customerMessageFound = false;
                        EventLog.LogEvent("Paypal Express Checkout Failed", "Specific Errors to follow", EventLogSeverity.Error);
                        foreach (MerchantTribe.Commerce.BusinessRules.WorkflowMessage item in c.Errors)
                        {
                            EventLog.LogEvent("Paypal Express Checkout Failed", item.Name + ": " + item.Description, EventLogSeverity.Error);
                            if (item.CustomerVisible)
                            {
                                if (WorkflowFailed != null)
                                {
                                    WorkflowFailed(item.Description);
                                }
                                customerMessageFound = true;
                            }
                        }
                        if (!customerMessageFound)
                        {
                            if (WorkflowFailed != null)
                            {
                                WorkflowFailed("Paypal Express Checkout Failed. If this problem persists please notify customer support.");
                            }
                        }
                    }
                }
            }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                MerchantTribe.Commerce.Payment.AvailablePayments availablePayments = new MerchantTribe.Commerce.Payment.AvailablePayments();
                Collection<DisplayPaymentMethod> enabledMethods;
                enabledMethods = availablePayments.EnabledMethods(MyPage.MTApp.CurrentStore);

                this.Visible = false;
                foreach (DisplayPaymentMethod m in enabledMethods)
                {
                    switch (m.MethodId)
                    {
                        case WebAppSettings.PaymentIdPaypalExpress:
                            this.Visible = true;
                            break;
                        default:
                            // do nothing
                            break;
                    }
                }
            }
        }
    }
}