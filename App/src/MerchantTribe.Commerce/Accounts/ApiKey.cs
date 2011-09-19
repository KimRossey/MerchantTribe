using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts
{
    public class ApiKey
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public string Key { get; set; }

        public ApiKey()
        {
            Id = -1;
            StoreId = -1;
            Key = string.Empty;
        }
    }
}
