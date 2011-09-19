using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Billing.Data
{
    public class InvoiceRepository: IInvoiceRepository
    {

        private string _ConnectionString = string.Empty;
        public string ConnectionString
        {
            get { return _ConnectionString; }
            set
            {
                _ConnectionString = value;
                context = new DbDataContext(value);
            }
        }
        private DbDataContext context = new DbDataContext();

        public InvoiceRepository()
        {
            ConnectionString = string.Empty;
        }
        public InvoiceRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        //private static void CopyModelToData(bvb_Subscription data, Subscription model)
        //{
        //    data.AccountId = model.AccountId;
        //    data.Amount = model.Amount;
        //    data.BillFor = (int)model.BillFor;
        //    data.CancelDateUtc = model.CancelDateUtc;
        //    data.Description = model.Description;
        //    data.Id = model.Id;
        //    data.IsCancelled = model.IsCancelled;
        //    data.Period = (int)model.Period;
        //    data.Sku = model.Sku;
        //    data.StartDateUtc = model.StartDateUtc;            
        //}
        //private static void CopyDataToModel(bvb_Subscription data, Subscription model)
        //{
        //    model.AccountId = data.AccountId;
        //    model.Amount = data.Amount;
        //    model.BillFor = (Periods)data.BillFor;
        //    model.CancelDateUtc = data.CancelDateUtc;
        //    model.Description = data.Description;
        //    model.Id = data.Id;
        //    model.IsCancelled = data.IsCancelled;
        //    model.Period = (Periods)data.Period;
        //    model.Sku = data.Sku;
        //    model.StartDateUtc = data.StartDateUtc;
        //}

        #region IInvoiceRepository Members

        public Invoice Find(long invoiceId)
        {
            throw new NotImplementedException();
        }

        public bool Create(Invoice s)
        {
            throw new NotImplementedException();
        }

        public Invoice CreateAndLoad(Invoice s)
        {
            throw new NotImplementedException();
        }

        public List<Invoice> FindAll()
        {
            throw new NotImplementedException();
        }

        public List<Invoice> FindAllForAccount(long accountId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
