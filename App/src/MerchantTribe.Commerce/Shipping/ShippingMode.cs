using System;

namespace MerchantTribe.Commerce.Shipping
{
	public enum ShippingMode : int
	{
		None = 0,
		ShipFromSite = 1,
		ShipFromVendor = 2,
		ShipFromManufacturer = 3
	}
}

