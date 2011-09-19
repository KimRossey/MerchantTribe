using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Billing.Data
{
    public interface IBillingAccountRepository
    {

        BillingAccount FindById(long id);

        BillingAccount FindOrCreate(string email);

        BillingAccount FindByEmail(string email);

        BillingAccount FindOrCreate(BillingAccount a);

        bool Update(BillingAccount a);

        List<BillingAccount> FindAll();
    }
}
