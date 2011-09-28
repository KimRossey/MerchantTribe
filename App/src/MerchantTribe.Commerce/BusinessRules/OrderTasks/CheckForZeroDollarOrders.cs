using System;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{
	public class CheckForZeroDollarOrders : OrderTask
	{

		public override Task Clone()
		{
			return new TestCreateErrors();
		}

		public override bool Execute(OrderTaskContext context)
		{
            bool allowed = false;
            if (context.MTApp.CurrentRequestContext != null)
            {
                allowed = context.MTApp.CurrentRequestContext.CurrentStore.Settings.AllowZeroDollarOrders;
            }
			if (!allowed) {
				if (context.Order.TotalOrderBeforeDiscounts - context.Order.TotalOrderDiscounts <= 0) {
					WorkflowMessage errorMessage = new WorkflowMessage("Error", "Zero dollar orders are not allowed on this store.", true);
					context.Errors.Add(errorMessage);
					return false;
				}
			}

            if (context.Order.Items.Count < 1)
            {
                WorkflowMessage error2 = new WorkflowMessage("Error", "The system was unable to process your order and may be busy. Please try again.", true);
                context.Errors.Add(error2);
                return false;
            }
			return true;
		}

		public override bool Rollback(OrderTaskContext context)
		{
			return true;
		}

		public override string TaskId()
		{
			return "A2D0FC29-CC3C-4f6b-ABC8-163D9543A1A8";
		}

		public override string TaskName()
		{
			return "Check for Zero Dollar Orders";
		}
	}
}

