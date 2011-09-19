using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Billing.Data
{
    public class StaticBillingAccountRepository: IBillingAccountRepository
    {
        public List<BillingAccount> Accounts { get; set; }        

        public StaticBillingAccountRepository()
        {
            Accounts = new List<BillingAccount>();
        }

        public BillingAccount FindById(long id)
        {
            BillingAccount result = null;

            foreach (BillingAccount b in Accounts)
            {
                if (b.Id == id)
                {
                    return b;
                }
            }

            return result;
        }

        public BillingAccount FindOrCreate(string email)
        {
            
            foreach (BillingAccount b in Accounts)
            {
                if (b.Email == email.Trim().ToLowerInvariant())
                {
                    return b;
                }
            }
            BillingAccount a = new BillingAccount();
            a.Email = email.Trim().ToLowerInvariant();
            a.Id = Accounts.Count + 1;
            Accounts.Add(a);
            return a;
        }

        public BillingAccount FindByEmail(string email)
        {
            BillingAccount result = null;

            var a = (from act in Accounts
                     where act.Email == email
                     select act).SingleOrDefault();

            if (a != null)
            {
                result = (BillingAccount)a;
            }

            return result;
        }

        public BillingAccount FindOrCreate(BillingAccount a)
        {

            foreach (BillingAccount b in Accounts)
            {
                if (b.Email == a.Email)
                {
                    return b;
                }
            }                        
            a.Id = Accounts.Count + 1;
            Accounts.Add(a);
            return a;
        }

        public bool Update(BillingAccount a)
        {            
            foreach (BillingAccount ba in Accounts)
            {
                if (ba.Id == a.Id)
                {
                    ba.Email = a.Email;
                    ba.CreditCard = a.CreditCard;
                }
            }

            return true;
        }

        public List<BillingAccount> FindAll()
        {
            return Accounts;
        }

    }
}
