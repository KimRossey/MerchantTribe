using System;
using System.Collections.Generic;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{
	
	public class CompleteCreditCards : OrderTask
	{

		public override Task Clone()
		{
			return new CompleteCreditCards();
		}

		public override bool Execute(OrderTaskContext context)
		{
			bool result = true;

			foreach (Orders.OrderTransaction p in context.MTApp.OrderServices.Transactions.FindForOrder(context.Order.bvin)) {

                List<Orders.OrderTransaction> transactions = context.MTApp.OrderServices.Transactions.FindForOrder(context.Order.bvin);

                if (p.Action == MerchantTribe.Payment.ActionType.CreditCardInfo ||
                    p.Action == MerchantTribe.Payment.ActionType.CreditCardHold)
                {
                    // if we already have an auth or charge on the card, skip
                    if (p.HasSuccessfulLinkedAction(MerchantTribe.Payment.ActionType.CreditCardCharge, transactions) ||
                        p.HasSuccessfulLinkedAction(MerchantTribe.Payment.ActionType.CreditCardHold, transactions))
                    {
                        continue;
                    }

                    try
                    {
                        MerchantTribe.Payment.Transaction t = context.Order.GetEmptyTransaction();
                        t.Card = p.CreditCard;
                        t.Amount = p.Amount;

                        if (p.Action == MerchantTribe.Payment.ActionType.CreditCardHold)
                        {
                            t.Action = MerchantTribe.Payment.ActionType.CreditCardCapture;
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
                        context.Errors.Add(new WorkflowMessage("Exception During Complete Credit Card", ex.Message + ex.StackTrace, false));
                    }
                }

                Orders.OrderPaymentStatus previousPaymentStatus = context.Order.PaymentStatus;
                context.MTApp.OrderServices.EvaluatePaymentStatus(context.Order);
                context.Inputs.Add("bvsoftware", "PreviousPaymentStatus", previousPaymentStatus.ToString());
                BusinessRules.Workflow.RunByName(context, WorkflowNames.PaymentChanged);
			}

			return result;
		}

		public override bool Rollback(OrderTaskContext context)
		{
			return true;
		}

		public override string TaskId()
		{
			return "6ADB5700-49DB-4527-946B-E98FC5A7CD2D";
		}

		public override string TaskName()
		{
			return "Complete Credit Cards";
		}
	}
}
