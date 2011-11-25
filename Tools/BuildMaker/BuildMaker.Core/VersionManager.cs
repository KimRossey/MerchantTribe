using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BuildMaker.Core
{
    public class VersionManager
    {
        private Writer _Writer = null;

        public VersionManager(Writer writer)
        {
            _Writer = writer;
        }

        public void VersionApp()
        {            
            try
            {
                // Increase version number            
                string currentVersion = VersionUp();
            }
            catch (Exception ex)
            {
                _Writer.WriteLine("EXCEPTION: " + ex.Message);
                _Writer.WriteLine("STACKTRACE: " + ex.StackTrace);
            }

            _Writer.WriteToDiskAndClear("builds\\versionlog.txt");            
        }

        #region "Versioning"

        private string VersionUp()
        {            
            VersionIncreaseBuildNumber();            
            string currentVersion = VersionGetFullBuildNumber();            
            VersionUpdateAssemblyFiles(currentVersion);            
            //ScmTagAndPush("v" + currentVersion);            
            return currentVersion;
        }
        private string VersionGetFullBuildNumber()
        {            
            string ver = "1.0.0";
            string build = "0";

            string versionFile = "versionnumber.txt";
            if (File.Exists(versionFile))
            {
                string[] verlines = File.ReadAllLines(versionFile);
                if (verlines.Length > 0)
                {
                    ver = verlines[0];
                }
            }

            string buildFile = "buildnumber.txt";
            if (File.Exists(buildFile))
            {
                string[] buildlines = File.ReadAllLines(buildFile);
                if (buildlines.Length > 0)
                {
                   build = buildlines[0];
                }
            }

            return ver + "." + build;
        }
        private bool VersionIncreaseBuildNumber()
        {
            _Writer.WriteLine("Increasing Build Number");
            try
            {                
                string buildFile = "buildnumber.txt";
                if (File.Exists(buildFile))
                {
                    string[] buildlines = File.ReadAllLines(buildFile);
                    if (buildlines.Length > 0)
                    {
                        string buildstring = buildlines[0];
                        int build = 0;
                        int.TryParse(buildstring, out build);
                        build += 1;
                        File.WriteAllText(buildFile, build.ToString() + System.Environment.NewLine);
                        _Writer.WriteLine("Build Number Updated to " + build.ToString());
                        //ScmCommit("Updated Build Number to " + build.ToString());
                        //ScmPush();                        
                    }
                }
            }
            catch (Exception ex)
            {
                _Writer.WriteLine(ex.Message);
                return false;
            }

            return true;
        }
        private bool VersionUpdateAssemblyFiles(string currentVersion)
        {
            _Writer.WriteLine("Updating Assembly Files");

            string assemblyFile = "versionfiles.txt";
                                    
            if (!File.Exists(assemblyFile))
            {
                _Writer.WriteLine("Could not locate assembly list file. Please check the name");
                return false;
            }

            string[] lines = File.ReadAllLines(assemblyFile);
            if (lines != null)
            {
                foreach (string l in lines)
                {
                    VersionUpdateSingleAssemblyFile(l, currentVersion);
                }
            }

            return true;
        }
        private bool VersionUpdateSingleAssemblyFile(string fileName, string ver)
        {
            if (fileName.Trim().Length < 1) return false;
                                                
            if (!File.Exists(fileName))
            {
                _Writer.WriteLine("Could not locate file to update: " + fileName);
                return false;
            }

            string result = "";
            string[] lines = File.ReadAllLines(fileName);

            foreach (string st in lines)
            {
                if (st.Contains("[assembly: AssemblyVersion(\"") && (!st.Contains("//")))
                {
                    result += "[assembly: AssemblyVersion(\"" + ver + "\")]" + System.Environment.NewLine;
                }
                else if (st.Contains("[assembly: AssemblyFileVersion(\"") && (!st.Contains("//")))
                {
                    result += "[assembly: AssemblyFileVersion(\"" + ver + "\")]" + System.Environment.NewLine;
                }
                else if (st.Contains("const string APPLICATION_VERSION = \""))
                {
                    result += "const string APPLICATION_VERSION = \"" + ver + "\";" + System.Environment.NewLine;
                }
                else
                {
                    result += st + System.Environment.NewLine;
                }
            }

            File.WriteAllText(fileName, result);

            _Writer.WriteLine("Updated: " + fileName);
            return true;
        }

        #endregion
    }
}
