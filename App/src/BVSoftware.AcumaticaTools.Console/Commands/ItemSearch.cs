using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools;

namespace BVSoftware.AcumaticaToolsConsole.Commands
{
    class ItemSearch: BaseCommand
    {
        public ItemSearch()
        {
            AddName("itemsearch");
            AddName("item");
        }

        public override void GetHelp()
        {
            Console.WriteLine("Search for an item");
            Console.WriteLine();
            Console.WriteLine(" itemsearch SKU      Searches for an item by SKU");
        }

        public override bool Execute(string[] args, ServiceContext context)
        {
            if (args.Length > 1)
            {
                string sku = args[1];
                ProductData p = Products.GetProductByUniqueId(sku, context);
                if (!String.IsNullOrEmpty(p.UniqueId))
                {
                    Console.WriteLine("Product Found: " + p.UniqueId + " " + p.Description);
                    if (p.Price.HasValue)
                    {
                        Console.WriteLine(p.Price.Value.ToString("C"));
                    }
                    else
                    {
                        Console.WriteLine("No Price Data");
                    }
                    //if (p.QuantityOnHand.HasValue)
                    //{
                    //    Console.WriteLine("Quantity On Hand: " + p.QuantityOnHand.Value.ToString());
                    //}
                }
                else
                {
                    Console.WriteLine("No Product Found with that SKU");
                }
                return true;
            }

            return false;
        }
    }
}
