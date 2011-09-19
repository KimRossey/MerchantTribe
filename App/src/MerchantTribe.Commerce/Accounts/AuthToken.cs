using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts
{
    public class AuthToken
    {
        public long Id { get; set; }
        public Guid TokenId { get; set; }
        public long UserId { get; set; }
        public DateTime Expires { get; set; }

        public AuthToken()
        {
            Id = -1;
            TokenId = System.Guid.NewGuid();
            UserId = 0;
            Expires = DateTime.UtcNow;
        }
    }
}
