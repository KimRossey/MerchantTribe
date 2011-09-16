using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools;

namespace BVSoftware.AcumaticaToolsConsole
{
    public interface ICommand
    {
        List<string> Names();
        bool NameMatches(string arg);
        bool Execute(string[] args, ServiceContext context);
        void GetHelp();
    }
}
