using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Geography;

namespace BVSoftware.Commerce.Taxes
{
    public interface ITaxSchedule
    {
        long TaxScheduleId();
        string TaxScheduleName();
    }
}
