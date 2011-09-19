using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Billing
{
    public class Service
    {        

        public Data.IBillingAccountRepository Accounts { get; set; }
        public Data.ISubscriptionRepository Subscriptions { get; set; }
        public Data.IInvoiceRepository Invoices { get; set; }
        public Data.ITransactionRepository Transactions { get; set; }

      
        public Service(string connectionString)
        {
            Accounts = new Data.BillingAccountRepository(connectionString);
            Subscriptions = new Data.SubscriptionRepository(connectionString);
            Invoices = new Data.InvoiceRepository(connectionString);
            Transactions = new Data.TransactionRepository(connectionString);
        }

        public Service(Data.IBillingAccountRepository accountRepository,
                        Data.ISubscriptionRepository subscriptionRepository,
                        Data.IInvoiceRepository invoiceRepository,
                        Data.ITransactionRepository transactionRepository)
        {
            Accounts = accountRepository;
            Subscriptions = subscriptionRepository;
            Invoices = invoiceRepository;
            Transactions = transactionRepository;
        }
         
        public static DateTime AddPeriodToDate(DateTime utcDate, Periods period)
        {
            switch(period)
            {
                case Periods.FiveYears:
                    return utcDate.AddYears(5);
                case Periods.Forever:
                    return DateTime.MaxValue;
                case Periods.OneMonth:
                    return utcDate.AddMonths(1);
                case Periods.OneTime:
                    return utcDate;
                case Periods.OneWeek:
                    return utcDate.AddDays(7);                
                case Periods.OneYear:
                    return utcDate.AddYears(1);
                case Periods.SixMonths:
                    return utcDate.AddMonths(6);                    
                case Periods.ThirtyDays:
                    return utcDate.AddDays(30);                    
                case Periods.ThreeMonths:
                    return utcDate.AddMonths(3);                    
                case Periods.TwoWeeks:
                    return utcDate.AddDays(14);                    
                case Periods.TwoYears:
                    return utcDate.AddYears(2);                    
                case Periods.Unknown:
                    return utcDate;                    
            }

            return utcDate;
        }

        public static DateTime SubtractPeriodFromDate(DateTime utcDate, Periods period)
        {
            switch (period)
            {
                case Periods.FiveYears:
                    return utcDate.AddYears(-5);                    
                case Periods.Forever:
                    return DateTime.MinValue;                    
                case Periods.OneMonth:
                    return utcDate.AddMonths(-1);                    
                case Periods.OneTime:
                    return utcDate;                    
                case Periods.OneWeek:
                    return utcDate.AddDays(-7);                    
                case Periods.OneYear:
                    return utcDate.AddYears(-1);                    
                case Periods.SixMonths:
                    return utcDate.AddMonths(-6);                    
                case Periods.ThirtyDays:
                    return utcDate.AddDays(-30);                    
                case Periods.ThreeMonths:
                    return utcDate.AddMonths(-3);                    
                case Periods.TwoWeeks:
                    return utcDate.AddDays(-14);                    
                case Periods.TwoYears:
                    return utcDate.AddYears(-2);                    
                case Periods.Unknown:
                    return utcDate;                    
            }

            return utcDate;
        }

        public Transaction ChargeCardForInvoice(Invoice inv, MerchantTribe.Payment.Method method)
        {
            Transaction t = new Transaction();
            t.Action = MerchantTribe.Payment.ActionType.CreditCardCharge;

            if (inv != null)
            {


                BillingAccount account = Accounts.FindById(inv.AccountId);
                if (account != null)
                {
                    if (!account.HasValidCreditCard(DateTime.Now))
                    {
                        t.Messages += "Billing Account does not have a valid credit card on file.";
                    }
                    else
                    {

                        t.AccountId = inv.AccountId;
                        t.InvoiceReference = inv.Id.ToString();

                        MerchantTribe.Payment.Transaction payTrans = new MerchantTribe.Payment.Transaction();
                        payTrans.Action = MerchantTribe.Payment.ActionType.CreditCardCharge;
                        payTrans.Amount = inv.TotalGrand();
                        payTrans.Card = account.CreditCard;
                        payTrans.Customer.PostalCode = account.BillingZipCode;
                        payTrans.MerchantDescription = "BV Software";
                        payTrans.MerchantInvoiceNumber = inv.Id.ToString();

                        method.ProcessTransaction(payTrans);

                        t.PopulateFromPaymentTransaction(payTrans);
                    }
                }
                else
                {
                    t.Messages += "Billing Account could not be located";
                }

            }
            else
            {
                t.Messages += "Invoice was null.";
            }

            t = Transactions.CreateAndLoad(t);

            return t;
        }
    }
}
