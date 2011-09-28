using System;
using System.Collections.Generic;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{

	public class ReceiveCreditCards : OrderTask
	{

		public override Task Clone()
		{
			return new ReceiveCreditCards();
		}

		public override bool Execute(OrderTaskContext context)
		{
			bool result = true;
            if (context.MTApp.OrderServices.PaymentSummary(context.Order).AmountDue > 0)
            {

                foreach (Orders.OrderTransaction p in context.MTApp.OrderServices.Transactions.FindForOrder(context.Order.bvin))
                {
                    List<Orders.OrderTransaction> transactions = context.MTApp.OrderServices.Transactions.FindForOrder(context.Order.bvin);

                    if (p.Action == MerchantTribe.Payment.ActionType.CreditCardInfo)
                    {
                        // if we already have an auth or charge on the card, skip
                        if (p.HasSuccessfulLinkedAction(MerchantTribe.Payment.ActionType.CreditCardCharge, transactions) ||
                            p.HasSuccessfulLinkedAction(MerchantTribe.Payment.ActionType.CreditCardHold, transactions))
                        {
                            Orders.OrderNote note = new Orders.OrderNote();
                            note.IsPublic = false;
                            note.Note = "Skipping receive for credit card info because auth or charge already exists. Transaction " + p.Id;
                            context.Order.Notes.Add(note);
                            continue;
                        }

                        try
                        {
                            MerchantTribe.Payment.Transaction t = context.Order.GetEmptyTransaction();
                            t.Card = p.CreditCard;
                            t.Amount = p.Amount;

                            if (context.MTApp.CurrentRequestContext.CurrentStore.Settings.PaymentCreditCardAuthorizeOnly == true)
                            {
                                t.Action = MerchantTribe.Payment.ActionType.CreditCardHold;
                            }
                            else
                            {
                                t.Action = MerchantTribe.Payment.ActionType.CreditCardCharge;
                            }

                            MerchantTribe.Payment.Method proc = context.MTApp.CurrentRequestContext.CurrentStore.Settings.PaymentCurrentCreditCardProcessor();
                            proc.ProcessTransaction(t);

                            Orders.OrderTransaction ot = new Orders.OrderTransaction(t);
                            ot.LinkedToTransaction = p.IdAsString;
                            context.MTApp.OrderServices.AddPaymentTransactionToOrder(context.Order, ot, context.MTApp);

                            if (t.Result.Succeeded == false) result = false;

                        }
                        catch (Exception ex)
                        {
                            context.Errors.Add(new WorkflowMessage("Exception During Receive Credit Card", ex.Message + ex.StackTrace, false));
                            Orders.OrderNote note = new Orders.OrderNote();
                            note.IsPublic = false;
                            note.Note = "EXCEPTION: " + ex.Message + " | " + ex.StackTrace;
                            context.Order.Notes.Add(note);
                        }

                    }
                }

                // Evaluate Payment Status After Receiving Payments
                Orders.OrderPaymentStatus previousPaymentStatus = context.Order.PaymentStatus;
                context.MTApp.OrderServices.EvaluatePaymentStatus(context.Order);
                context.Inputs.Add("bvsoftware", "PreviousPaymentStatus", previousPaymentStatus.ToString());
                BusinessRules.Workflow.RunByName(context, WorkflowNames.PaymentChanged);
            }
            else
            {
                Orders.OrderNote note = new Orders.OrderNote();
                note.IsPublic = false;
                note.Note = "Amount due was less than zero. Skipping receive credit cards";
                context.Order.Notes.Add(note);             
            }

            

			if (result == false) {

				// Add Error
				bool throwErrors = false;
                throwErrors = context.MTApp.CurrentRequestContext.CurrentStore.Settings.RejectFailedCreditCardOrdersAutomatically;
				if (throwErrors == true) {
					if (result == false) {
                        string errorString = string.Empty; // this.SettingsManager.GetSetting("CustomerErrorMessage");
						if (errorString == string.Empty) {
							errorString = "An error occured while attempting to process your credit card. Please check your payment information and try again";
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
			return "253305A6-87A4-4bcc-AA98-8A082E2D8162";
		}

		public override string TaskName()
		{
			return "Receive Credit Cards";
		}

	}
}

