using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.Commerce.Membership;

namespace BVSoftware.Commerce.BusinessRules.OrderTasks
{
    public class ReceiveRewardsPoints : OrderTask
    {

        public override Task Clone()
        {
            return new ReceiveRewardsPoints();
        }

        public override bool Execute(OrderTaskContext context)
        {
            bool result = true;
            if (context.BVApp.OrderServices.PaymentSummary(context.Order).AmountDue > 0)
            {
                CustomerPointsManager pointsManager = CustomerPointsManager.InstantiateForDatabase(context.CurrentRequest.CurrentStore.Settings.RewardsPointsIssuedPerDollarSpent,
                                                                                context.CurrentRequest.CurrentStore.Settings.RewardsPointsNeededPerDollarCredit,
                                                                                context.CurrentRequest.CurrentStore.Id);
                Orders.OrderPaymentManager payManager = new Orders.OrderPaymentManager(context.Order,
                                                                                       context.BVApp);

                foreach (Orders.OrderTransaction p in context.BVApp.OrderServices.Transactions.FindForOrder(context.Order.bvin))
                {
                    List<Orders.OrderTransaction> transactions = context.BVApp.OrderServices.Transactions.FindForOrder(context.Order.bvin);

                    if (p.Action == BVSoftware.Payment.ActionType.RewardPointsInfo)
                    {
                        // if we already have an auth or charge on the card, skip
                        if (p.HasSuccessfulLinkedAction(BVSoftware.Payment.ActionType.RewardPointsDecrease, transactions) ||
                            p.HasSuccessfulLinkedAction(BVSoftware.Payment.ActionType.RewardPointsHold, transactions))
                        {
                            continue;
                        }

                        try
                        {
                            payManager.RewardsPointsHold(p, p.Amount);                                                     
                        }
                        catch (Exception ex)
                        {
                            context.Errors.Add(new WorkflowMessage("Exception During Receive Credit Card", ex.Message + ex.StackTrace, false));
                        }

                    }
                }

                // Evaluate Payment Status After Receiving Payments
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
            return "A912F623-9210-4DDB-B8E1-9E366E9520F9";
        }

        public override string TaskName()
        {
            return "Receive Rewards Points";
        }

    }
}

