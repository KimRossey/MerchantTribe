using System;

namespace MerchantTribe.Commerce.Payment
{

    // Holds a payment method as displayed to customers
    // during the checkout process    
	public abstract class DisplayPaymentMethod
	{

		public abstract string MethodName {
			get;
		}
		public abstract string MethodId {
			get;
		}

	}
}

