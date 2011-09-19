using System;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Payment.Method
{


	public class CashOnDelivery : DisplayPaymentMethod
	{
        public static string Id() { return "EE171EFD-9E4A-4eda-AD70-4CB99F28E06C"; }

		public override string MethodId {
			get { return Id(); }
		}

		public override string MethodName {
			get { return "Cash on Delivery"; }
		}

	}
}

