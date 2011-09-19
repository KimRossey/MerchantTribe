using System;
using System.Xml.Serialization;

namespace MerchantTribe.Commerce.Contacts
{

	public enum AffiliateConflictMode : int
	{
		None = 0,
		FavorOldAffiliate = 1,
		FavorNewAffiliate = 2
	}

}

