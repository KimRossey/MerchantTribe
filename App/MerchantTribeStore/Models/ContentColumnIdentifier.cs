using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.Models
{
    public class ContentColumnIdentifier
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ContentColumnIdentifier()
        {
            Id = string.Empty;
            Name = string.Empty;
        }
    }

    
}