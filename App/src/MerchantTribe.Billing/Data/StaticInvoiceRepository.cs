using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Billing.Data
{
    public class StaticInvoiceRepository: IInvoiceRepository
    {

        private List<Invoice> Invoices { get; set; }

        public StaticInvoiceRepository()
        {
            this.Invoices = new List<Invoice>();
        }

        #region IInvoiceRepository Members

        public Invoice Find(long invoiceId)
        {
            Invoice i = null;
            foreach (Invoice v in Invoices)
            {
                if (v.Id == invoiceId)
                {
                    i = v;
                    break;
                }
            }
            return i;
        }

        public bool Create(Invoice s)
        {
            long max = 0;
            if (Invoices.Count > 0)
            {
                var max2 = (from v in Invoices
                            select v.Id).Max();
                max = (long)max2;
                max += 1;
            }
            else
            {
                max = 1;
            }

            s.Id = max;
            Invoices.Add(s);
            return true;
        }

        public Invoice CreateAndLoad(Invoice s)
        {
            long max = 0;
            if (Invoices.Count > 0)
            {
                var max2 = (from v in Invoices
                            select v.Id).Max();
                max = (long)max2;
                max += 1;
            }
            else
            {
                max = 1;
            }

            s.Id = max;
            Invoices.Add(s);
            return Find(max);
        }

        public List<Invoice> FindAll()
        {
            return Invoices;
        }

        public List<Invoice> FindAllForAccount(long accountId)
        {
            List<Invoice> result = new List<Invoice>();

            var invoices = (from i in Invoices
                            where i.AccountId == accountId
                            select i).ToList();
            if (invoices != null)
            {
                result = (List<Invoice>)invoices;
            }
            return result;
        }

        #endregion
    }
}
