using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace MerchantTribe.Commerce.Reporting
{
    public class OrderSummary
    {
        public int NewOrderCount {get;private set;}
        public int OnHoldOrderCount { get; private set; }
        public int ReadyForPaymentOrderCount { get; private set; }
        public int ReadyForShippingOrderCount { get; private set; }

        private class OrderSummaryData
        {
            public string StatusCode { get; set; }
            public int OrderCount { get; set; }

            public OrderSummaryData()
            {
                StatusCode = string.Empty;
                OrderCount = 0;
            }
        }

        public OrderSummary(long storeId)
        {
            NewOrderCount = 0;
            OnHoldOrderCount = 0;
            ReadyForPaymentOrderCount = 0;
            ReadyForShippingOrderCount = 0;

            try
            {
                Data.EF.EntityFrameworkDevConnectionString ef = new Data.EF.EntityFrameworkDevConnectionString(WebAppSettings.ApplicationConnectionStringForEntityFramework);

                var result = ef.bvc_Order.Where(y => y.IsPlaced == 1)
                                         .Where(y => y.StoreId == storeId)
                                         .GroupBy(y => y.StatusCode)
                                         .Select(grouping => grouping.Select(y => new OrderSummaryData()
                                                                                  {
                                                                                    StatusCode = y.StatusCode,
                                                                                    OrderCount = grouping.Count()
                                                                                   })
                                                                                .FirstOrDefault())
                                            .OrderBy(y => y.StatusCode);
                if (result != null)
                {
                    foreach (OrderSummaryData stat in result)
                    {
                        if (stat.StatusCode == Orders.OrderStatusCode.Received)
                        {
                            this.NewOrderCount = stat.OrderCount;
                        }
                        if (stat.StatusCode == Orders.OrderStatusCode.OnHold)
                        {
                            this.OnHoldOrderCount = stat.OrderCount;
                        }
                        if (stat.StatusCode == Orders.OrderStatusCode.ReadyForPayment)
                        {
                            this.ReadyForPaymentOrderCount = stat.OrderCount;
                        }
                        if (stat.StatusCode == Orders.OrderStatusCode.ReadyForShipping)
                        {
                            this.ReadyForShippingOrderCount = stat.OrderCount;
                        }              
                    }
                }               
             
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);                
            }
           
        }

    }
    
}
