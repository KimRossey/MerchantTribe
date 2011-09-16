using System;

namespace BVSoftware.Commerce.Shipping
{
	public abstract class DimensionCalculator
	{
		public abstract void GenerateDimensions(ShippingGroup group);

	}
}
