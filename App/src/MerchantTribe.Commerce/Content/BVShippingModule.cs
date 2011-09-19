using System;

namespace MerchantTribe.Commerce.Content
{

	public class BVShippingModule : BVModule
	{

		private Shipping.ShippingMethod _shippingMethod = null;

		public Shipping.ShippingMethod ShippingMethod {
			get { return _shippingMethod; }
			set { _shippingMethod = value; }
		}

	}
}
