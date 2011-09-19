using System;

namespace MerchantTribe.Commerce.Orders
{
	public enum OrderShippingStatus : int
	{
		Unknown = 0,
		Unshipped = 1,
		PartiallyShipped = 2,
		FullyShipped = 3,
		NonShipping = 4
	}
}
