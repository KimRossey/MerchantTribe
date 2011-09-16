using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools;

namespace BVSoftware.AcumaticaToolsConsole.Commands
{
    class CustomerSearch: BaseCommand
    {
        public CustomerSearch()
        {
            AddName("customersearch");
            AddName("customer");
        }

        public override void GetHelp()
        {
            Console.WriteLine("Search for a customer");
            Console.WriteLine();
            Console.WriteLine(" customersearch email      Searches for an email");
        }

        public override bool Execute(string[] args, ServiceContext context)
        {
            if (args.Length > 1)
            {
                string email = args[1];
                string customerId = Customers.GetCustomerIdByEmail(email, context);
                if (customerId != string.Empty)
                {
                    Console.WriteLine("Customer Id = " + customerId);
                }
                else
                {
                    Console.WriteLine("No Customer Found with that Email");
                }
                return true;
            }

            return false;
        }
    }
    
}
