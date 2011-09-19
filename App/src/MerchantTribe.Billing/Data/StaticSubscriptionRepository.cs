using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Billing.Data
{
    public class StaticSubscriptionRepository: ISubscriptionRepository
    {
        public List<Subscription> Subscriptions { get; set; }

        public StaticSubscriptionRepository()
        {
            Subscriptions = new List<Subscription>();
        }

        #region ISubscriptionRepository Members

        public Subscription Find(long subscriptionId)
        {
            Subscription s = null;
            foreach (Subscription t in Subscriptions)
            {
                if (t.Id == subscriptionId)
                {
                    s = t;
                    break;
                }
            }
            return s;
        }

        public bool Create(Subscription s)
        {
            s.Id = Subscriptions.Count + 1;
            Subscriptions.Add(s);
            return true;
        }

        public bool Cancel(long subscriptionId)
        {
            Subscription s = Find(subscriptionId);
            if (s == null) { return false; }

            s.IsCancelled = true;
            s.CancelDateUtc = DateTime.UtcNow;

            return Update(s);
        }

        public bool Update(Subscription s)
        {
            foreach (Subscription n in Subscriptions)
            {
                if (n.Id == s.Id)
                {
                    Subscriptions.Remove(n);
                    break;
                }
            }

            Subscriptions.Add(s);
            return true;
        }

        public List<Subscription> FindAll()
        {
            return Subscriptions;
        }

        public List<Subscription> FindAllCancelled()
        {
            return FindAll().Where(s => s.IsCancelled == true).ToList();
        }

        public List<Subscription> FindAllActive()
        {
            return FindAll().Where(s => s.IsCancelled == false).ToList();
        }

        public List<Subscription> FindForAccount(long accountId)
        {
            return FindAll().Where(s => s.AccountId == accountId).ToList();
        }
       
        #endregion
    }
}
