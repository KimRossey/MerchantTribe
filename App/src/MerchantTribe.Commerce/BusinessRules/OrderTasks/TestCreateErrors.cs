using System;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{
	public class TestCreateErrors : OrderTask
	{

		public override Task Clone()
		{
			return new TestCreateErrors();
		}

		public override bool Execute(OrderTaskContext context)
		{
			for (int i = 0; i <= 5; i++) {
				WorkflowMessage errorMessage = new WorkflowMessage("Error", "This error is error number " + i.ToString(), true);
				context.Errors.Add(errorMessage);
			}
			return false;
		}

		public override bool Rollback(OrderTaskContext context)
		{
			return true;
		}

		public override string TaskId()
		{
			return "a8f7e43f-6fea-4a02-a4aa-74af6ef6320c";
		}

		public override string TaskName()
		{
			return "Test Create Errors";
		}
	}
}
