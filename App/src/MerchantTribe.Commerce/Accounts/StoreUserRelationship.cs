using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts
{
    public class StoreUserRelationship
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long StoreId { get; set; }
        public StoreAccessMode AccessMode {get;set;}

        public StoreUserRelationship()
        {
            Id = -1;
            UserId = 0;
            StoreId = 0;
            AccessMode = StoreAccessMode.Manager;
        }
    }
}
