using System;

namespace MerchantTribe.Commerce.Catalog
{

	public enum CategorySortOrder : int
	{
		None = 0,
		ManualOrder = 1,
		ProductName = 2,
		ProductPriceAscending = 3,
		ProductPriceDescending = 4,
		ManufacturerName = 5
	}

}
