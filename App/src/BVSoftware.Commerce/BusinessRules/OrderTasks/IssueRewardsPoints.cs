using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Payment;
using BVSoftware.Commerce.Orders;
using BVSoftware.Commerce.Membership;

namespace BVSoftware.Commerce.BusinessRules.OrderTasks
{
    public class IssueRewardsPoints : OrderTask
    {

        public override Task Clone()
        {
            return new IssueRewardsPoints();
        }

        public override bool Execute(OrderTaskContext context)
        {
            if (context.Order != null)
            {
                string issued = context.Order.CustomProperties.GetProperty("bvsoftware", "rewardspointsissued");
                if (issued == "1") return true;

                // skip if there is no user account
                if (context.UserId == string.Empty) return true;

                bool hasPointsPayment = false;
                foreach (OrderTransaction t in context.BVApp.OrderServices.Transactions.FindForOrder(context.Order.bvin))
                {
                    if (t.Action == BVSoftware.Payment.ActionType.RewardPointsInfo)
                    {
                        hasPointsPayment = true;
                        break;
                    }
                }

                // Don't issue points when paying with points
                if (hasPointsPayment) return true;

                CustomerPointsManager pointsManager = CustomerPointsManager.InstantiateForDatabase(context.CurrentRequest.CurrentStore.Settings.RewardsPointsIssuedPerDollarSpent,
                                                                                context.CurrentRequest.CurrentStore.Settings.RewardsPointsNeededPerDollarCredit,
                                                                                context.CurrentRequest.CurrentStore.Id);
                int pointsToIssue = pointsManager.PointsToIssueForSpend(context.Order.TotalOrderAfterDiscounts);

                pointsManager.IssuePoints(context.Order.UserID, pointsToIssue);
                context.Order.CustomProperties.SetProperty("bvsoftware", "rewardspointsissued", "1");
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
            return "21850F22-2A10-4CFA-BFF9-813CF448E07D";
        }

        public override string TaskName()
        {
            return "Issue Rewards Points";
        }
    }
}
