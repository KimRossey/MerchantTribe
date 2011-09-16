using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools;

namespace BVSoftware.AcumaticaToolsConsole.Commands
{
    class SimpleTest: BaseCommand
    {
        public SimpleTest()
        {
            AddName("simpletest");
            AddName("test");
        }
        public override void GetHelp()
        {
            Console.WriteLine("Runs a simple test.");
            Console.WriteLine();
            Console.WriteLine(" ac simpletest Connects and lists users");
        }

        public override bool Execute(string[] args, ServiceContext context  )
        {
            Connections.RunSimpleTest(context);            
            return true;          
        }
              
        
    }
}
