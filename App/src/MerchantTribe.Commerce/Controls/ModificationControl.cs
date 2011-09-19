using System;

namespace MerchantTribe.Commerce.Controls
{
	public abstract class ModificationControl<T> : ModificationControlBase
	{

		public abstract T ApplyChanges(T item);
	}
}
