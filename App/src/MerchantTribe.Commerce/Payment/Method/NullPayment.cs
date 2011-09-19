using System;

namespace MerchantTribe.Commerce.Payment.Method
{

	public class NullPayment : DisplayPaymentMethod
	{

        public string Id() { return "6CB123D7-7FC5-43c1-A061-D6187E21AC89"; }
		public override string MethodId {
			get { return Id(); }
		}

		public override string MethodName {
			get { return "Null Payment"; }
		}

	}
}

