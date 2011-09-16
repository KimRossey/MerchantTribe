using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.AcumaticaToolsConsole
{
    class Program
    {

        static void Main(string[] args)
        {
            CommandManager manager = new CommandManager();

            if (args.Length < 1)
            {
                // interactive mode
                while (true)
                {
                    Console.WriteLine();
                    Console.Write("acumatica console: ");
                    string cmd = Console.ReadLine();
                    if (cmd.ToLowerInvariant() == "x" ||
                        cmd.ToLowerInvariant() == "exit" ||
                        cmd.ToLowerInvariant() == "q" ||
                        cmd.ToLowerInvariant() == "quit")
                    {
                        break;
                    }
                    manager.ParseCommand(cmd.Split(' '));
                }
            }
            else
            {
                manager.ParseCommand(args);
            }
        }
    }
}
