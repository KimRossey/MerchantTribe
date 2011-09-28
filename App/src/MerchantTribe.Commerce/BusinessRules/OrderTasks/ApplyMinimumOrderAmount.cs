
namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{
	
	public class ApplyMinimumOrderAmount : OrderTask
	{

		public override Task Clone()
		{
			return new ApplyMinimumOrderAmount();
		}

		public override bool Execute(OrderTaskContext context)
		{
			if (context.Order.TotalOrderBeforeDiscounts < context.MTApp.CurrentRequestContext.CurrentStore.Settings.MinumumOrderAmount) {
                context.Errors.Add(new BusinessRules.WorkflowMessage("Minimum Order Amount", context.MTApp.CurrentRequestContext.CurrentStore.Settings.MinumumOrderAmount.ToString("c"), true));
				return false;
			}
			else {
				return true;
			}
		}

		public override bool Rollback(OrderTaskContext context)
		{
			return true;
		}

		public override string TaskId()
		{
			return "a07ed476-3165-4842-a4bf-ab40c8054501";
		}

		public override string TaskName()
		{
			return "Apply Minimum Order Amount";
		}
	}
}
