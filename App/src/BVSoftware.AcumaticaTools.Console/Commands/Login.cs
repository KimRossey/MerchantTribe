using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools;

namespace BVSoftware.AcumaticaToolsConsole.Commands
{
    class Login : BaseCommand
    {
        public Login()
        {
            AddName("login");
        }

        public override void GetHelp()
        {
            Console.WriteLine("Login to Acumatica Site");
            Console.WriteLine();
            Console.WriteLine(" login username password [siteaddress]");
        }

        public override bool Execute(string[] args, ServiceContext context)
        {
            if (args.Length > 2)
            {
                string username = args[1];
                string password = args[2];
                string siteAddress = string.Empty;
                if (args.Length > 3)
                {
                    siteAddress = args[3];
                }
                else
                {
                    siteAddress = Properties.Settings.Default.SiteAddress;
                }
                
                context = Connections.Login(username, password, siteAddress);
                context.NewItemTaxAccountId = Properties.Settings.Default.NewItemTaxCategoryId;
                
                if (context.HasLoggedIn)
                {
                    Console.WriteLine("Logged in!");
                }
                else
                {
                    Console.WriteLine("Not logged in");
                    if (context.Errors.Count > 0)
                    {
                        foreach (ServiceError e in context.Errors)
                        {
                            Console.WriteLine(e.Description);
                        }
                    }
                }

                return true;
            }

            return false;
        }
    }
}
