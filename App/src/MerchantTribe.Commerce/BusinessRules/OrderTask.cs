using System;

namespace MerchantTribe.Commerce.BusinessRules
{
	public abstract class OrderTask : Task
	{

		public override bool Execute(TaskContext context)
		{
			return Execute((OrderTaskContext)context);
		}

		public override bool Rollback(TaskContext context)
		{
			return Rollback((OrderTaskContext)context);
		}

		public abstract bool Execute(OrderTaskContext context);

		public abstract bool Rollback(OrderTaskContext context);

	}
}

