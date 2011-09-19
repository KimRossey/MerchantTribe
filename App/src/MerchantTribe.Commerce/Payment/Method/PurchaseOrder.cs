using System;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Payment.Method
{


	public class PurchaseOrder : DisplayPaymentMethod
	{
        public static string Id() { return "26C948F3-22EF-4bcb-9AE9-DEB9839BF4A7"; }
	    public override string MethodId {
			get { return Id(); }
		}

		public override string MethodName {
			get { return "Purchase Order"; }
		}
	}
}

