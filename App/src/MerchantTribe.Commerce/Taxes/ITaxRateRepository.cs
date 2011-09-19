using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Taxes
{
    public interface ITaxRateRepository
    {
        List<ITaxRate> GetRates(long storeId, long scheduleId);
    }
}
