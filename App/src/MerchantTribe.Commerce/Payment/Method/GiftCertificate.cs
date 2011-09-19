using System;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Payment.Method
{


	public class GiftCertificate : DisplayPaymentMethod
	{

        //public override string FriendlyStatus(Orders.OrderPayment op)
        //{
        //    string result = "Waiting for Gift Certificate";
        //    if (op.AmountAuthorized > 0) {
        //        if (op.AmountCharged > 0) {
        //            if (op.AmountRefunded > 0) {
        //                result = "Giftcard " + op.GiftCertificateNumber + " refunded";
        //            }
        //            else {
        //                result = "Giftcard " + op.GiftCertificateNumber + " debited for " + op.AmountCharged.ToString("c");
        //            }
        //        }
        //        else {
        //            result = "Giftcard " + op.GiftCertificateNumber + " checked for " + op.AmountAuthorized.ToString("c") + ", error during debit.";
        //        }
        //    }
        //    else {
        //        result = "Waiting for Gift Certificate";
        //    }

        //    return result;
        //}

        public static string Id() { return "91a205f1-8c1c-4267-bed0-c8e410e7e680"; }
		public override string MethodId {
			get { return Id(); }
		}

		public override string MethodName {
			get { return "Gift Certificate"; }
		}

	}
}

