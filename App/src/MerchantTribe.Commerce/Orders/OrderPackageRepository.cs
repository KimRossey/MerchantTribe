using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;
using MerchantTribe.Shipping;

namespace MerchantTribe.Commerce.Orders
{
    public class OrderPackageRepository: ConvertingRepositoryBase<Data.EF.bvc_OrderPackage, OrderPackage>
    {
        public static OrderPackageRepository InstantiateForMemory(RequestContext c)
        {
            OrderPackageRepository result = null;
            ILogger logger = new MerchantTribe.Commerce.EventLog();
            result = new OrderPackageRepository(new MemoryStrategy<Data.EF.bvc_OrderPackage>(PrimaryKeyType.Long), logger);
            return result;
        }
        public static OrderPackageRepository InstantiateForDatabase(RequestContext c)
        {
            OrderPackageRepository result = null;
            ILogger logger = new MerchantTribe.Commerce.EventLog();
            result = new OrderPackageRepository(new EntityFrameworkRepository<Data.EF.bvc_OrderPackage>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)), logger);
            return result;
        }
        public OrderPackageRepository(IRepositoryStrategy<Data.EF.bvc_OrderPackage> strategy, ILogger log)
        {
            repository = strategy;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyModelToData(Data.EF.bvc_OrderPackage data, OrderPackage model)
        {
            data.CustomProperties = model.CustomProperties.ToXml();
            data.Description = model.Description;
            data.EstimatedShippingCost = model.EstimatedShippingCost;
            data.HasShipped = model.HasShipped ? 1 : 0;
            data.Height = model.Height;
            data.Id = model.Id;
            data.Items = model.ItemsToXml();
            data.LastUpdatedUtc = model.LastUpdatedUtc;
            data.Length = model.Length;
            data.OrderId = model.OrderId;
            data.ShipDateUtc = model.ShipDateUtc;
            data.ShippingProviderId = model.ShippingProviderId;
            data.ShippingProviderServiceCode = model.ShippingProviderServiceCode;
            data.SizeUnits = (int)model.SizeUnits;
            data.StoreId = model.StoreId;
            data.TrackingNumber = model.TrackingNumber;
            data.Weight = model.Weight;
            data.WeightUnits = (int)model.WeightUnits;
            data.Width = model.Width;            
        }
        protected override void CopyDataToModel(Data.EF.bvc_OrderPackage data, OrderPackage model)
        {
            model.CustomProperties = CustomPropertyCollection.FromXml(data.CustomProperties);
            model.Description = data.Description;
            model.EstimatedShippingCost = data.EstimatedShippingCost;
            model.HasShipped = data.HasShipped == 1;
            model.Height = data.Height;
            model.Id = data.Id;
            model.ItemsFromXml(data.Items);
            model.LastUpdatedUtc = data.LastUpdatedUtc;
            model.Length = data.Length;
            model.OrderId = data.OrderId;
            model.ShipDateUtc = data.ShipDateUtc;
            model.ShippingProviderId = data.ShippingProviderId;
            model.ShippingProviderServiceCode = data.ShippingProviderServiceCode;
            model.SizeUnits = (LengthType)data.SizeUnits;
            model.StoreId = data.StoreId;
            model.TrackingNumber = data.TrackingNumber;
            model.Weight = data.Weight;
            model.WeightUnits = (WeightType)data.WeightUnits;
            model.Width = data.Width;
        }

        public bool Update(OrderPackage item)
        {
            item.LastUpdatedUtc = DateTime.UtcNow;
            return base.Update(item, new PrimaryKey(item.Id));
        }
        public bool Delete(long id)
        {
            return Delete(new PrimaryKey(id));
        }

        public List<OrderPackage> FindForOrder(string orderBvin)
        {
            var items = repository.Find().Where(y => y.OrderId == orderBvin)
                                        .OrderBy(y => y.Id);
            return ListPoco(items);
        }
        public void DeleteForOrder(string orderBvin)
        {
            List<OrderPackage> existing = FindForOrder(orderBvin);
            foreach (OrderPackage sub in existing)
            {
                Delete(sub.Id);
            }
        }

        public void MergeList(string orderBvin,long storeId, List<OrderPackage> subitems)
        {
            // Set Base Key Field
            foreach (OrderPackage item in subitems)
            {
                item.OrderId = orderBvin;
                item.StoreId = storeId;
            }

            // Create or Update
            foreach (OrderPackage itemnew in subitems)
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
            List<OrderPackage> existing = FindForOrder(orderBvin);
            foreach (OrderPackage ex in existing)
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
		
    }
}
