using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools;

namespace BVSoftware.AcumaticaToolsConsole
{
    class BaseCommand : ICommand
    {
        private List<string> names = new List<string>();

        public List<string> Names()
        {
            return names;
        }

        protected void AddName(string name)
        {
            this.names.Add(name);
        }

        public bool NameMatches(string arg)
        {
            foreach (string s in names)
            {
                if (s.ToLowerInvariant() == arg) return true;
            }

            return false;
        }

        public virtual bool Execute(string[] args, ServiceContext context)
        {
            return false;
        }
        public virtual void GetHelp()
        {
            Console.WriteLine("No help is available for this command.");
        }
    }
}
