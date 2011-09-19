using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Billing
{
    public enum Periods
    {
        Unknown = 0,
        OneTime = 1,
        OneWeek = 2,
        TwoWeeks = 3,
        OneMonth = 4,
        ThirtyDays = 5,
        ThreeMonths = 6,
        SixMonths = 7,
        OneYear = 8,
        TwoYears = 9,
        FiveYears = 10,
        Forever = 99        
    }
}
