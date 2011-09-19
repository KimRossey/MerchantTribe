using System;

namespace MerchantTribe.Commerce.BusinessRules
{
	public abstract class ShippingTask : Task
	{

		public override bool Execute(TaskContext context)
		{
			return Execute((ShippingTaskContext)context);
		}

		public override bool Rollback(TaskContext context)
		{
			return Rollback((ShippingTaskContext)context);
		}

		public abstract bool Execute(ShippingTaskContext context);

		public abstract bool Rollback(ShippingTaskContext context);

	}
}

