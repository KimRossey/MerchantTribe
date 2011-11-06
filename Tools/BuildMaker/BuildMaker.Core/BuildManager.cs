using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;

namespace BuildMaker.Core
{
    public class BuildManager
    {
        private Writer _Writer = null;

        public BuildManager(Writer writer)
        {
            _Writer = writer;
        }

        public void Build()
        {
            string workingFolder = "Builds\\v";

            try
            {
                // Make sure we have a builds folder
                _Writer.WriteLine("Creating Build Directory");
                FileTools.CreateAndCheckRelativeDirectory("Builds");

                // Increase version number            
                string currentVersion = VersionUp();

                // Create Working Folder
                workingFolder = "Builds\\v" + currentVersion;
                _Writer.WriteLine("Creating Directory for Build v" + currentVersion);
                FileTools.CreateAndCheckRelativeDirectory(workingFolder);

                // Build the App in Release Mode
                BuildInReleaseMode();

                // Build Installer in Release Mode
                BuildInstaller();

                // Package Built Files with Installer
                string fullBuildFolder = Path.Combine(FileTools.CurrentWorkingDirectory(), workingFolder + "\\Full");
                FileTools.CreateAndCheckRelativeDirectory(fullBuildFolder);
                PackageInstaller(fullBuildFolder);

                // Package App Files
                PackageApp(fullBuildFolder + "\\src");
                
                // Zip Files
                ZipMainBuild(workingFolder, "MerchantTribe" + currentVersion.Replace(".","") + ".zip");
                CopyFreeFiles(workingFolder, fullBuildFolder + "\\src");
                ZipFreeBuild(workingFolder, "MerchantTribe" + currentVersion.Replace(".", "") + "_free.zip");
                ZipWebPlatformInstaller(workingFolder, "MerchantTribe" + currentVersion.Replace(".", "") + "_WebPI.zip");                
                                                
                // Try Auto Install
                if (File.Exists("AutoInstallBuilds.txt"))
                {
                    _Writer.WriteLine("Auto installing this build...");

                    string process = Path.Combine(FileTools.CurrentWorkingDirectory(), workingFolder + "\\Full\\CreateNewStoreCmd.exe");

                    string commands = File.ReadAllText("AutoInstallBuilds.txt");
                    commands = commands.Replace("{version}", currentVersion);

                    RunProcess(process, commands);
                }
                else
                {
                    _Writer.WriteLine("No Auto Install Requested");
                }

            }
            catch (Exception ex)
            {
                _Writer.WriteLine("EXCEPTION: " + ex.Message);
                _Writer.WriteLine("STACKTRACE: " + ex.StackTrace);
            }

            _Writer.WriteToDiskAndClear(workingFolder + "\\log.txt");            
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

        //#region scm
        //private void ScmCommit(string message)
        //{
        //    System.Diagnostics.Process pProcess2 = new System.Diagnostics.Process();
        //    pProcess2.StartInfo.FileName = "git";
        //    pProcess2.StartInfo.Arguments = "add .";
        //    pProcess2.StartInfo.UseShellExecute = true;
        //    pProcess2.StartInfo.RedirectStandardOutput = false;            
        //    _Writer.WriteLine(pProcess2.StartInfo.FileName + " " + pProcess2.StartInfo.Arguments);
        //    _Writer.WriteLine("Adding changed files... ");
        //    pProcess2.Start();
        //    pProcess2.WaitForExit();

        //    System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
        //    pProcess.StartInfo.FileName = "git";
        //    pProcess.StartInfo.Arguments = "commit -am " + "\"" + message + "\"";
        //    pProcess.StartInfo.UseShellExecute = true;
        //    pProcess.StartInfo.RedirectStandardOutput = false;            
        //    _Writer.WriteLine(pProcess.StartInfo.FileName + " " + pProcess.StartInfo.Arguments);
        //    _Writer.WriteLine("Commiting Changes. Please Wait... ");
        //    pProcess.Start();
        //    pProcess.WaitForExit();
        //    _Writer.WriteLine("Finished commiting changes.");
        //}
        //private void ScmPush()
        //{
        //    System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
        //    pProcess.StartInfo.FileName = "git";
        //    pProcess.StartInfo.Arguments = "push";
        //    pProcess.StartInfo.UseShellExecute = true;
        //    pProcess.StartInfo.RedirectStandardOutput = false;
        //    _Writer.WriteLine(pProcess.StartInfo.FileName + " " + pProcess.StartInfo.Arguments);
        //    _Writer.WriteLine("Pushing Changes. Please Wait... ");
        //    pProcess.Start();
        //    pProcess.WaitForExit();
        //    _Writer.WriteLine("Finished pushing changes.");
        //}
        //private void ScmTagAndPush(string tag)
        //{
        //    System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
        //    pProcess.StartInfo.FileName = "git";
        //    pProcess.StartInfo.Arguments = "tag -a -m \"Tagging Release " + tag + "\" " + tag;
        //    pProcess.StartInfo.UseShellExecute = true;
        //    pProcess.StartInfo.RedirectStandardOutput = false;
        //    _Writer.WriteLine(pProcess.StartInfo.FileName + " " + pProcess.StartInfo.Arguments);
        //    _Writer.WriteLine("Taging Repository. " + tag + " Please Wait... ");
        //    pProcess.Start();
        //    pProcess.WaitForExit();
            
        //    System.Diagnostics.Process pProcess2 = new System.Diagnostics.Process();
        //    pProcess2.StartInfo.FileName = "git";
        //    pProcess2.StartInfo.Arguments = "push origin " + tag;
        //    pProcess2.StartInfo.UseShellExecute = true;
        //    pProcess2.StartInfo.RedirectStandardOutput = false;
        //    _Writer.WriteLine(pProcess2.StartInfo.FileName + " " + pProcess2.StartInfo.Arguments);
        //    _Writer.WriteLine("Pushing Tags. Please Wait... ");
        //    pProcess2.Start();
        //    pProcess2.WaitForExit();

        //    _Writer.WriteLine("Finished pushing tags");
        //}
        //#endregion

        #region Building
        private bool BuildInReleaseMode()
        {
            string buildingDirectory = System.IO.Path.Combine(FileTools.CurrentWorkingDirectory(), "App");
            _Writer.WriteLine("Building Directory: " + buildingDirectory);
            if (!System.IO.Directory.Exists(buildingDirectory))
            {
                _Writer.WriteLine("That build directory couldn't be found.");
                return false;
            }

            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = "msbuild";
            pProcess.StartInfo.Arguments = "/t:Clean,Build /p:Configuration=Release";
            pProcess.StartInfo.UseShellExecute = true;
            pProcess.StartInfo.RedirectStandardOutput = false;
            pProcess.StartInfo.WorkingDirectory = buildingDirectory;

            _Writer.WriteLine(pProcess.StartInfo.FileName + " " + pProcess.StartInfo.Arguments);
            _Writer.WriteLine("Building for Release. Please Wait... ");

            pProcess.Start();
            pProcess.WaitForExit();

            _Writer.WriteLine("Finished Building.");
            return true;
        }
        private bool BuildInstaller()
        {
            string buildingDirectory = System.IO.Path.Combine(FileTools.CurrentWorkingDirectory(), "Installer");
            _Writer.WriteLine("Building Installer Directory: " + buildingDirectory);
            if (!System.IO.Directory.Exists(buildingDirectory))
            {
                _Writer.WriteLine("That build directory couldn't be found.");
                return false;
            }

            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = "msbuild";
            pProcess.StartInfo.Arguments = "/t:Clean,Build /p:Configuration=Release";
            pProcess.StartInfo.UseShellExecute = true;
            pProcess.StartInfo.RedirectStandardOutput = false;
            pProcess.StartInfo.WorkingDirectory = buildingDirectory;

            _Writer.WriteLine(pProcess.StartInfo.FileName + " " + pProcess.StartInfo.Arguments);
            _Writer.WriteLine("Building Installer. Please Wait... ");

            pProcess.Start();
            pProcess.WaitForExit();

            _Writer.WriteLine("Finished Building.");
            
            return true;
        }
        #endregion

        #region Packaging
        private void PackageInstaller(string destinationFolder)
        {
            _Writer.WriteLine("Packaging Installer");
            FileTools.SingleFileCopy("Installer\\CreateNewStore\\bin\\Release\\CreateNewStore.exe", destinationFolder + "\\CreateNewStore.exe", _Writer);
            FileTools.SingleFileCopy("Installer\\CreateNewStore\\bin\\Release\\BVSoftware.CreateStoreCore.dll", destinationFolder + "\\BVSoftware.CreateStoreCore.dll", _Writer);
            FileTools.SingleFileCopy("Installer\\CreateNewStore\\bin\\Release\\Instructions.rtf", destinationFolder + "\\Instructions.rtf", _Writer);
            FileTools.SingleFileCopy("Installer\\CreateNewStoreCmd\\bin\\Release\\CreateNewStoreCmd.exe", destinationFolder + "\\CreateNewStoreCmd.exe", _Writer);

            _Writer.WriteLine("Creating src folder to hold output files");
            Directory.CreateDirectory(destinationFolder + "\\src");

            _Writer.WriteLine("Finished Packaging Intaller");
        }
        private void PackageApp(string destinationFolder)
        {
            _Writer.WriteLine("Copying App Files to Output");

            List<string> foldersToIgnoreDuringCopy = new List<string>();
            foldersToIgnoreDuringCopy.Add("obj");
            foldersToIgnoreDuringCopy.Add("debug");
            foldersToIgnoreDuringCopy.Add("release-hostedstaging");
            foldersToIgnoreDuringCopy.Add("release-hostedproduction");
            foldersToIgnoreDuringCopy.Add("release-source");                        

            // keep bin folder off ignore list so that bin from web app is copied
            FileTools.FileCopyNoBackup("App\\MerchantTribeStore", destinationFolder + "\\MerchantTribeStore", _Writer, foldersToIgnoreDuringCopy);

            // Add bin folder so source is smaller
            foldersToIgnoreDuringCopy.Add("bin");
            FileTools.FileCopyNoBackup("App\\lib", destinationFolder + "\\lib", _Writer, foldersToIgnoreDuringCopy);
            FileTools.FileCopyNoBackup("App\\packages", destinationFolder + "\\packages", _Writer, foldersToIgnoreDuringCopy);
            FileTools.FileCopyNoBackup("App\\src", destinationFolder + "\\src", _Writer, foldersToIgnoreDuringCopy);            

            FileTools.SingleFileCopy("App\\MerchantTribe.sln", destinationFolder + "\\MerchantTribeStore.sln", _Writer);

            // Copy Web Platform Installer Files
            _Writer.WriteLine("Copying Web Platform Installer Files");
            FileTools.SingleFileCopy("Installer\\MicrosoftWebDeploy\\Manifest.xml", destinationFolder + "\\Manifest.xml", _Writer);
            FileTools.SingleFileCopy("Installer\\MicrosoftWebDeploy\\parameters.xml", destinationFolder + "\\Parameters.xml", _Writer);
            FileTools.SingleFileCopy("App\\MerchantTribeStore\\BVAdmin\\SqlScripts\\Full\\CreateTables.sql", destinationFolder + "\\CreateTables.sql", _Writer);
            FileTools.SingleFileCopy("App\\MerchantTribeStore\\BVAdmin\\SqlScripts\\Full\\CreateProcedures.sql", destinationFolder + "\\CreateProcedures.sql", _Writer);
            FileTools.SingleFileCopy("App\\MerchantTribeStore\\BVAdmin\\SqlScripts\\Full\\PopulateData.sql", destinationFolder + "\\PopulateData.sql", _Writer);
            
            // Create placeholder for "Sites"
            _Writer.WriteLine("Creating placeholder for sites at " + destinationFolder + "\\MerchantTribeStore\\images\\sites\\placeholder.txt");
            FileTools.CreateAndCheckDirectory(destinationFolder + "\\MerchantTribeStore\\images\\sites");
            File.WriteAllText(destinationFolder + "\\MerchantTribeStore\\images\\sites\\placeholder.txt", "This is a placeholder");

            // Clean Unwanted Files
            _Writer.WriteLine("Cleaning Unwanted Files");
            //FileTools.RemoveDirectory(destinationFolder + "\\src\\BVSoftware.AcumaticaTools", true, _Writer);
            //FileTools.RemoveDirectory(destinationFolder + "\\src\\BVSoftware.AcumaticaTools.Console", true, _Writer);
            //FileTools.RemoveDirectory(destinationFolder + "\\src\\BVSoftware.Shipping.FedEx", true, _Writer);            
            FileTools.RemoveFile(destinationFolder + "\\MerchantTribeStore\\Web.Debug.config", _Writer);
            FileTools.RemoveFile(destinationFolder + "\\MerchantTribeStore\\Web.Release.config", _Writer);
            FileTools.RemoveFile(destinationFolder + "\\MerchantTribeStore\\Web.Release-Hosted.config", _Writer);
            FileTools.RemoveFile(destinationFolder + "\\MerchantTribeStore\\Web.Release-HostedStaging.config", _Writer);
            FileTools.RemoveFile(destinationFolder + "\\MerchantTribeStore\\Web.Release-Source.config", _Writer);
            FileTools.RemoveFile(destinationFolder + "\\MerchantTribeStore\\MerchantTribeStore.csproj.orig", _Writer);
            
            _Writer.WriteLine("Updating Config Files");
            PackagingUpdateConfig(destinationFolder + "\\MerchantTribeStore\\Web.config");

            _Writer.WriteLine("Finished Packaging App");
        }
        private void PackagingUpdateConfig(string filename)
        {
            if (File.Exists(filename))
            {
                _Writer.WriteLine("Found Config");
            }
            else
            {
                _Writer.Write("Config file not found");
                return;
            }

            try
            {                                                
                    FileInfo writerInfo = this.GetTemporaryFileInfo();
                    StreamWriter writer = new StreamWriter(writerInfo.OpenWrite());

                    using (StreamReader reader = File.OpenText(filename))
                    {
                        string line = reader.ReadLine();

                        while (line != null)
                        {
                            bool foundReplacement = false;

                            // Connection Strings
                            string _connStringMatch = "<add name=\"commerce6ConnectionString\" connectionString=";
                            string _connStringFormat = "<add name=\"commerce6ConnectionString\" connectionString=\"{0}\" />";
                            string _connString = "Data Source=localhost;Initial Catalog=CommerceHosted;User ID=commercehosted;Password=password;";                                     
                            if (line.Trim().StartsWith(_connStringMatch))
                            {
                                _Writer.WriteLine("Found Match for Connection String");
                                writer.WriteLine(String.Format(_connStringFormat, _connString));
                                foundReplacement = true;
                            }                       

                            if (TrySettingReplace(writer, _Writer, line,                                            
                                "BaseApplicationUrl", "http://localhost/")) foundReplacement = true;
                            if (TrySettingReplace(writer, _Writer, line,                                            
                                "MailServer", "localhost")) foundReplacement = true;
                            if (TrySettingReplace(writer, _Writer, line,                                            
                                "MailServerUsername", "user@domain.com")) foundReplacement = true;
                            if (TrySettingReplace(writer, _Writer, line,                                            
                                "MailServerPassword", "password")) foundReplacement = true;
                            if (TrySettingReplace(writer, _Writer, line,                                            
                                "MailServerSSL", "False")) foundReplacement = true;
                            if (TrySettingReplace(writer, _Writer, line,                                            
                                "MailServerPort", "25")) foundReplacement = true;
                            if (TrySettingReplace(writer, _Writer, line,                                            
                                "MailServerAsync", "False")) foundReplacement = true;
                            if (TrySettingReplace(writer, _Writer, line,                                            
                                "BillingStoreId", "-1")) foundReplacement = true;
                            if (TrySettingReplace(writer, _Writer, line,                                            
                                "storekey", "C589E3FB-9B9D-47EB-A694-690A97742C82")) foundReplacement = true;

                            // Compile
                            string _CompileMatch = "<compilation debug=\"true\" targetFramework=\"4.0\">";
                            string _CompileReplace = "<compilation debug=\"false\" targetFramework=\"4.0\">";
                            if (line.Trim().StartsWith(_CompileMatch))
                            {
                                _Writer.WriteLine("Found Match for Compile");
                                writer.WriteLine(_CompileReplace);
                                foundReplacement = true;
                            }        

                            // Write the default line if we didn't match
                            if (foundReplacement == false)
                            {
                                writer.WriteLine(line);
                            }

                            line = reader.ReadLine();
                        }
                    }

                    writer.Close();
                    File.Copy(writerInfo.FullName, filename, true);
                
            }
            catch (Exception ex)
            {
                _Writer.WriteLine(ex.Message);
            }
        }
        private void PackagingUpdateConfigForFree(string filename)
        {
            if (File.Exists(filename))
            {
                _Writer.WriteLine("Found Config");
            }
            else
            {
                _Writer.Write("Config file not found");
                return;
            }

            try
            {
                FileInfo writerInfo = this.GetTemporaryFileInfo();
                StreamWriter writer = new StreamWriter(writerInfo.OpenWrite());

                using (StreamReader reader = File.OpenText(filename))
                {
                    string line = reader.ReadLine();

                    while (line != null)
                    {
                        bool foundReplacement = false;
                        
                        if (TrySettingReplace(writer, _Writer, line,
                            "storekey", "288B5403-5FD9-4e80-BFD4-1892E5553511")) foundReplacement = true;
                        
                        // Write the default line if we didn't match
                        if (foundReplacement == false)
                        {
                            writer.WriteLine(line);
                        }

                        line = reader.ReadLine();
                    }
                }

                writer.Close();
                File.Copy(writerInfo.FullName, filename, true);

            }
            catch (Exception ex)
            {
                _Writer.WriteLine(ex.Message);
            }
        }

        private bool TrySettingReplace(StreamWriter writer, Writer _Writer, string line, string key, string value)
        {
            string _appStringFormat = "<add key=\"{0}\" value=\"{1}\" />";
            string _appMatch = "<add key=\"{0}\"";

            if (line.Trim().StartsWith(String.Format(_appMatch, key)))
            {
                _Writer.WriteLine("Found Match for " + key);
                writer.WriteLine(String.Format(_appStringFormat, key, value));
                return true;
            }

            return false;
        }

        #endregion

        public void CopyFreeFiles(string workingFolder, string destinationFolder)
        {
            FileTools.RemoveFile(destinationFolder + "\\MerchantTribeStore\\BVAdmin\\SetupWizard\\Eula.txt", _Writer);
            FileTools.SingleFileCopy("App\\MerchantTribeStore\\BVAdmin\\SetupWizard\\Eula-Free.txt", destinationFolder + "\\MerchantTribeStore\\BVAdmin\\SetupWizard\\Eula.txt", _Writer);
            
            string configFile = destinationFolder + "\\MerchantTribeStore\\Web.config";
            PackagingUpdateConfigForFree(configFile);
        }

        public void ZipMainBuild(string workingFolder, string outputName)        
        {
            _Writer.WriteLine("Zipping up Main Build");
            _Writer.WriteLine("Source Folder = " + workingFolder);
            _Writer.WriteLine("Output = " + outputName);
            using (ZipFile mainZip = new ZipFile())
            {                
                mainZip.AddDirectory(workingFolder + "\\Full\\src", "src");
                mainZip.AddFile(workingFolder + "\\Full\\BVSoftware.CreateStoreCore.dll", "");
                mainZip.AddFile(workingFolder + "\\Full\\CreateNewStore.exe", "");
                mainZip.AddFile(workingFolder + "\\Full\\CreateNewStoreCmd.exe", "");
                mainZip.AddFile(workingFolder + "\\Full\\Instructions.rtf", "");
                mainZip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");
                mainZip.Save(workingFolder + "\\" + outputName);
            }
            _Writer.WriteLine("Generating Hash for Zip...");
            string hash = HashTools.GetSHA1HashForFile(workingFolder + "\\" + outputName);
            File.WriteAllText(workingFolder + "\\" + outputName + ".sha1.txt", hash);
            _Writer.WriteLine("Finished Zipping Main Build");
        }
        public void ZipFreeBuild(string workingFolder, string outputName)
        {
            _Writer.WriteLine("Zipping up Free Build");
            _Writer.WriteLine("Source Folder = " + workingFolder);
            _Writer.WriteLine("Output = " + outputName);
            using (ZipFile mainZip = new ZipFile())
            {
                mainZip.AddDirectory(workingFolder + "\\Full\\src", "src");
                mainZip.AddFile(workingFolder + "\\Full\\BVSoftware.CreateStoreCore.dll", "");
                mainZip.AddFile(workingFolder + "\\Full\\CreateNewStore.exe", "");
                mainZip.AddFile(workingFolder + "\\Full\\CreateNewStoreCmd.exe", "");
                mainZip.AddFile(workingFolder + "\\Full\\Instructions.rtf", "");
                mainZip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");
                mainZip.Save(workingFolder + "\\" + outputName);
            }
            _Writer.WriteLine("Generating Hash for Zip...");
            string hash = HashTools.GetSHA1HashForFile(workingFolder + "\\" + outputName);
            File.WriteAllText(workingFolder + "\\" + outputName + ".sha1.txt", hash);
            _Writer.WriteLine("Finished Zipping Free Build");
        }
        public void ZipWebPlatformInstaller(string workingFolder, string outputName)
        {
            _Writer.WriteLine("Zipping up Web Platform Installer Build");
            _Writer.WriteLine("Source Folder = " + workingFolder);
            _Writer.WriteLine("Output = " + outputName);
            using (ZipFile mainZip = new ZipFile())
            {
                mainZip.AddDirectory(workingFolder + "\\Full\\src\\MerchantTribeStore", "MerchantTribeStore");
                mainZip.AddFile(workingFolder + "\\Full\\src\\Manifest.xml", "");
                mainZip.AddFile(workingFolder + "\\Full\\src\\Parameters.xml", "");
                mainZip.AddFile(workingFolder + "\\Full\\src\\CreateTables.sql", "");                
                mainZip.AddFile(workingFolder + "\\Full\\src\\CreateProcedures.sql", "");
                mainZip.AddFile(workingFolder + "\\Full\\src\\PopulateData.sql", "");
                mainZip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");
                mainZip.Save(workingFolder + "\\" + outputName);
            }
            _Writer.WriteLine("Generating Hash for Zip...");
            string hash = HashTools.GetSHA1HashForFile(workingFolder + "\\" + outputName);
            File.WriteAllText(workingFolder + "\\" + outputName + ".sha1.txt", hash);
            _Writer.WriteLine("Finished Zipping Web Platform Installer Build");
        }

        private void RunProcess(string processName, string commands)
        {
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = processName;
            pProcess.StartInfo.Arguments = commands;
            pProcess.StartInfo.UseShellExecute = true;
            pProcess.StartInfo.RedirectStandardOutput = false;
            
            _Writer.WriteLine(pProcess.StartInfo.FileName + " " + pProcess.StartInfo.Arguments);
            _Writer.WriteLine("Running. Please Wait... ");

            pProcess.Start();
            pProcess.WaitForExit();

            _Writer.WriteLine("Finished Running.");
        }

        private FileInfo GetTemporaryFileInfo()
        {
            string tempFileName;
            FileInfo myFileInfo;
            try
            {
                tempFileName = Path.GetTempFileName();
                myFileInfo = new FileInfo(tempFileName);
                myFileInfo.Attributes = FileAttributes.Temporary;
            }
            catch
            {
                return null;
            }

            return myFileInfo;
        }
    }

}
