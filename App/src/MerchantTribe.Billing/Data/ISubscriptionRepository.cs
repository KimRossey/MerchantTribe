using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Billing.Data
{
    public interface ISubscriptionRepository
    {

        Subscription Find(long subscriptionId);

        bool Create(Subscription s);

        bool Update(Subscription s);

        bool Cancel(long subscriptionId);

        List<Subscription> FindAll();

        List<Subscription> FindAllCancelled();

        List<Subscription> FindAllActive();

        List<Subscription> FindForAccount(long accountId);
    }
}
