using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildMaker.Core;

namespace VersionMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            Writer writer = new Writer();

            VersionManager vers = new VersionManager(writer);
            vers.VersionApp();     
        }
    }
}
