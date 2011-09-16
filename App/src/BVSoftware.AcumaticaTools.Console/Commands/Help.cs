using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.AcumaticaToolsConsole.Commands
{
    class Help: BaseCommand
    {
        public Help()
        {
            AddName("help");
            AddName("h");
            AddName("?");
        }

        public override void GetHelp()
        {
            Console.WriteLine("The help function lists help for other commands.");
            Console.WriteLine();
            Console.WriteLine("Useage: ac help [commandname]");
            Console.WriteLine("        ac h [commandname]");
            Console.WriteLine();
        }

        public override bool Execute(string[] args, BVSoftware.AcumaticaTools.ServiceContext context)
        {
            if (args.Length > 1)
            {
                string name = args[1];
                CommandManager manager2 = new CommandManager();
                GetHelpForCommand(name, manager2.AvailableCommands());
            }
            else
            {
                // Call Version Command
                CommandManager manager3 = new CommandManager();
                manager3.ParseCommand(new string[] { "v" });

                Console.WriteLine();
                Console.WriteLine("Useage: ac [command] [commandarguments]");
                Console.WriteLine();
                Console.WriteLine("Available Commands: ");


                foreach (ICommand cmd in manager3.AvailableCommands())
                {
                    StringBuilder sb = new StringBuilder();
                    List<string> names = cmd.Names();
                    for (int i = 0; i < names.Count; i++)
                    {
                        sb.Append(names[i]);
                        if (i < names.Count - 1)
                        {
                            sb.Append(", ");
                        }
                    }
                    Console.WriteLine("   * " + sb.ToString());
                }
            }

            return false;
        }


        private void GetHelpForCommand(string commandname, List<ICommand> availableCommands)
        {
            bool found = false;

            foreach (ICommand cmd in availableCommands)
            {
                if (cmd.NameMatches(commandname.ToLowerInvariant()))
                {
                    found = true;
                    cmd.GetHelp();
                }
            }

            if (!found)
            {
                Console.WriteLine("No help was found for that command");
            }
        }
    }
}
