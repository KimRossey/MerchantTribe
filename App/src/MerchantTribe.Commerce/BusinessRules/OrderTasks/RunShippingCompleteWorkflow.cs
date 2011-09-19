
namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{
	public class RunShippingCompleteWorkFlow : BusinessRules.OrderTask
	{

		public override Task Clone()
		{
			return new RunShippingCompleteWorkFlow();
		}

		public override bool Execute(OrderTaskContext context)
		{
			if (context.Order.ShippingStatus == Orders.OrderShippingStatus.FullyShipped) {
				return BusinessRules.Workflow.RunByName(context, WorkflowNames.ShippingComplete);
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
			return "5ba90d63-ff6c-4ca4-8e34-7705ec3b2728";
		}

		public override string TaskName()
		{
			return "Run Shipping Complete Workflow If Needed";
		}
	}
}
