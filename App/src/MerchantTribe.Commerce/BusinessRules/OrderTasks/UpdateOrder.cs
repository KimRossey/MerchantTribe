using System;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.BusinessRules;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Orders;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{	
	public class UpdateOrder : OrderTask
	{

		public override bool Execute(OrderTaskContext context)
		{
            return context.MTApp.OrderServices.Orders.Update(context.Order);
		}

		public override bool Rollback(OrderTaskContext context)
		{
			return true;
		}

		public override string TaskId()
		{
			return "30B9B11C-1621-4779-ABC2-FEC5D280484B";
		}

		public override string TaskName()
		{
			return "Update Order";
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
			return new UpdateOrder();
		}

	}
}

