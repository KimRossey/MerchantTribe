
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BVSoftware.Commerce.Orders;

namespace BVSoftware.Commerce.BusinessRules.OrderTasks
{
	
	public class ReceivePaypalExpressPayments : OrderTask
	{

		public override Task Clone()
		{
			return new ReceivePaypalExpressPayments();
		}

		public override bool Execute(OrderTaskContext context)
		{
			bool result = true;

            if (context.BVApp.OrderServices.PaymentSummary(context.Order).AmountDue > 0)
            {

                foreach (OrderTransaction p in context.BVApp.OrderServices.Transactions.FindForOrder(context.Order.bvin))
                {
                    List<OrderTransaction> transactions = context.BVApp.OrderServices.Transactions.FindForOrder(context.Order.bvin);

                    if (p.Action == BVSoftware.Payment.ActionType.PayPalExpressCheckoutInfo)
                    {
                        // if we already have an auth or charge on the card, skip
                        if (p.HasSuccessfulLinkedAction(BVSoftware.Payment.ActionType.PayPalHold, transactions) ||
                            p.HasSuccessfulLinkedAction(BVSoftware.Payment.ActionType.PayPalCharge, transactions))
                        {
                            continue;
                        }

                        try
                        {
                            Orders.OrderPaymentManager payManager = new OrderPaymentManager(context.Order,
                                                                                            context.BVApp);

                            bool transactionSuccess = false;

                            if (context.CurrentRequest.CurrentStore.Settings.PayPal.ExpressAuthorizeOnly)
                            {
                                transactionSuccess = payManager.PayPalExpressHold(p, context.BVApp.OrderServices.PaymentSummary(context.Order).AmountDue);
                            }
                            else
                            {
                                transactionSuccess = payManager.PayPalExpressCharge(p, context.BVApp.OrderServices.PaymentSummary(context.Order).AmountDue);
                            }

                            if (transactionSuccess == false) result = false;
                        }
                        catch (Exception ex)
                        {
                            context.Errors.Add(new WorkflowMessage("Exception During Receive Paypal Express Payments", ex.Message + ex.StackTrace, false));
                        }
                        finally
                        {
                            Orders.OrderPaymentStatus previousPaymentStatus = context.Order.PaymentStatus;
                            context.BVApp.OrderServices.EvaluatePaymentStatus(context.Order);
                            context.Inputs.Add("bvsoftware", "PreviousPaymentStatus", previousPaymentStatus.ToString());
                            BusinessRules.Workflow.RunByName(context, WorkflowNames.PaymentChanged);
                        }
                    }
                }                                                               
			}

			if (result == false) {

				// Add Error
                bool throwErrors = true;
				//throwErrors = this.SettingsManager.GetBooleanSetting("ThrowErrors");
				if (throwErrors == true) {
					if (result == false) {
                        string errorString = string.Empty; // this.SettingsManager.GetSetting("CustomerErrorMessage");
						if (errorString == string.Empty) {
							errorString = "An error occured while attempting to process your Paypal Express payment. Please check your payment information and try again";
						}
						context.Errors.Add(new WorkflowMessage("Receive Card Failed", errorString, true));
					}
				}
				else {
					// Hide Error
					result = true;
				}

				// Failure Status Code
				bool SetStatus = false;
                SetStatus = true; // this.SettingsManager.GetBooleanSetting("SetStatusOnFail");
				if (SetStatus == true) {
					string failCode = Orders.OrderStatusCode.OnHold;					
					Orders.OrderStatusCode c = Orders.OrderStatusCode.FindByBvin(failCode);
					if (c != null) {
						context.Order.StatusCode = c.Bvin;
						context.Order.StatusName = c.StatusName;
					}
				}

			}
			return result;
		}

		public override bool Rollback(OrderTaskContext context)
		{
			return true;
		}

		public override string TaskId()
		{
			return "37e0b27e-567f-4ee3-95ec-c7e5a66bfb26";
		}

		public override string TaskName()
		{
			return "Receive Paypal Express Payments";
		}

	}
}
