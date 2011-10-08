using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{	
	public class ChangeOrderStatusWhenShipmentRemoved : OrderTask
	{

		public override bool Execute(OrderTaskContext context)
		{
			if (context.Inputs["PreviousShippingStatus"] != null) {
				int val = 0;
				if (int.TryParse(context.Inputs["PreviousShippingStatus"].Value,out val)) {
					if ((val == (int)Orders.OrderShippingStatus.FullyShipped) && (context.Order.ShippingStatus != Orders.OrderShippingStatus.FullyShipped)) {
						if (context.Order.ShippingStatus != Orders.OrderShippingStatus.NonShipping) {
							string statusCode =  Orders.OrderStatusCode.Received;
			                Orders.OrderStatusCode orderStatus = Orders.OrderStatusCode.FindByBvin(statusCode);
							if (orderStatus != null && orderStatus.Bvin != string.Empty) {
								context.Order.StatusCode = orderStatus.Bvin;
								context.Order.StatusName = orderStatus.StatusName;
							}
							else {
								EventLog.LogEvent("Change Order Status When Shipment Removed", "Could not find order status with id of " + statusCode, EventLogSeverity.Error);
							}
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
			return "b5d9c29d-2761-439d-92a4-6ae35870ba5b";
		}

		public override string TaskName()
		{
			return "Change Order Status When Shipment Removed";
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
			return new ChangeOrderStatusWhenShipmentRemoved();
		}
	}
}
