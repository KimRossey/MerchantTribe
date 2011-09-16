using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools;

namespace BVSoftware.AcumaticaToolsConsole.Commands
{
    class TestConnection : BaseCommand
    {
        public TestConnection()
        {
            AddName("testconnection");
            AddName("tc");
        }
        public override void GetHelp()
        {
            Console.WriteLine("Tests a connection to an Acumatica service.");
            Console.WriteLine();
            Console.WriteLine("  ac testconnection  Connects and lists users");
        }

        public override bool Execute(string[] args, ServiceContext context)
        {                    
            Connections.TestConnection(context.Username, context.Password, context.SiteAddress);

            return true;
        }


    }
}
