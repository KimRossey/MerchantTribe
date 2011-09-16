using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools;

namespace BVSoftware.AcumaticaToolsConsole.Commands
{
    class CreateCustomer: BaseCommand
    {
        public CreateCustomer()
        {
            AddName("createcustomer");
            AddName("cc");
        }

        public override void GetHelp()
        {
            Console.WriteLine("Creates a new Customer Record in Acumatica");
            Console.WriteLine();
            Console.WriteLine(" cc email firstname lastname");
        }

        public override bool Execute(string[] args, ServiceContext context)
        {
            if (args.Length > 3)
            {
                string email = args[1];
                string firstname = args[2];
                string lastname = args[3];

                CustomerData data = new CustomerData(){ Email = email, FirstName = firstname, LastName = lastname};
                data.BillingAddress = new MerchantTribe.Web.Geography.SimpleAddress();                
                data.BillingAddress.Street = "99 John Street";
                data.BillingAddress.City = "New York";
                data.BillingAddress.PostalCode = "10038";
                data.BillingAddress.CountryData.Bvin = MerchantTribe.Web.Geography.Country.FindByISOCode("US").Bvin;
                data.BillingAddress.CountryData.Name = "US";
                data.BillingAddress.RegionData.Abbreviation = "NY";
                data.BillingAddress.RegionData.Name = "NY";

                data.ShippingAddress = new MerchantTribe.Web.Geography.SimpleAddress();
                data.ShippingAddress.Street = "1445 5th Ave";
                data.ShippingAddress.City = "New York";
                data.ShippingAddress.PostalCode = "10011";
                data.ShippingAddress.CountryData.Bvin = MerchantTribe.Web.Geography.Country.FindByISOCode("US").Bvin;
                data.ShippingAddress.CountryData.Name = "US";
                data.ShippingAddress.RegionData.Abbreviation = "NY";
                data.ShippingAddress.RegionData.Name = "NY";

                string customerId = Customers.GetOrCreateCustomer(data, context);
                if (customerId != string.Empty)
                {
                    Console.WriteLine("Customer Id = " + customerId);
                }
                else
                {
                    Console.WriteLine("No Customer Found with that Email");
                    foreach (ServiceError e in context.Errors)
                    {
                        Console.WriteLine("ERROR: " + e.Description);
                    }
                }
                return true;
            }

            return false;
        }
    }
}
