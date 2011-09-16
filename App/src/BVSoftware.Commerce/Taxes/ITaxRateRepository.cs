using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Commerce.Taxes
{
    public interface ITaxRateRepository
    {
        List<ITaxRate> GetRates(long storeId, long scheduleId);
    }
}
