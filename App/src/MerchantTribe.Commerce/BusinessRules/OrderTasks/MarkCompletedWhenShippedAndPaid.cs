using MerchantTribe.Commerce.Orders;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{

	public class MarkCompletedWhenShippedAndPaid : OrderTask
	{

		public override bool Execute(OrderTaskContext context)
		{
			if ((context.Order.ShippingStatus == Orders.OrderShippingStatus.FullyShipped) && (context.Order.PaymentStatus == Orders.OrderPaymentStatus.Paid)) {
				Orders.OrderStatusCode orderStatus = Orders.OrderStatusCode.FindByBvin(OrderStatusCode.Completed);
				if (orderStatus != null) {
					context.Order.StatusCode = orderStatus.Bvin;
					context.Order.StatusName = orderStatus.StatusName;
				}
			}
			return true;
		}

		public override bool Rollback(OrderTaskContext context)
		{
			context.Order.StatusCode = OrderStatusCode.Received;
			return true;
		}

		public override string TaskId()
		{
			return "19e6e637-e651-488d-a54c-11bc249ef28f";
		}

		public override string TaskName()
		{
			return "Mark Completed When Order Is Shipped And Paid";
		}

		public override Task Clone()
		{
			return new MarkCompletedWhenShippedAndPaid();
		}
	}
}

