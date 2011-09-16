using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.Payment;

namespace BVSoftware.Commerce.Payment
{
    public class RewardPointsSettings: BVSoftware.Payment.MethodSettings
    {
        public int PointsIssuedPerDollarSpent
        {
            get { return GetIntSetting("PointsIssuedPerDollarSpent"); }
            set { SetIntSetting("PointsIssuedPerDollarSpent", value); }
        }
        public int PointsNeededForDollarCredit
        {
            get { return GetIntSetting("PointsNeededForDollarCredit"); }
            set { SetIntSetting("PointsNeededForDollarCredit", value); }
        }
    }
}
