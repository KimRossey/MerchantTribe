using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using BVSoftware.CreateStoreCore;

namespace CreateNewStoreCmd
{
    class Program
    {
        static void ShowHelp()
        {
            Console.WriteLine("This command line application will install a copy of MerchantTribe. You will");
            Console.WriteLine("need to specify the location and database information.");
            Console.WriteLine("After this program is finished you will still need to:");
            Console.WriteLine("  a) Create your Database");
            Console.WriteLine("  b) Run the SQL Scripts in the /BVAdmin/sql folder of the installed site.");
            Console.WriteLine("  c) Create a web application (or site) in IIS");
            Console.WriteLine();
            Console.WriteLine("Sample Usage:");
            Console.WriteLine("CreateNewStoreCmd.exe -path=c:\\projects\\mt -sqlserver=localhost ....");
            Console.WriteLine();
            Console.WriteLine(" -path={path}            The destination directory");
            Console.WriteLine(" -sqlserver={server}     Name of your SQL Server");
            Console.WriteLine(" -sqldatabase={db}       Name of your SQL Database");
            Console.WriteLine(" -sqluser={user}         SQL User Account");
            Console.WriteLine(" -sqlpassword={pass}     SQL User Password");
            Console.WriteLine(" -debug=true             (optional) force debug mode in web.config");
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Console.WriteLine("Create MT Store | " + version);
            Console.WriteLine();

            if (args.Length < 6)
            {
                ShowHelp();
                return;
            }

            Console.WriteLine("Parsing Arguments");

            SiteData data = new SiteData();
            data.InstallSourceCode = true;            
            data.SourceFolder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\src\\";
            
            foreach (string arg in args)
            {
                ParseArg(data, arg);
            }

            data.PrepArgs();
            
            SiteBuilder builder = new SiteBuilder();
            builder.ProgressReport += new SiteBuilder.ProgressReportDelegate(builder_ProgressReport);

            if (builder.CreateSite(data))
            {
                Console.WriteLine("Create store SUCCESS!");
            }
            else
            {
                Console.WriteLine("Create store FAILED!");
            }
        }

        static void builder_ProgressReport(string message)
        {
            Console.WriteLine(message);
        }

        private static void ParseArg(SiteData data, string arg)
        {
            string[] argParts = arg.Split('=');
            if (argParts.Length < 2) return;

            switch(argParts[0].Trim().ToLowerInvariant())
            {
                case "-path":
                    data.Location = argParts[1];
                    break;
                case "-sqlserver":
                    data.SQLServer = argParts[1];
                    break;
                case "-sqldatabase":
                    data.SQLDatabase = argParts[1];
                    break;
                case "-sqluser":
                    data.SQLUsername = argParts[1];
                    break;
                case "-sqlpassword":
                    data.SQLPassword = argParts[1];
                    break;
                case "-url":
                    data.WebSiteAddress = argParts[1];
                    break;
                case "-debug":
                    data.ForceDebug = true;
                    break;
            }
        }
    }
}
