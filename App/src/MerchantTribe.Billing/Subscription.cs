using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Billing
{
    public class Subscription
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDateUtc { get; set; }
        public Periods Period { get; set; }
        public Periods BillFor { get; set; }
        public string Description { get; set; }
        public string Sku { get; set; }
        public long AccountId { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? CancelDateUtc {get;set;}

        public Subscription()
        {
            Id = 0;
            Amount = 0m;
            StartDateUtc = DateTime.UtcNow;
            Period = Periods.Unknown;
            BillFor = Periods.Unknown;
            Description = string.Empty;
            Sku = string.Empty;
            AccountId = 0;
            IsCancelled = false;
        }

        public int DaysUntilStart()
        {
            DateTime current = DateTime.UtcNow;
            return DaysUntilStart(current);
        }

        public int DaysUntilStart(DateTime currentTimeUtc)
        {

            if (currentTimeUtc >= StartDateUtc)
            {
                return 0;
            }
            else
            {
                TimeSpan ts = StartDateUtc - currentTimeUtc;
                if (ts.Hours > 0 || ts.Minutes > 0 || ts.Seconds > 0)
                {
                    return ts.Days + 1;
                }
                else
                {
                    return ts.Days;
                }
            }
        }

        public bool HasStarted()
        {
            if (DaysUntilStart() > 0)
            {
                return false;
            }
            return true;
        }

        public DateTime NextBillDateUtc()
        {
            return NextBillDateUtc(DateTime.UtcNow);
        }

        public DateTime NextBillDateUtc(DateTime currentTimeUtc)
        {
            DateTime current = currentTimeUtc;
            
            if (!HasStarted())
            {
                return StartDateUtc;
            }
            
            // Billing has started so there should be one invoice.
            
            // TODO: Get Last Invoice 

            return StartDateUtc;
        }
    }
}
