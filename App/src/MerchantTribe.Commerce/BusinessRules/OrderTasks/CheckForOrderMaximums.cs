using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{
    public class CheckForOrderMaximums : OrderTask
    {

        public override Task Clone()
        {
            return new CheckForOrderMaximums();
        }

        public override bool Execute(OrderTaskContext context)
        {
            int      maxItems  = context.MTApp.CurrentRequestContext.CurrentStore.Settings.MaxItemsPerOrder;
            if (maxItems <= 0) maxItems = 99999;
            decimal maxWeight = context.MTApp.CurrentRequestContext.CurrentStore.Settings.MaxWeightPerOrder;
            if (maxWeight <= 0) maxWeight = 99999;
            string maxMessage = context.MTApp.CurrentRequestContext.CurrentStore.Settings.MaxOrderMessage;

            int totalItems = context.Order.Items.Sum(y => y.Quantity);
            decimal totalWeight = context.Order.TotalWeightOfShippingItems();

            if ((totalItems > maxItems) || (totalWeight > maxWeight))
            {
                context.Errors.Add(new BusinessRules.WorkflowMessage("Order Too Large", maxMessage, true));
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool Rollback(OrderTaskContext context)
        {
            return true;
        }

        public override string TaskId()
        {
            return "D39354D5-6CD5-4a77-95E4-15D7609AA164";
        }

        public override string TaskName()
        {
            return "Check For Order Maximums";
        }
    }
}
