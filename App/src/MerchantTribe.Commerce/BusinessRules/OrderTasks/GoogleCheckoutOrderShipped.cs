
using System;
using MerchantTribe.PaypalWebServices;
using System.Web;
using GCheckout;
using GCheckout.OrderProcessing;
using GCheckout.Checkout;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{	
	public class GoogleCheckoutOrderShipped : OrderTask
	{

		public override Task Clone()
		{
			return new GoogleCheckoutOrderShipped();
		}

		public override bool Execute(OrderTaskContext context)
		{
            Accounts.Store currentStore = context.CurrentRequest.CurrentStore;

			bool googleCheckoutUsed = false;
			string googleOrderNumber = string.Empty;
            //foreach (Orders.OrderPayment payment in context.Order.Payments) {
            //    if (payment.PaymentMethodId == WebAppSettings.PaymentIdGoogleCheckout) {
            //        googleCheckoutUsed = true;
            //        googleOrderNumber = payment.ThirdPartyOrderId;
            //        break; 
            //    }
            //}

			if (googleCheckoutUsed) {
                DeliverOrderRequest packageShipped = new DeliverOrderRequest(currentStore.Settings.GoogleCheckout.MerchantId,
                                                                    currentStore.Settings.GoogleCheckout.MerchantKey, 
                                                                    currentStore.Settings.GoogleCheckout.Mode, 
                                                                    googleOrderNumber);
				GCheckout.Util.GCheckoutResponse response = packageShipped.Send();

                if (currentStore.Settings.GoogleCheckout.DebugMode)
                {
					if (response.IsGood) {
						EventLog.LogEvent("Google Checkout", "Google Checkout Order Shipped Notification Sent", Metrics.EventLogSeverity.Information);
					}
					else {
						EventLog.LogEvent("Google Checkout", "Google Checkout Order Shipped Notification Failed To Send. Order Id: " + googleOrderNumber + " Error Message: " + response.ErrorMessage, Metrics.EventLogSeverity.Error);
					}
				}

				if (!response.IsGood) {
					if (response.ErrorMessage != string.Empty) {
						Orders.OrderNote note = new Orders.OrderNote();
						note.IsPublic = false;
						note.Note = response.ErrorMessage;
						context.Order.Notes.Add(note);
                        context.BVApp.OrderServices.Orders.Update(context.Order);
					}
				}
			}
			return true;
		}

		public override bool Rollback(OrderTaskContext context)
		{
			return true;
		}

		public override string TaskId()
		{
			return "9e1f141c-e9bf-4df5-801b-35b623dae3a8";
		}

		public override string TaskName()
		{
			return "Google Checkout Send Order Shipped Notification";
		}
	}
}

