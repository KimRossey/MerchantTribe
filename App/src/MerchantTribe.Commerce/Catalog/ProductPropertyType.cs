using System;

namespace MerchantTribe.Commerce.Catalog
{

	public enum ProductPropertyType : int
	{
		None = 0,
		TextField = 1,
		MultipleChoiceField = 2,
		CurrencyField = 3,
		DateField = 4,
		HyperLink = 7
	}

}

