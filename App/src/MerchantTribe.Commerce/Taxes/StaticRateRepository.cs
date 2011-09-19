using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Taxes
{
    public class StaticRateRepository : ITaxRateRepository
    {
        public List<ITaxRate> Rates { get; set; }

        public StaticRateRepository()
        {
            Rates = new List<ITaxRate>();
        }
        #region ITaxRateRepository Members

        public List<ITaxRate> GetRates(long storeId, long scheduleId)
        {
            return Rates;
        }

        #endregion
    }
}
