using System;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Payment.Method
{

	public class Telephone : DisplayPaymentMethod
	{
        public static string Id() { return "9FD35C50-CDCB-42ac-9549-14119BECBD0C"; }
		public override string MethodId {
			get { return Id(); }
		}

		public override string MethodName {
			get { return "Telephone"; }
		}

	}

}
