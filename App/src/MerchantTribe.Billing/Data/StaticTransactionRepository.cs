using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Billing.Data
{
    public class StaticTransactionRepository: ITransactionRepository
    {

        private List<Transaction> Transactions { get; set; }

        public StaticTransactionRepository()
        {
            this.Transactions = new List<Transaction>();
        }

        #region ITransactionRepository Members

        public Transaction Find(long id)
        {
            Transaction t = null;
            foreach (Transaction v in Transactions)
            {
                if (v.Id == id)
                {
                    t = v;
                    break;
                }
            }
            return t;
        }

        public bool Create(Transaction t)
        {
            long max = 0;
            if (Transactions.Count > 0)
            {
                var max2 = (from v in Transactions
                            select v.Id).Max();
                max = (long)max2;
                max += 1;
            }
            else
            {
                max = 1;
            }

            t.Id = max;
            t.TimeStampUtc = DateTime.UtcNow;
            t.CreditCard.SecurityCode = string.Empty;
            Transactions.Add(t);
            return true;
        }

        public Transaction CreateAndLoad(Transaction t)
        {
            long max = 0;
            if (Transactions.Count > 0)
            {
                var max2 = (from v in Transactions
                            select v.Id).Max();
                max = (long)max2;
                max += 1;
            }
            else
            {
                max = 1;
            }

            t.Id = max;
            t.TimeStampUtc = DateTime.UtcNow;
            t.CreditCard.SecurityCode = string.Empty;
            Transactions.Add(t);
            return Find(max);
        }

        public List<Transaction> FindAll(int pageSize, int pageNumber, ref int totalCount)
        {
            if (Transactions == null)
                return new List<Transaction>();

            totalCount = Transactions.Count();
            return Transactions.Skip(pageSize * pageNumber).Take(pageSize).ToList();
        }

        public List<Transaction> FindAllForAccount(long accountId, int pageSize, int pageNumber, ref int totalCount)
        {
            var trans = (from t in Transactions
                         where t.AccountId == accountId
                         select t);

            if (trans == null)
                return new List<Transaction>();
            
            totalCount = trans.Count();
            return trans.Skip(pageSize * pageNumber).Take(pageSize).ToList();
        }

        #endregion
    }
}
