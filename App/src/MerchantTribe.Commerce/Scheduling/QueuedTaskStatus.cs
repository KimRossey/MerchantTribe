using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Scheduling
{
    public enum QueuedTaskStatus
    {        
        Unknown = 0,
        Pending = 1,
        Running = 2,
        Completed = 3,
        Failed = 4
    }
}
