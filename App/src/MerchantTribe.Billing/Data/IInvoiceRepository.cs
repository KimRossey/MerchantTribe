using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Billing.Data
{
    public interface IInvoiceRepository
    {
        Invoice Find(long invoiceId);

        bool Create(Invoice s);

        Invoice CreateAndLoad(Invoice s);
               
        List<Invoice> FindAll();

        List<Invoice> FindAllForAccount(long accountId);
    }
}
