
using MerchantTribe.PaypalWebServices;
using System.Web;
using GCheckout;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{	
	public class GoogleCheckoutSendTrackingInfo : OrderTask
	{

		public override Task Clone()
		{
			return new GoogleCheckoutSendTrackingInfo();
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
				foreach (Orders.OrderPackage package in context.Order.Packages) {
					if (package.HasShipped) {
						string googletrackingsent = package.CustomProperties.GetProperty("bvsoftware", "googletrackingsent");
						if (googletrackingsent == string.Empty) {
							bool trackingInfoSent = true;
							if (package.TrackingNumber.Trim() != string.Empty) {
                                MerchantTribe.Shipping.IShippingService provider = Shipping.AvailableServices.FindById(package.ShippingProviderId, currentStore);
								if (provider != null) {
                                    GCheckout.OrderProcessing.AddTrackingDataRequest AddTrackingData 
                                        = new GCheckout.OrderProcessing.AddTrackingDataRequest(currentStore.Settings.GoogleCheckout.MerchantId, 
                                                                                                currentStore.Settings.GoogleCheckout.MerchantKey, 
                                                                                                currentStore.Settings.GoogleCheckout.Mode, 
                                                                                                googleOrderNumber, 
                                                                                                provider.Name, package.TrackingNumber);
									GCheckout.Util.GCheckoutResponse response = AddTrackingData.Send();
									if (!response.IsGood) {
										trackingInfoSent = false;
                                        if (currentStore.Settings.GoogleCheckout.DebugMode)
                                        {
											EventLog.LogEvent("Google Checkout", "Google Checkout Tracking Number Notification Failed To Sent. Error Message " + response.ErrorMessage, Metrics.EventLogSeverity.Error);
										}
									}
									else {
                                        if (currentStore.Settings.GoogleCheckout.DebugMode)
                                        {
											EventLog.LogEvent("Google Checkout", "Google Checkout Tracking Number Notification Sent", Metrics.EventLogSeverity.Information);
										}
									}
								}
							}
							if (trackingInfoSent) {
								package.CustomProperties.SetProperty("bvsoftware", "googletrackingsent", "1");								
							}
						}
					}
				}
                context.BVApp.OrderServices.Orders.Update(context.Order);
			}
			return true;
		}

		public override bool Rollback(OrderTaskContext context)
		{
			return true;
		}

		public override string TaskId()
		{
			return "0a91c490-df6c-4f88-9b12-cb65615ab758";
		}

		public override string TaskName()
		{
			return "Google Checkout Send Tracking Information";
		}
	}
}

