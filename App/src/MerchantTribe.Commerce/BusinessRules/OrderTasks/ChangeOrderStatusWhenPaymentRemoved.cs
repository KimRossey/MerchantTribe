using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{
	
	public class ChangeOrderStatusWhenPaymentRemoved : OrderTask
	{

		public override bool Execute(OrderTaskContext context)
		{
			if (context.Inputs["PreviousPaymentStatus"] != null) {
				int val = 0;
				if (int.TryParse(context.Inputs["PreviousPaymentStatus"].Value,out val)) {
					if ((val == (int)Orders.OrderPaymentStatus.Paid) && (context.Order.PaymentStatus != Orders.OrderPaymentStatus.Paid)) {
						string statusCode =  Orders.OrderStatusCode.Received;						
						Orders.OrderStatusCode orderStatus = Orders.OrderStatusCode.FindByBvin(statusCode);
						if (orderStatus != null && orderStatus.Bvin != string.Empty) {
							context.Order.StatusCode = orderStatus.Bvin;
							context.Order.StatusName = orderStatus.StatusName;
						}
						else {
							EventLog.LogEvent("Change Order Status When Payment Removed", "Could not find order status with id of " + statusCode, EventLogSeverity.Error);
						}
					}
				}
			}
			return true;
		}

		public override bool Rollback(OrderTaskContext context)
		{
			context.Order.StatusCode = Orders.OrderStatusCode.Received;
			return true;
		}

		public override string TaskId()
		{
			return "4aa03a0d-1d38-4f66-ad85-82edf715103b";
		}

		public override string TaskName()
		{
			return "Change Order Status When Payment Removed";
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
			return new ChangeOrderStatusWhenPaymentRemoved();
		}
	}
}

