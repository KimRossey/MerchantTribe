using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools;

namespace BVSoftware.AcumaticaToolsConsole.Commands
{
    class SampleOrder: BaseCommand
    {
        public SampleOrder()
        {
            AddName("sampleorder");
            AddName("so");
        }

        public override void GetHelp()
        {
            Console.WriteLine("Creates a new SampleOrder");
            Console.WriteLine();
            Console.WriteLine(" so OrderNumber");
        }

        public override bool Execute(string[] args, ServiceContext context)
        {
            if (args.Length > 1)
            {
                string orderNumber = args[1];

                OrderData o = new OrderData();
                o.BVOrderNumber = orderNumber;
                o.TimeOfOrder = DateTime.Now;
                o.Customer.AcumaticaId = "AAABVSOFTW";
                o.Customer.FirstName = "Adam";
                o.Customer.LastName = "Smith";
                o.Customer.Email = "aaa@bvsoftware.com";

                o.ShippingAddress = new MerchantTribe.Web.Geography.SimpleAddress();
                o.ShippingAddress.Street = "99 John Street";
                o.ShippingAddress.City = "New York";
                o.ShippingAddress.PostalCode = "10038";
                o.ShippingAddress.CountryData.Bvin = MerchantTribe.Web.Geography.Country.FindByISOCode("US").Bvin;
                o.ShippingAddress.CountryData.Name = "US";
                o.ShippingAddress.RegionData.Abbreviation = "NY";
                o.ShippingAddress.RegionData.Name = "NY";
                

                ProductData p = new ProductData() { UniqueId = "CPU00001", Description = "CPU Item", Price = 49.74m /*, QuantityOnHand = 0*/ };
                o.Items.Add(new OrderItemData() { Product = p, Quantity = 3, LineTotal=149.22m });

                o.TaxTotal = 5.55m;
                
                o.Shipping.ShippingTotal = 7.77m;
                o.Shipping.ShippingMethodId = "FEDEX";
                o.Shipping.ShippingMethodDescription = "FedEx Overnight";

                o.GrandTotal = 162.54m;

                OrderData result = Orders.CreateNewOrder(o, context);                
                
                if (!String.IsNullOrEmpty(result.AcumaticaOrderNumber))
                {
                    Console.WriteLine("Order Created: " + result.AcumaticaOrderNumber + " " + result.TimeOfOrder.ToString());
                    Console.WriteLine("Total: " + result.GrandTotal.ToString());
                }
                else
                {
                    Console.WriteLine("Order Not Created");
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
