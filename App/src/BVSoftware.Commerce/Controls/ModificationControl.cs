using System;

namespace BVSoftware.Commerce.Controls
{
	public abstract class ModificationControl<T> : ModificationControlBase
	{

		public abstract T ApplyChanges(T item);
	}
}
