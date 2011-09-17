using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment
{
    public class CustomerData
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Street { get; set; }        
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string IpAddress { get; set; }

        public string ShipFirstName { get; set; }
        public string ShipLastName { get; set; }
        public string ShipCompany { get; set; }
        public string ShipStreet { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
        public string ShipPhone { get; set; }

        public CustomerData()
        {
            UserId = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Company = string.Empty;
            Street = string.Empty;
            City = string.Empty;
            Region = string.Empty;
            PostalCode = string.Empty;
            Country = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            IpAddress = "0.0.0.0";

            ShipFirstName = string.Empty;
            ShipLastName = string.Empty;
            ShipCompany = string.Empty;
            ShipStreet = string.Empty;
            ShipCity = string.Empty;
            ShipRegion = string.Empty;
            ShipPostalCode = string.Empty;
            ShipCountry = string.Empty;
            ShipPhone = string.Empty;
        }
    }
}
