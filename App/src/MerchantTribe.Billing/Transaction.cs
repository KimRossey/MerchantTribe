using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Payment;

namespace MerchantTribe.Billing
{
    public class Transaction
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public DateTime TimeStampUtc { get; set; }
        public ActionType Action { get; set; }
        public decimal Amount { get; set; }
        public CardData CreditCard { get; set; }
        public bool Success { get; set; }
        public bool Voided { get; set; }
        public string RefNum1 { get; set; }
        public string RefNum2 { get; set; }
        public string InvoiceReference { get; set; }
        public string Messages { get; set; }
        public string ExternalOrderNumber { get; set; }
        public long LinkedToTransaction { get; set; }

        public Transaction()
        {
            Id = 0;
            AccountId = 0;            
            TimeStampUtc = DateTime.UtcNow;
            Action = ActionType.Uknown;
            Amount = 0m;
            CreditCard = new CardData();
            Success = false;
            Voided = false;
            RefNum1 = string.Empty;
            RefNum2 = string.Empty;
            InvoiceReference = string.Empty;
            Messages = string.Empty;
            ExternalOrderNumber = string.Empty;
            LinkedToTransaction = 0;
        }

        public Transaction(MerchantTribe.Payment.Transaction t)
        {
            Id = 0;
            AccountId = 0;
            InvoiceReference = string.Empty;
            ExternalOrderNumber = string.Empty;
            LinkedToTransaction = 0;
            PopulateFromPaymentTransaction(t);
        }

        public void PopulateFromPaymentTransaction(MerchantTribe.Payment.Transaction t)
        {
            if (t != null)
            {
                TimeStampUtc = DateTime.UtcNow;
                Action = t.Action;
                Amount = t.Amount;
                if (t.Action == ActionType.CreditCardRefund)
                {
                    Amount = (t.Amount * -1);
                }
                CreditCard = t.Card;
                Success = t.Result.Succeeded;
                Voided = false;
                RefNum1 = t.Result.ReferenceNumber;
                RefNum2 = t.Result.ReferenceNumber2;
                Messages = string.Empty;
                if (t.Result.Messages.Count > 0)
                {
                    foreach (Message m in t.Result.Messages)
                    {
                        Messages += m.Code + "::" + m.Description;
                    }
                }
            }
        }

        // Only allow voids for about 16 hours since most credit card
        // companies don't allow voids after the batch is processed for the day
        // Merchants should use a Refund instead of void at that point
        public bool IsVoidable { 
            get {
                long cutOffTicks= DateTime.UtcNow.AddHours(-16).Ticks;
                long timestampTicks = TimeStampUtc.Ticks;
                if (timestampTicks >= cutOffTicks)
                {
                    return true;
                }
                return false;             
            } 
        }

        public decimal AmountApplied
        {
            get
            {
                decimal result = 0;

                if (this.Success)
                {
                    if (!this.Voided)
                    {
                        if (this.Action == ActionType.CreditCardCapture || 
                            this.Action == ActionType.CreditCardCharge ||
                            this.Action == ActionType.CreditCardRefund)
                        {
                            result += this.Amount;
                        }
                    }
                }

                return result;
            }
        }
    }
}
