using System;

namespace MerchantTribe.Commerce.Orders
{
	public enum OrderPaymentStatus : int
	{
		Unknown = 0,
		Unpaid = 1,
		PartiallyPaid = 2,
		Paid = 3,
		Overpaid = 4
	}

}
