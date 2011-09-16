using System;

namespace BVSoftware.Commerce.Orders
{
	public enum RMAStatus
	{
		None = -1,
		Unsubmitted = 0,
		Pending = 1,
		Open = 2,
		Closed = 3,
		Rejected = 4
	}
}
