using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Contacts
{
    public class MailingListSnapShot
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public bool IsPrivate { get; set; }
        public long StoreId { get; set; }

        public MailingListSnapShot()
        {
            Id = 0;
            Name = string.Empty;
            LastUpdatedUtc = DateTime.UtcNow;
            IsPrivate = false;
            StoreId = 0;
        }
    }
}
