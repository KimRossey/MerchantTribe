using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts
{
    public class StoreDomain
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        private string _DomainName = string.Empty;
        public string DomainName 
        {
            get { return _DomainName; }
            set {
                string temp = value.Trim().ToLowerInvariant();
                if (temp.StartsWith("http://"))
                {
                    temp = temp.Substring(7, temp.Length - 7);
                }
                if (temp.StartsWith("https://"))
                {
                    temp = temp.Substring(8, temp.Length - 8);
                }
                temp = temp.TrimEnd('/');
                _DomainName = temp;
            }
        }

        public StoreDomain()
        {
            Id = 0;
            StoreId = 0;
            DomainName = string.Empty;
        }
    }
}
