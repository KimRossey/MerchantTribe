using MerchantTribe.Commerce;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{	
	public class UpdateLineItemsForSave : OrderTask
	{

		public override bool Execute(OrderTaskContext context)
		{
            context.Order.EvaluateCurrentShippingStatus();
            context.MTApp.OrderServices.Orders.Update(context.Order);
			return true;
		}

		public override bool Rollback(OrderTaskContext context)
		{
			return true;
		}

		public override string TaskId()
		{
			return "6E27D187-F367-40f8-8231-160B6E22AB86";
		}

		public override string TaskName()
		{
			return "Update Line Items for Save";
		}

		public override string StepName()
		{
			string result = string.Empty;
			if (result == string.Empty) {
				result = this.TaskName();
			}
			return result;
		}

		public override Task Clone()
		{
			return new UpdateLineItemsForSave();
		}
	}
}

