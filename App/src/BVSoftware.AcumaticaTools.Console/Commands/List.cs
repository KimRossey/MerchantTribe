using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools;

namespace BVSoftware.AcumaticaToolsConsole.Commands
{
    class List: BaseCommand
    {
        public List()
        {
            AddName("list");
        }

        public override void GetHelp()
        {
            Console.WriteLine("Lists Accounts and Classses");
            Console.WriteLine();
            Console.WriteLine(" list taxes          List all tax categories");
        }

        public override bool Execute(string[] args, ServiceContext context)
        {
            if (args.Length > 1)
            {
                string type = args[1];

                List<AccountDescriptor> result = new List<AccountDescriptor>();

                switch (type.Trim().ToLowerInvariant())
                {
                    case "tax":
                        result = Accounts.ListAllTaxClasses(context);
                        break;
                }

                if (context.Errors.Count > 0)
                {
                    Console.WriteLine("Errors:");
                    foreach (ServiceError e in context.Errors)
                    {
                        Console.WriteLine(e.ErrorCode + " " + e.Description);
                    }
                }

                foreach (AccountDescriptor a in result)
                {
                    Console.WriteLine("[" + a.Id + "] - " + a.Description);
                }
                return true;
            }

            return false;
        }
    }
}
