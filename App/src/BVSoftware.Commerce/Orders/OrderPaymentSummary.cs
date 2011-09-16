using System;
using System.Data;
using System.Collections.ObjectModel;

namespace BVSoftware.Commerce.Orders
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
                    case BVSoftware.Payment.ActionType.CashReceived:
                    case BVSoftware.Payment.ActionType.CheckReceived:
                    case BVSoftware.Payment.ActionType.CreditCardCapture:
                    case BVSoftware.Payment.ActionType.CreditCardCharge:
                    case BVSoftware.Payment.ActionType.GiftCardCapture:
                    case BVSoftware.Payment.ActionType.GiftCardDecrease:
                    case BVSoftware.Payment.ActionType.PayPalCapture:
                    case BVSoftware.Payment.ActionType.PayPalCharge:
                    case BVSoftware.Payment.ActionType.PurchaseOrderAccepted:
                    case BVSoftware.Payment.ActionType.CompanyAccountAccepted:
                    case BVSoftware.Payment.ActionType.RewardPointsCapture:
                    case BVSoftware.Payment.ActionType.RewardPointsDecrease:
                        _AmountCharged += t.AmountAppliedToOrder;
                        break;
                    case BVSoftware.Payment.ActionType.CreditCardHold:
                    case BVSoftware.Payment.ActionType.GiftCardHold:
                    case BVSoftware.Payment.ActionType.PayPalHold:
                    case BVSoftware.Payment.ActionType.RewardPointsHold:
                        _AmountAuthorized += t.AmountHeldForOrder;
                        break;
                    case BVSoftware.Payment.ActionType.CashReturned:
                    case BVSoftware.Payment.ActionType.CheckReturned:
                    case BVSoftware.Payment.ActionType.CreditCardRefund:
                    case BVSoftware.Payment.ActionType.GiftCardIncrease:
                    case BVSoftware.Payment.ActionType.PayPalRefund:
                    case BVSoftware.Payment.ActionType.RewardPointsIncrease:
                        _AmountRefunded += -1 * t.AmountAppliedToOrder;
                        break;
                }
            }

            _PaymentsSummary = svc.OrdersListPaymentMethods(o);                        
			_AmountDue = o.TotalGrand - _TotalCredit;
		}

	}
}

