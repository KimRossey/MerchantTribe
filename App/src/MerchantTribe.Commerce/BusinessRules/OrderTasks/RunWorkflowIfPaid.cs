
namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{
	
	public class RunWorkFlowIfPaid : BusinessRules.OrderTask
	{

		public override BusinessRules.Task Clone()
		{
			return new RunWorkFlowIfPaid();
		}

		public override bool Execute(BusinessRules.OrderTaskContext context)
		{
			if (context.Order.PaymentStatus == Orders.OrderPaymentStatus.Paid) {
				bool val = BusinessRules.Workflow.RunByName(context, WorkflowNames.PaymentComplete);
				if (!val) {
					context.Errors.Add(new BusinessRules.WorkflowMessage("Error", "An Error Occurred While Trying To Process Your Request, Please Try Again.", true));
				}
				return val;
			}
			else {
				return true;
			}
		}

		public override bool Rollback(BusinessRules.OrderTaskContext context)
		{
			return true;
		}

		public override string TaskId()
		{
			return "6565A823-1591-4069-8005-2FEE50E4C9E9";
		}

		public override string TaskName()
		{
			return "Run Payment Complete Workflow if Paid";
		}

	}


}
