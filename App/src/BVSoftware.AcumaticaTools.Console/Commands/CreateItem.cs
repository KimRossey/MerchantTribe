using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools;

namespace BVSoftware.AcumaticaToolsConsole.Commands
{
    class CreateItem: BaseCommand
    {
        public CreateItem()
        {
            AddName("createitem");
            AddName("ci");
        }

        public override void GetHelp()
        {
            Console.WriteLine("Creates a new Inventory Item in Acumatica");
            Console.WriteLine();
            Console.WriteLine(" ci SKU description price qtyonhand");
        }

        public override bool Execute(string[] args, ServiceContext context)
        {
            if (args.Length > 4)
            {
                string sku = args[1];
                string description = args[2];
                string price = args[3];
                string qtyonhand = args[4];

                ProductData data = new ProductData(){ UniqueId = sku, Description = description, Price = decimal.Parse(price),
                                                    BaseWeight = 0
                                                    /*, 
                                                    QuantityOnHand = decimal.Parse(qtyonhand)*/};

                ProductData result = Products.GetOrCreateProduct(data, context);
                if (!String.IsNullOrEmpty(result.UniqueId))
                {
                    Console.WriteLine("Product Found/Created: " + result.UniqueId + " " + result.Description);
                    Console.WriteLine("Price: " + result.Price.Value.ToString("C"));
                    //Console.WriteLine("Qty On Hand: " + result.QuantityOnHand.ToString());
                }
                else
                {
                    Console.WriteLine("No product Found/Created with that sku");
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
