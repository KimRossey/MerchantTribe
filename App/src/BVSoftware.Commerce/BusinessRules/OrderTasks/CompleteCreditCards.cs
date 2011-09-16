using System;
using System.Collections.Generic;

namespace BVSoftware.Commerce.BusinessRules.OrderTasks
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

			foreach (Orders.OrderTransaction p in context.BVApp.OrderServices.Transactions.FindForOrder(context.Order.bvin)) {

                List<Orders.OrderTransaction> transactions = context.BVApp.OrderServices.Transactions.FindForOrder(context.Order.bvin);

                if (p.Action == BVSoftware.Payment.ActionType.CreditCardInfo ||
                    p.Action == BVSoftware.Payment.ActionType.CreditCardHold)
                {
                    // if we already have an auth or charge on the card, skip
                    if (p.HasSuccessfulLinkedAction(BVSoftware.Payment.ActionType.CreditCardCharge, transactions) ||
                        p.HasSuccessfulLinkedAction(BVSoftware.Payment.ActionType.CreditCardHold, transactions))
                    {
                        continue;
                    }

                    try
                    {
                        BVSoftware.Payment.Transaction t = context.Order.GetEmptyTransaction();
                        t.Card = p.CreditCard;
                        t.Amount = p.Amount;

                        if (p.Action == BVSoftware.Payment.ActionType.CreditCardHold)
                        {
                            t.Action = BVSoftware.Payment.ActionType.CreditCardCapture;
                        }
                        else
                        {
                            t.Action = BVSoftware.Payment.ActionType.CreditCardCharge;
                        }

                        BVSoftware.Payment.Method proc = context.CurrentRequest.CurrentStore.Settings.PaymentCurrentCreditCardProcessor();
                        proc.ProcessTransaction(t);

                        Orders.OrderTransaction ot = new Orders.OrderTransaction(t);
                        ot.LinkedToTransaction = p.IdAsString;

                        context.BVApp.OrderServices.AddPaymentTransactionToOrder(context.Order, ot, context.BVApp);

                        if (t.Result.Succeeded == false) result = false;

                    }
                    catch (Exception ex)
                    {
                        context.Errors.Add(new WorkflowMessage("Exception During Complete Credit Card", ex.Message + ex.StackTrace, false));
                    }
                }

                Orders.OrderPaymentStatus previousPaymentStatus = context.Order.PaymentStatus;
                context.BVApp.OrderServices.EvaluatePaymentStatus(context.Order);
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
