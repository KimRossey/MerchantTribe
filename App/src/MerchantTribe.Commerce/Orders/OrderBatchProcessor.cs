using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Orders
{
    public class OrderBatchProcessor
    {
        public static void AcceptAllNewOrders(OrderService svc)
        {
            OrderSearchCriteria criteria = new OrderSearchCriteria();
            criteria.IsPlaced = true;
            criteria.StatusCode = OrderStatusCode.Received;
            int pageSize = 1000;            
            int totalCount = 0;

            List<OrderSnapshot> orders =  svc.Orders.FindByCriteriaPaged(criteria, 1, pageSize, ref totalCount);
            if (orders != null)
            {
                foreach (OrderSnapshot o in orders)
                {
                    Order ord = svc.Orders.FindForCurrentStore(o.bvin);
                    if (ord != null)
                    {
                        ord.StatusCode = OrderStatusCode.ReadyForPayment;
                        ord.StatusName = "Ready for Payment";
                        svc.Orders.Update(ord);
                    }
                }
            }
        }

        public static void CollectPaymentAndShipPendingOrders(MerchantTribeApplication app)
        {
            OrderSearchCriteria criteria = new OrderSearchCriteria();
            criteria.IsPlaced = true;
            criteria.StatusCode = OrderStatusCode.ReadyForPayment;
            int pageSize = 1000;            
            int totalCount = 0;

            List<OrderSnapshot> orders = app.OrderServices.Orders.FindByCriteriaPaged(criteria, 1, pageSize, ref totalCount);
            if (orders != null)
            {
                foreach (OrderSnapshot os in orders)
                {
                    Order o = app.OrderServices.Orders.FindForCurrentStore(os.bvin);
                    OrderPaymentManager payManager = new OrderPaymentManager(o, app);
                    payManager.CreditCardCompleteAllCreditCards();
                    payManager.PayPalExpressCompleteAllPayments();
                    if (o.PaymentStatus == OrderPaymentStatus.Paid ||
                        o.PaymentStatus == OrderPaymentStatus.Overpaid)
                    {
                        if (o.ShippingStatus == OrderShippingStatus.FullyShipped)
                        {
                            o.StatusCode = OrderStatusCode.Completed;
                            o.StatusName = "Completed";
                        }
                        else
                        {
                            o.StatusCode = OrderStatusCode.ReadyForShipping;
                            o.StatusName = "Ready for Shipping";
                        }
                        app.OrderServices.Orders.Update(o);
                    }
                }
            }
        }
      
    }
}
