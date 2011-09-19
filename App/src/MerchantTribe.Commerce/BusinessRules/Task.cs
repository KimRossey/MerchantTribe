using System;
using System.Collections.ObjectModel;
using System.Data;

namespace MerchantTribe.Commerce.BusinessRules
{
	public abstract class Task
	{

		public abstract string TaskName();

		public abstract string TaskId();

		public abstract bool Execute(TaskContext context);

		public abstract bool Rollback(TaskContext context);

		public virtual string StepName()
		{
			return TaskName();
		}

		public abstract Task Clone();

	}
}

