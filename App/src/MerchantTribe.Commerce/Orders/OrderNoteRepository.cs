using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Orders
{
    public class OrderNoteRepository: ConvertingRepositoryBase<Data.EF.bvc_OrderNote, OrderNote>
    {
        public static OrderNoteRepository InstantiateForMemory(RequestContext c)
        {
            OrderNoteRepository result = null;
            ILogger logger = new MerchantTribe.Commerce.EventLog();
            result = new OrderNoteRepository(new MemoryStrategy<Data.EF.bvc_OrderNote>(PrimaryKeyType.Long), logger);
            return result;
        }
        public static OrderNoteRepository InstantiateForDatabase(RequestContext c)
        {
            OrderNoteRepository result = null;
            ILogger logger = new MerchantTribe.Commerce.EventLog();
            result = new OrderNoteRepository(new EntityFrameworkRepository<Data.EF.bvc_OrderNote>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)), logger);
            return result;
        }
        public OrderNoteRepository(IRepositoryStrategy<Data.EF.bvc_OrderNote> strategy, ILogger log)
        {
            repository = strategy;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyModelToData(Data.EF.bvc_OrderNote data, OrderNote model)
        {
            data.AuditDate = model.AuditDate;            
            data.Id = model.Id;
            data.IsPublic = model.IsPublic;
            data.LastUpdatedUtc = model.LastUpdatedUtc;
            data.Note = model.Note;
            data.OrderId = model.OrderID;
            data.StoreId = model.StoreId;            
        }
        protected override void CopyDataToModel(Data.EF.bvc_OrderNote data, OrderNote model)
        {
            model.AuditDate = data.AuditDate;
            model.Id = data.Id;
            model.IsPublic = data.IsPublic;
            model.LastUpdatedUtc = data.LastUpdatedUtc;
            model.Note = data.Note;
            model.OrderID = data.OrderId;
            model.StoreId = data.StoreId;            
        }

        public bool Update(OrderNote item)
        {
            item.LastUpdatedUtc = DateTime.UtcNow;
            return base.Update(item, new PrimaryKey(item.Id));
        }
        public bool Delete(long id)
        {
            return Delete(new PrimaryKey(id));
        }

        public List<OrderNote> FindForOrder(string orderBvin)
        {
            var items = repository.Find().Where(y => y.OrderId == orderBvin)
                                        .OrderBy(y => y.Id);
            return ListPoco(items);
        }
        public void DeleteForOrder(string orderBvin)
        {
            List<OrderNote> existing = FindForOrder(orderBvin);
            foreach (OrderNote sub in existing)
            {
                Delete(sub.Id);
            }
        }

        public void MergeList(string orderBvin,long storeId, List<OrderNote> subitems)
        {
            // Set Base Key Field
            foreach (OrderNote item in subitems)
            {
                item.OrderID = orderBvin;
                item.StoreId = storeId;
            }

            // Create or Update
            foreach (OrderNote itemnew in subitems)
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
            List<OrderNote> existing = FindForOrder(orderBvin);
            foreach (OrderNote ex in existing)
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
