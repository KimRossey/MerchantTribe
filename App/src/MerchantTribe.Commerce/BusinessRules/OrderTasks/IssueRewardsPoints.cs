using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Membership;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
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
                foreach (OrderTransaction t in context.MTApp.OrderServices.Transactions.FindForOrder(context.Order.bvin))
                {
                    if (t.Action == MerchantTribe.Payment.ActionType.RewardPointsInfo)
                    {
                        hasPointsPayment = true;
                        break;
                    }
                }

                // Don't issue points when paying with points
                if (hasPointsPayment) return true;

                CustomerPointsManager pointsManager = CustomerPointsManager.InstantiateForDatabase(context.MTApp.CurrentRequestContext.CurrentStore.Settings.RewardsPointsIssuedPerDollarSpent,
                                                                                context.MTApp.CurrentRequestContext.CurrentStore.Settings.RewardsPointsNeededPerDollarCredit,
                                                                                context.MTApp.CurrentRequestContext.CurrentStore.Id);
                int pointsToIssue = pointsManager.PointsToIssueForSpend(context.Order.TotalOrderAfterDiscounts);

                pointsManager.IssuePoints(context.Order.UserID, pointsToIssue);
                context.Order.CustomProperties.SetProperty("bvsoftware", "rewardspointsissued", "1");
                context.MTApp.OrderServices.Orders.Update(context.Order);
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
