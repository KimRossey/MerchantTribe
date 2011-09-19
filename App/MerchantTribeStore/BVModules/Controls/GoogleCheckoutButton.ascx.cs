using System.Collections.ObjectModel;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Metrics;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Payment;

namespace BVCommerce
{

    partial class BVModules_Controls_GoogleCheckoutButton : MerchantTribe.Commerce.Content.BVUserControl
    {
        public delegate void WorkflowFailedDelegate(string message);
        public event WorkflowFailedDelegate WorkflowFailed;
        public delegate void CheckoutButtonClickedDelegate(GoogleCheckoutArgs args);
        public event CheckoutButtonClickedDelegate CheckoutButtonClicked;

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            this.Visible = false;
            MerchantTribe.Commerce.Payment.AvailablePayments availablePayments = new MerchantTribe.Commerce.Payment.AvailablePayments();
            Collection<DisplayPaymentMethod> enabledMethods = availablePayments.EnabledMethods(MyPage.MTApp.CurrentStore);
            foreach (DisplayPaymentMethod m in enabledMethods)
            {
                switch (m.MethodId)
                {
                    case WebAppSettings.PaymentIdGoogleCheckout:
                        string url = string.Empty;
                        if (string.Compare(MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.Mode, "Production", true) == 0)
                        {
                            url = "http://checkout.google.com/buttons/checkout.gif?variant=text&";
                        }
                        else
                        {
                            url = "http://sandbox.google.com/checkout/buttons/checkout.gif?variant=text&";
                        }


                        this.Visible = true;
                        switch (MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.ButtonSize.ToUpper())
                        {
                            case "SMALL":
                                url += "w=160&h=43";
                                break;
                            case "MEDIUM":
                                url += "w=168&h=44";
                                break;
                            case "LARGE":
                                url += "w=180&h=46";
                                break;
                            default:
                                url += "w=168&h=44";
                                break;
                        }

                        switch (MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.ButtonBackground.ToUpper())
                        {
                            case "WHITE":
                                url += "&style=white";
                                break;
                            case "TRANSPARENT":
                                url += "&style=trans";
                                break;
                            default:
                                url += "&style=trans";
                                break;
                        }
                        this.GoogleCheckoutImageButton.ImageUrl = url;
                        break;
                    default:
                        // do nothign
                        break;
                }
            }
        }
     

        protected void GoogleCheckoutImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            GoogleCheckoutArgs args = new GoogleCheckoutArgs();
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
                    c.Inputs.Add("bvsoftware", "Mode", "GoogleCheckout");
                    if (!MerchantTribe.Commerce.BusinessRules.Workflow.RunByName(c, MerchantTribe.Commerce.BusinessRules.WorkflowNames.ThirdPartyCheckoutSelected))
                    {
                        bool customerMessageFound = false;
                        EventLog.LogEvent("Google Checkout", "Failed: Specific Errors to follow", EventLogSeverity.Error);
                        foreach (MerchantTribe.Commerce.BusinessRules.WorkflowMessage item in c.Errors)
                        {
                            EventLog.LogEvent("Google Checkout", "Failed: " + item.Name + ": " + item.Description, EventLogSeverity.Error);
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
                                WorkflowFailed("Google Checkout Failed. If this problem persists please notify customer support.");
                            }
                        }
                    }
                }
            }
        }
    }
}