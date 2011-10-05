using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Orders
{
    public class OrderCouponRepository: ConvertingRepositoryBase<Data.EF.bvc_OrderCoupon, OrderCoupon>
    {
        public static OrderCouponRepository InstantiateForMemory(RequestContext c)
        {
            OrderCouponRepository result = null;
            ILogger logger = new MerchantTribe.Commerce.EventLog();
            result = new OrderCouponRepository(new MemoryStrategy<Data.EF.bvc_OrderCoupon>(PrimaryKeyType.Long), logger);
            return result;
        }
        public static OrderCouponRepository InstantiateForDatabase(RequestContext c)
        {
            OrderCouponRepository result = null;
            ILogger logger = new MerchantTribe.Commerce.EventLog();
            result = new OrderCouponRepository(new EntityFrameworkRepository<Data.EF.bvc_OrderCoupon>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)), logger);
            return result;
        }
        public OrderCouponRepository(IRepositoryStrategy<Data.EF.bvc_OrderCoupon> strategy, ILogger log)
        {
            repository = strategy;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyModelToData(Data.EF.bvc_OrderCoupon data, OrderCoupon model)
        {
            data.Id = model.Id;  
            data.CouponCode = model.CouponCode;
            data.IsUsed = model.IsUsed;
            data.LastUpdatedUtc = model.LastUpdatedUtc;
            data.OrderBvin = model.OrderBvin;
            data.StoreId = model.StoreId;
            data.UserId = model.UserId;
        }
        protected override void CopyDataToModel(Data.EF.bvc_OrderCoupon data, OrderCoupon model)
        {
            model.Id = data.Id;  
            model.CouponCode = data.CouponCode;
            model.IsUsed = data.IsUsed;
            model.LastUpdatedUtc = data.LastUpdatedUtc;
            model.OrderBvin = data.OrderBvin;
            model.StoreId = data.StoreId;
            model.UserId = data.UserId;      
            
        }

        public bool Update(OrderCoupon item)
        {
            item.LastUpdatedUtc = DateTime.UtcNow;
            return base.Update(item, new PrimaryKey(item.Id));
        }
        public bool Delete(long id)
        {
            return Delete(new PrimaryKey(id));
        }

        public List<OrderCoupon> FindForOrder(string orderBvin)
        {
            var items = repository.Find().Where(y => y.OrderBvin == orderBvin)
                                        .OrderBy(y => y.Id);
            return ListPoco(items);
        }
        public void DeleteForOrder(string orderBvin)
        {
            List<OrderCoupon> existing = FindForOrder(orderBvin);
            foreach (OrderCoupon sub in existing)
            {
                Delete(sub.Id);
            }
        }

        public void MergeList(string orderBvin,long storeId, List<OrderCoupon> subitems)
        {
            // Set Base Key Field
            foreach (OrderCoupon item in subitems)
            {
                item.OrderBvin = orderBvin;
                item.StoreId = storeId;
            }

            // Create or Update
            foreach (OrderCoupon itemnew in subitems)
            {
                if (itemnew.Id < 1)
                {
                    itemnew.LastUpdatedUtc = DateTime.UtcNow;
                    Create(itemnew);
                }
                else
                {
                    Update(itemnew);
                }
            }    
        
            // Delete missing
            List<OrderCoupon> existing = FindForOrder(orderBvin);
            foreach (OrderCoupon ex in existing)
            {
                var count = (from sub in subitems
                             where sub.Id == ex.Id
                             select sub).Count();
                if (count < 1)                
                {
                    Delete(ex.Id);
                }
            }
        }

		public int GetUseTimesByUserId(string couponCode, string UserId, long storeId)
		{
            var c = repository.Find().Where(y => y.StoreId == storeId)
                                    .Where(y => y.CouponCode == couponCode)                    
                                    .Where(y => y.UserId == UserId)
                                    .Count();
            return c;
		}

		public int GetUseTimesForStore(string couponCode, long storeId)
		{
            var c = repository.Find().Where(y => y.StoreId == storeId)
                                   .Where(y => y.CouponCode == couponCode)
                                   .Count();
            return c;
		}

    }
}
