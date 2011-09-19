using System;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Payment.Method
{

	public class Cash : DisplayPaymentMethod
	{
        public static string Id() {return "7FCC4B3F-6E67-4f58-86B0-25BCCC035A0E";}

    	public override string MethodId {
			get { return Id(); }
		}

		public override string MethodName {
			get { return "Cash"; }
		}

	}
}

