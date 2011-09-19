using System;
using System.Linq;
using System.Data.Objects;
using System.Transactions;

namespace MerchantTribe.Commerce.Orders
{
    public class OrderNumberGenerator
    {
        public static long GenerateNewOrderNumber(long storeId)
        {
            try
            {
                Data.EF.EntityFrameworkDevConnectionString ef = new Data.EF.EntityFrameworkDevConnectionString(WebAppSettings.ApplicationConnectionStringForEntityFramework);
                ObjectResult<long?> results = ef.GenerateNewOrderNumber(storeId);
                if (results != null)
                {
                    long? number = results.FirstOrDefault();
                    if (number != null)
                    {
                        if (number.HasValue)
                        {
                            return number.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
                return 0;
            }

            return 0;
        }

    }

}
