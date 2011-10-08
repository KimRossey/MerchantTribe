
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MerchantTribe.PaypalWebServices;
using com.paypal.soap.api;
using System.Web;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{	
	public class StartPaypalExpressCheckout : OrderTask
	{

		public override Task Clone()
		{
			return new StartPaypalExpressCheckout();
		}

		public override bool Execute(OrderTaskContext context)
		{
			if (context.Inputs["Mode"] != null) {
				if (context.Inputs["Mode"].Value == "PaypalExpress") {
					if (HttpContext.Current != null) {
						PayPalAPI ppAPI = Utilities.PaypalExpressUtilities.GetPaypalAPI();
						try {

                            string cartReturnUrl = string.Empty;
                            string cartCancelUrl = string.Empty;
                            if (context.MTApp.CurrentRequestContext != null)
                            {
                                cartReturnUrl = context.MTApp.CurrentRequestContext.CurrentStore.RootUrlSecure() + "paypalexpresscheckout";
                                cartCancelUrl = context.MTApp.CurrentRequestContext.CurrentStore.RootUrlSecure() + "checkout";
                            }

							SetExpressCheckoutResponseType expressResponse;
							PaymentActionCodeType mode = PaymentActionCodeType.None;

                            if (context.MTApp.CurrentRequestContext.CurrentStore.Settings.PayPal.ExpressAuthorizeOnly)
                            {
								mode = PaymentActionCodeType.Order;
							}
							else {
								mode = PaymentActionCodeType.Sale;
							}

                            // Accelerated boarding
                            if (context.MTApp.CurrentRequestContext.CurrentStore.Settings.PayPal.UserName.Trim().Length < 1) mode = PaymentActionCodeType.Sale;


							bool addressSupplied = false;
							if (context.Inputs["AddressSupplied"] != null) {
								if (context.Inputs["AddressSupplied"].Value == "1") {
									addressSupplied = true;
									context.Order.CustomProperties.Add("bvsoftware", "PaypalAddressOverride", "1");
								}
							}

							string amountToPayPal = context.Order.TotalOrderBeforeDiscounts.ToString("N", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));

							if (addressSupplied) {
								Contacts.Address address = context.Order.ShippingAddress;

								MerchantTribe.Web.Geography.Country country = MerchantTribe.Web.Geography.Country.FindByBvin(address.CountryBvin);
								if (country != null) {
                                    expressResponse = ppAPI.SetExpressCheckout(
                                        amountToPayPal, 
                                        cartReturnUrl, 
                                        cartCancelUrl, 
                                        mode,
                                        PayPalAPI.GetCurrencyCodeType(context.MTApp.CurrentRequestContext.CurrentStore.Settings.PayPal.Currency), 
                                        address.FirstName + " " + address.LastName, 
                                        country.IsoCode, 
                                        address.Line1, 
                                        address.Line2, 
                                        address.City,
									    address.RegionBvin, 
                                        address.PostalCode, 
                                        address.Phone, 
                                        context.Order.OrderNumber + System.Guid.NewGuid().ToString());
								}
								else {
									EventLog.LogEvent("StartPaypalExpressCheckout", "Country with bvin " + address.CountryBvin + " was not found.", EventLogSeverity.Error);
									return false;
								}
							}
							else {
                                expressResponse = ppAPI.SetExpressCheckout(amountToPayPal, 
                                                                        cartReturnUrl, 
                                                                        cartCancelUrl, 
                                                                        mode,
                                                                        PayPalAPI.GetCurrencyCodeType(context.MTApp.CurrentRequestContext.CurrentStore.Settings.PayPal.Currency), 
                                                                        context.Order.OrderNumber + System.Guid.NewGuid().ToString());
							}

							if (expressResponse.Ack == AckCodeType.Success || expressResponse.Ack == AckCodeType.SuccessWithWarning) {
								context.Order.ThirdPartyOrderId = expressResponse.Token;
								
                                // Recording of this info is handled on the paypal express
                                // checkout page instead of here.
                                //Orders.OrderPaymentManager payManager = new Orders.OrderPaymentManager(context.Order);
                                //payManager.PayPalExpressAddInfo(context.Order.TotalGrand, expressResponse.Token);

                                Orders.OrderNote note = new Orders.OrderNote();
                                note.IsPublic = false;
								note.Note = "Paypal Order Accepted With Paypal Order Number: " + expressResponse.Token;
								context.Order.Notes.Add(note);
                                if (context.MTApp.OrderServices.Orders.Update(context.Order))
                                {
                                    if (string.Compare(context.MTApp.CurrentRequestContext.CurrentStore.Settings.PayPal.Mode, "Live", true) == 0)
                                    {
										HttpContext.Current.Response.Redirect("https://www.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=" + expressResponse.Token, false);
									}
									else {
										HttpContext.Current.Response.Redirect("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=" + expressResponse.Token, false);
									}

								}
								return true;
							}
							else {
								foreach (ErrorType ppError in expressResponse.Errors) {

                                    context.Errors.Add(new WorkflowMessage(ppError.ErrorCode,ppError.ShortMessage, true));

									//create a note to save the paypal error info onto the order
									Orders.OrderNote note = new Orders.OrderNote();
                                    note.IsPublic = false;
									note.Note = "Paypal error number: " + ppError.ErrorCode + " Paypal Error: '" + ppError.ShortMessage + "' Message: '" + ppError.LongMessage;
									context.Order.Notes.Add(note);

									EventLog.LogEvent("Paypal error number: " + ppError.ErrorCode, "Paypal Error: '" + ppError.ShortMessage + "' Message: '" + ppError.LongMessage + "' " + " Values passed to SetExpressCheckout: Total=" + string.Format("{0:c}", context.Order.TotalOrderBeforeDiscounts) + " Cart Return Url: " + cartReturnUrl + " Cart Cancel Url: " + cartCancelUrl, EventLogSeverity.Error);
								}
								context.Errors.Add(new WorkflowMessage("Paypal checkout error", Content.SiteTerms.GetTerm(Content.SiteTermIds.PaypalCheckoutCustomerError), true));
								return false;
							}
						}
						catch (Exception ex) {
							EventLog.LogEvent("Paypal Express Checkout", "Exception occurred during call to Paypal: " + ex.ToString(), EventLogSeverity.Error);
							context.Errors.Add(new WorkflowMessage("Paypal checkout error", Content.SiteTerms.GetTerm(Content.SiteTermIds.PaypalCheckoutCustomerError), true));
							return false;
						}
						finally {
							ppAPI = null;
						}
					}
				}
				else {
					return true;
				}
			}
			else {
				return true;
			}

            return false;
		}

		public override bool Rollback(OrderTaskContext context)
		{
			return true;
		}

		public override string TaskId()
		{
			return "56597582-05d0-4b51-bd87-7426a9cf146f";
		}

		public override string TaskName()
		{
			return "Start Paypal Express Checkout";
		}
	}
}
