using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Billing.Data
{
    public interface ITransactionRepository
    {

        Transaction Find(long id);

        bool Create(Transaction t);

        Transaction CreateAndLoad(Transaction t);

        List<Transaction> FindAll(int pageSize, int pageNumber, ref int totalCount);

        List<Transaction> FindAllForAccount(long accountId, int pageSize, int pageNumber, ref int totalCount);


    }
}
