using System;

namespace MerchantTribe.Commerce.Shipping
{
	public abstract class DimensionCalculator
	{
		public abstract void GenerateDimensions(ShippingGroup group);

	}
}
