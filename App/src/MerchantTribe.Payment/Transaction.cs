using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Decimal Amount { get; set; }
        public ActionType Action { get; set; }
        public CardData Card { get; set; }        
        public CustomerData Customer {get;set;}
        public Dictionary<string, string> AdditionalSettings { get; set; }
        public string PreviousTransactionNumber {get;set;}
        public string PreviousTransactionAuthCode { get; set; }
        public string MerchantDescription {get;set;}
        public string MerchantInvoiceNumber { get; set; }
        public ResultData Result { get; set; }
        public string CheckNumber { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string CompanyAccountNumber { get; set; }
        public string GiftCardNumber { get; set; }
        public int RewardPoints { get; set; }

        // Is this a transaction that refunds money?
        public bool IsRefundTransaction
        {
            get {
                if (Action == ActionType.CreditCardRefund ||
                    Action == ActionType.CashReturned ||
                    Action == ActionType.CheckReturned ||
                    Action == ActionType.GiftCardIncrease ||
                    Action == ActionType.RewardPointsIncrease ||
                    Action == ActionType.PayPalRefund ||
                    Action == ActionType.RewardPointsUnHold)
                {
                    return true;
                }

                return false;
            }
        }

        public Transaction()
        {
            Id = new Guid();
            Amount = 0m;
            Action = ActionType.Uknown;            
            Card = new CardData();
            Customer = new CustomerData();
            PreviousTransactionNumber = string.Empty;
            PreviousTransactionAuthCode = string.Empty;
            MerchantDescription = string.Empty;
            MerchantInvoiceNumber = string.Empty;
            Result = new ResultData();
            CheckNumber = string.Empty;
            PurchaseOrderNumber = string.Empty;
            GiftCardNumber = string.Empty;
            CompanyAccountNumber = string.Empty;
            RewardPoints = 0;
        }
      
    }
}
