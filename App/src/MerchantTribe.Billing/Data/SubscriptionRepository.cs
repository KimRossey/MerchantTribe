using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Billing.Data
{
    public class SubscriptionRepository: ISubscriptionRepository
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

        public SubscriptionRepository()
        {
            ConnectionString = string.Empty;
        }
        public SubscriptionRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        private static void CopyModelToData(bvb_Subscription data, Subscription model)
        {
            data.AccountId = model.AccountId;
            data.Amount = model.Amount;
            data.BillFor = (int)model.BillFor;
            data.CancelDateUtc = model.CancelDateUtc;
            data.Description = model.Description;
            data.Id = model.Id;
            data.IsCancelled = model.IsCancelled;
            data.Period = (int)model.Period;
            data.Sku = model.Sku;
            data.StartDateUtc = model.StartDateUtc;            
        }
        private static void CopyDataToModel(bvb_Subscription data, Subscription model)
        {
            model.AccountId = data.AccountId;
            model.Amount = data.Amount;
            model.BillFor = (Periods)data.BillFor;
            model.CancelDateUtc = data.CancelDateUtc;
            model.Description = data.Description;
            model.Id = data.Id;
            model.IsCancelled = data.IsCancelled;
            model.Period = (Periods)data.Period;
            model.Sku = data.Sku;
            model.StartDateUtc = data.StartDateUtc;
        }

        #region ISubscriptionRepository Members

        public bool Create(Subscription s)
        {
            bool result = false;

            if (s != null)
            {
                bvb_Subscription x = new bvb_Subscription();
                CopyModelToData(x, s);

                try
                {
                    if (context != null)
                    {
                        context.bvb_Subscriptions.InsertOnSubmit(x);
                        context.SubmitChanges();
                        result = true;
                        // Copy id back to model
                        CopyDataToModel(x, s);
                    }
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        public Subscription Find(long subscriptionId)
        {
            Subscription result = null;

            try
            {
                var x = (from n in context.bvb_Subscriptions
                         where n.Id == subscriptionId
                         select n).SingleOrDefault();

                if (x != null)
                {
                    result = new Subscription();
                    CopyDataToModel(x, result);
                }
            }
            catch
            {
                result = null;
            }

            return result;
        }
            
        public bool Update(Subscription s)
        {
            bool result = false;

            if (s != null)
            {
                try
                {
                    var x = (from n in context.bvb_Subscriptions
                             where n.Id == s.Id
                             select n).SingleOrDefault();

                    if (x != null)
                    {
                        CopyModelToData(x, s);
                        context.SubmitChanges();
                        result = true;
                    }

                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        public bool Cancel(long subscriptionId)
        {
            Subscription s = Find(subscriptionId);
            if (s != null)
            {
                if (s.Id == subscriptionId)
                {
                    s.IsCancelled = true;
                    s.CancelDateUtc = DateTime.UtcNow;
                    return Update(s);
                }
            }
            else
            {
                // This subscription doesn't exist, same as if cancelled.
                return true;
            }

            return false;
        }

        public List<Subscription> FindAll()
        {
            List<Subscription> result = new List<Subscription>();

            try
            {
                var x = context.bvb_Subscriptions.ToList();
                if (x != null)
                {
                    foreach (bvb_Subscription n in x)
                    {
                        Subscription m = new Subscription();
                        CopyDataToModel(n, m);
                        result.Add(m);
                    }
                }
            }
            catch
            {
                result = new List<Subscription>();
            }

            return result;
        }

        public List<Subscription> FindAllCancelled()
        {
            List<Subscription> result = new List<Subscription>();

            try
            {
                var x = context.bvb_Subscriptions.Where(s => s.IsCancelled == true).ToList();
                if (x != null)
                {
                    foreach (bvb_Subscription n in x)
                    {
                        Subscription m = new Subscription();
                        CopyDataToModel(n, m);
                        result.Add(m);
                    }
                }
            }
            catch
            {
                result = new List<Subscription>();
            }

            return result;
        }

        public List<Subscription> FindAllActive()
        {
            List<Subscription> result = new List<Subscription>();

            try
            {
                var x = context.bvb_Subscriptions.Where(s => s.IsCancelled == false).ToList();
                if (x != null)
                {
                    foreach (bvb_Subscription n in x)
                    {
                        Subscription m = new Subscription();
                        CopyDataToModel(n, m);
                        result.Add(m);
                    }
                }
            }
            catch
            {
                result = new List<Subscription>();
            }

            return result;
        }

        public List<Subscription> FindForAccount(long accountId)
        {
            List<Subscription> result = new List<Subscription>();

            try
            {
                var x = context.bvb_Subscriptions.Where(s => s.AccountId == accountId).ToList();
                if (x != null)
                {
                    foreach (bvb_Subscription n in x)
                    {
                        Subscription m = new Subscription();
                        CopyDataToModel(n, m);
                        result.Add(m);
                    }
                }
            }
            catch
            {
                result = new List<Subscription>();
            }

            return result;
        }

        #endregion
    }
}
