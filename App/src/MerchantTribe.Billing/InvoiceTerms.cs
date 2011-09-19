using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Billing
{
    public enum InvoiceTerms
    {
        DueOnReceipt = 0,        
        Net15 = 15,
        Net20 = 20,
        Net30 = 30,
        Net60 = 60,
        Net90 = 90
    }
}
