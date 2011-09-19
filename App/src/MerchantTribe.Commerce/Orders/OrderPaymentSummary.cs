using System;
using System.Data;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Orders
{

	public class OrderPaymentSummary
	{

		private decimal _AmountAuthorized = 0m;
		private decimal _AmountCharged = 0m;
		private decimal _AmountRefunded = 0m;
		private decimal _AmountDue = 0m;
		private string _PaymentsSummary = string.Empty;
		private decimal _TotalCredit = 0m;
		private decimal _GiftCardAmount = 0m;
		private bool _Collectable = false;

		public decimal AmountAuthorized {
			get { return _AmountAuthorized; }
		}
		public decimal AmountCharged {
			get { return _AmountCharged; }
		}
		public decimal AmountRefunded {
			get { return _AmountRefunded; }
		}
		public decimal AmountDue {
			get { return _AmountDue; }
		}
		public string PaymentsSummary {
			get { return _PaymentsSummary; }
		}
        //public bool Collectable {
        //    get { return _Collectable; }
        //}
		public decimal TotalCredit {
			get { return _TotalCredit; }
		}
		public decimal GiftCardAmount {
			get { return _GiftCardAmount; }
			set { _GiftCardAmount = value; }
		}

		public void Clear()
		{
			_AmountAuthorized = 0m;
			_AmountCharged = 0m;
			_AmountRefunded = 0m;
			_AmountDue = 0m;
			_PaymentsSummary = string.Empty;
			_TotalCredit = 0m;
			_Collectable = false;
		}

		public void Populate(Orders.Order o, OrderService svc)
		{
			Clear();

            foreach (Orders.OrderTransaction t in svc.Transactions.FindForOrder(o.bvin))
            {
                
                _TotalCredit += t.AmountAppliedToOrder;
                switch (t.Action)
                {
                    case MerchantTribe.Payment.ActionType.CashReceived:
                    case MerchantTribe.Payment.ActionType.CheckReceived:
                    case MerchantTribe.Payment.ActionType.CreditCardCapture:
                    case MerchantTribe.Payment.ActionType.CreditCardCharge:
                    case MerchantTribe.Payment.ActionType.GiftCardCapture:
                    case MerchantTribe.Payment.ActionType.GiftCardDecrease:
                    case MerchantTribe.Payment.ActionType.PayPalCapture:
                    case MerchantTribe.Payment.ActionType.PayPalCharge:
                    case MerchantTribe.Payment.ActionType.PurchaseOrderAccepted:
                    case MerchantTribe.Payment.ActionType.CompanyAccountAccepted:
                    case MerchantTribe.Payment.ActionType.RewardPointsCapture:
                    case MerchantTribe.Payment.ActionType.RewardPointsDecrease:
                        _AmountCharged += t.AmountAppliedToOrder;
                        break;
                    case MerchantTribe.Payment.ActionType.CreditCardHold:
                    case MerchantTribe.Payment.ActionType.GiftCardHold:
                    case MerchantTribe.Payment.ActionType.PayPalHold:
                    case MerchantTribe.Payment.ActionType.RewardPointsHold:
                        _AmountAuthorized += t.AmountHeldForOrder;
                        break;
                    case MerchantTribe.Payment.ActionType.CashReturned:
                    case MerchantTribe.Payment.ActionType.CheckReturned:
                    case MerchantTribe.Payment.ActionType.CreditCardRefund:
                    case MerchantTribe.Payment.ActionType.GiftCardIncrease:
                    case MerchantTribe.Payment.ActionType.PayPalRefund:
                    case MerchantTribe.Payment.ActionType.RewardPointsIncrease:
                        _AmountRefunded += -1 * t.AmountAppliedToOrder;
                        break;
                }
            }

            _PaymentsSummary = svc.OrdersListPaymentMethods(o);                        
			_AmountDue = o.TotalGrand - _TotalCredit;
		}

	}
}

