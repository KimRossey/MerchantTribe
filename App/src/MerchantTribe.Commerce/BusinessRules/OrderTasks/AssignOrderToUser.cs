
namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{
	
	public class AssignOrderToUser : BusinessRules.OrderTask
	{

		public override bool Execute(OrderTaskContext context)
		{
			context.Order.UserID = context.UserId;
			return true;
		}

		public override bool Rollback(OrderTaskContext context)
		{
			context.Order.UserID = string.Empty;
			return true;
		}

		public override string TaskId()
		{
			return "71EC229F-F0EF-4a6e-9790-9E84DC8DCA09";
		}

		public override string TaskName()
		{
			return "Assign Order To User";
		}

		public override string StepName()
		{
			string result = string.Empty;
            result = "Assign Order to User";
			if (result == string.Empty) {
				result = this.TaskName();
			}
			return result;
		}

		public override Task Clone()
		{
			return new AssignOrderToUser();
		}

	}
}

