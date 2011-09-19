using System;

namespace MerchantTribe.Commerce.Utilities
{

	public enum DateRangeType
	{
		None = 0,
		Today = 1,
		ThisWeek = 2,
		LastWeek = 3,
		Last31Days = 4,
		Last60Days = 5,
		Last120Days = 6,
		YearToDate = 7,
		LastYear = 8,
		AllDates = 9,
		ThisMonth = 10,
		LastMonth = 11,
        Yesterday = 12,
		Custom = 99
	}
}
