using System;

namespace MerchantTribe.Commerce.Catalog
{

	public enum CategorySourceType : int
	{
		Manual = 0,
		ByRules = 1,
		CustomLink = 2,
		CustomPage = 3,
        DrillDown = 4,
        FlexPage = 5
	}

}
