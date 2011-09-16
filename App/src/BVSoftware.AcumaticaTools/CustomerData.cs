using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Geography;

namespace BVSoftware.AcumaticaTools
{
    public class CustomerData
    {
        public string AcumaticaId { get; set; }
        public string Bvin { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
		public string PaymentMethod { get; set; }
        public string PaymentIdentifier { get; set; }
        public IAddress BillingAddress { get; set; }
        public IAddress ShippingAddress { get; set; }
        
        public CustomerData()
        {
            AcumaticaId = string.Empty;
            PaymentIdentifier = "CASH";
            PaymentMethod = "CASH";
            BillingAddress = new MerchantTribe.Web.Geography.SimpleAddress();
            ShippingAddress = new MerchantTribe.Web.Geography.SimpleAddress();
        }
        
    }
}
