using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildMaker.Core;

namespace BuildMakerCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Writer writer = new Writer();

            BuildManager build = new BuildManager(writer);
            build.Build();            
        }       
    }
}
