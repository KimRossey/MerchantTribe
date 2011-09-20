using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Membership;

namespace MerchantTribeStore.BVModules.Controls
{
    public partial class PaymentRewardsPoints : MerchantTribe.Commerce.Content.BVUserControl
    {
        private CustomerPointsManager pointsManager = null;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            pointsManager = CustomerPointsManager.InstantiateForDatabase(MyPage.MTApp.CurrentStore.Settings.RewardsPointsIssuedPerDollarSpent,
                                                      MyPage.MTApp.CurrentStore.Settings.RewardsPointsNeededPerDollarCredit,
                                                      MyPage.MTApp.CurrentStore.Id);
        }

        protected override void OnLoad(EventArgs e)
        {            
            base.OnLoad(e);            
        }

        public void Populate(Order o)
        {
            if (o == null) return;

            string userId = o.UserID;
            if (userId == string.Empty) return;
            
            int points = pointsManager.FindAvailablePoints(userId);            
            int potentialPointsToUse = pointsManager.PointsNeededForPurchaseAmount(o.TotalOrderAfterDiscounts);
            int amountToUse = 0;
            if (points > potentialPointsToUse)
            {
                amountToUse = potentialPointsToUse;
            }
            else
            {
                amountToUse = points;
            }

            string rewardsName = MyPage.MTApp.CurrentStore.Settings.RewardsPointsName;

            this.lblPointsAvailable.Text = "You have " + points.ToString() + " " + rewardsName + " available.";
            this.rbUsePoints.Text = "Use " + amountToUse.ToString() + " " + rewardsName;
        }

        public decimal PotentialCredit(Order o)
        {
            decimal result = 0;
            if (rbNoPoints.Checked) return result;

            int points = pointsManager.FindAvailablePoints(o.UserID);
            int potentialPointsToUse = pointsManager.PointsNeededForPurchaseAmount(o.TotalOrderAfterDiscounts);
            int amountToUse = 0;
            if (points > potentialPointsToUse)
            {
                amountToUse = potentialPointsToUse;
            }
            else
            {
                amountToUse = points;
            }

            result = pointsManager.DollarCreditForPoints(amountToUse);

            return result;
        }

        public void ApplyInfoToOrder(Order o)
        {            
            // Remove any current points info transactions
            foreach (OrderTransaction t in MyPage.MTApp.OrderServices.Transactions.FindForOrder(o.bvin))
            {
                if (t.Action == MerchantTribe.Payment.ActionType.RewardPointsInfo)
                {
                    MyPage.MTApp.OrderServices.Transactions.Delete(t.Id);
                }
            }

            // Don't add if we're not using points
            if (rbNoPoints.Checked) return;

            // Apply Info to Order
            OrderPaymentManager payManager = new OrderPaymentManager(o, MyPage.MTApp);
            payManager.RewardsPointsAddInfo(PotentialCredit(o));
        }
    }
}