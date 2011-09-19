using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Marketing
{
    /// <summary>
    /// This helps us determine which promotion qualifications
    /// should be evaluated first. Lower cost qualifications
    /// will be checked first so that if they fail, we never
    /// run the expensive operations that might need to check
    /// the database.
    /// </summary>
    public enum RelativeProcessingCost
    {
        Lowest = 0,
        Lower = 250,
        Normal = 500,
        Higher = 750,
        Highest = 1000
    }
}
