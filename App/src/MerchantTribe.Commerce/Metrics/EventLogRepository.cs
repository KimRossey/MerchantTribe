using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Metrics
{
    public class EventLogRepository: ConvertingRepositoryBase<Data.EF.bvc_EventLog, EventLogEntry>
    {
        private RequestContext context = null;
        
        public EventLogRepository(RequestContext c)
        {
            context = c;
            repository = new EntityFrameworkRepository<Data.EF.bvc_EventLog>(new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework));

            // Use supress logger for log repository to prevent
            // recursive calls on error
            this.logger = new MerchantTribe.Web.Logging.SupressLogger();
            repository.Logger = this.logger;
        }

        public EventLogRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_EventLog> r)
        {
            context = c;
            repository = r;

            // Use supress logger for log repository to prevent
            // recursive calls on error
            this.logger = new MerchantTribe.Web.Logging.SupressLogger();
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_EventLog data, EventLogEntry model)
        {
            model.Bvin = data.bvin;
            model.EventTimeUtc = data.EventTime;
            model.LastUpdatedUtc = data.LastUpdated;
            model.Message = data.Message;
            model.Severity = (EventLogSeverity)data.Severity;
            model.Source = data.Source;
            model.StoreId = data.StoreId;            
        }
        protected override void CopyModelToData(Data.EF.bvc_EventLog data, EventLogEntry model)
        {
            data.bvin = model.Bvin;
            data.EventTime = model.EventTimeUtc;
            data.LastUpdated = model.LastUpdatedUtc;
            data.Message = model.Message;
            data.Severity = (int)model.Severity;
            data.Source = model.Source;
            data.StoreId = model.StoreId;
        }


        public EventLogEntry Find(string bvin)
        {
            EventLogEntry result = FindForAllStores(bvin);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public EventLogEntry FindForAllStores(string bvin)
        {
            Data.EF.bvc_EventLog data = repository.FindByPrimaryKey(new PrimaryKey(bvin));
            if (data == null) return null;

            EventLogEntry result = new EventLogEntry();
            CopyDataToModel(data, result);
            return result;
        }

        public override bool Create(EventLogEntry item)
        {
            item.LastUpdatedUtc = DateTime.UtcNow;            
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.StoreId = context.CurrentStore.Id;            

 	        return base.Create(item);
        }       
        public bool Update(EventLogEntry c)
        {
            if (c.StoreId != context.CurrentStore.Id)
            {
                return false;
            }

            c.LastUpdatedUtc = DateTime.UtcNow;
            return this.Update(c, new PrimaryKey(c.Bvin));            
        }
        public bool Delete(string bvin)
        {
            long storeId = context.CurrentStore.Id;
            EventLogEntry img = Find(bvin);
            if (img == null) return false;
            return Delete(new PrimaryKey(bvin));            
        }

        public List<EventLogEntry> FindBySeverity(EventLogSeverity severity)
        {
            return FindBySeverityPaged(severity, 1, int.MaxValue);
        }
        public List<EventLogEntry> FindBySeverityPaged(EventLogSeverity severity, int pageNumber, int pageSize)
        {
            List<EventLogEntry> result = new List<EventLogEntry>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;
            long storeId = context.CurrentStore.Id;

            IQueryable<Data.EF.bvc_EventLog> items = repository.Find().Where(y => y.StoreId == storeId);

            if (severity != EventLogSeverity.Error)
            {
                items = items.Where(y => y.Severity == (int)severity);
            }

            items = items.OrderByDescending(y => y.EventTime)
                         .Skip(skip)
                         .Take(take);

            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }

        public List<EventLogEntry> FindBySeverityAllStores(EventLogSeverity severity)
        {
            return FindBySeverityPagedAllStores(severity, 1, int.MaxValue);
        }
        public List<EventLogEntry> FindBySeverityPagedAllStores(EventLogSeverity severity, int pageNumber, int pageSize)
        {
            List<EventLogEntry> result = new List<EventLogEntry>();

            if (pageNumber < 1) pageNumber = 1;

            int take = pageSize;
            int skip = (pageNumber - 1) * pageSize;            

            IQueryable<Data.EF.bvc_EventLog> items = repository.Find();

            if (severity != EventLogSeverity.Error)
            {
                items = items.Where(y => y.Severity == (int)severity);
            }

            items = items.OrderByDescending(y => y.EventTime)
                         .Skip(skip)
                         .Take(take);

            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }



        public void Roll()
        {
            IQueryable<Data.EF.bvc_EventLog> items = repository.Find().Where(y => y.StoreId == context.CurrentStore.Id);
            var count = items.Count();
            if (count > 199)
            {
                List<string> ids = new List<string>();
                int counter = 0;
                var sorted = items.OrderByDescending(y => y.EventTime).ToList();
                foreach (var item in sorted)
                {
                    counter++;
                    if (counter > 99)
                    {
                        ids.Add(item.bvin);
                    }
                }

                foreach (string deleteId in ids)
                {
                    Delete(new PrimaryKey(deleteId));
                }
            }            
        }

        public bool DeleteAllForStore(long storeId)
        {
            IQueryable<Data.EF.bvc_EventLog> items = repository.Find().Where(y => y.StoreId == storeId);


            List<string> toDeleteIds = new List<string>();

            if (items != null)
            {
                foreach (Data.EF.bvc_EventLog e in items)
                {
                    toDeleteIds.Add(e.bvin);                    
                }
            }

            foreach (string s in toDeleteIds)
            {
                this.Delete(new PrimaryKey(s));
            }

            return true;
        }
    }
}
