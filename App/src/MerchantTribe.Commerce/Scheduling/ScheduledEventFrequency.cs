using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Scheduling
{
    public enum ScheduledEventFrequency
    {
        Every5Minutes = 5,
        Every30Minutes = 30,
        EveryHour = 60,
        Every6Hours = 360,
        Every12Hours = 720,
        OnceADay = 1440,
        OnceAWeek = 10080
    }
}
