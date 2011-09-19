using System;

namespace MerchantTribe.Commerce.BusinessRules
{
	public class NullTask : Task
	{

		public override string TaskName()
		{
			return "Null Task";
		}

		public override bool Execute(TaskContext context)
		{
			return false;
		}

		public override bool Rollback(TaskContext context)
		{
			return true;
		}

		public override string TaskId()
		{
			return string.Empty;
		}

		public override Task Clone()
		{
			NullTask result = new NullTask();
			return result;
		}

	}
}

