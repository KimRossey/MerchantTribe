using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BVSoftware.CreateStoreCore
{
    public class SiteBuilder
    {

        public delegate void ProgressReportDelegate(string message);
        public event ProgressReportDelegate ProgressReport;

        public bool CreateSite(SiteData data)
        {
            // Make sure data isn't null
            if (data == null)
            {
                wl("Site data was missing");
                return false;
            }

            wl("Starting install at " + DateTime.Now.ToString());
            wl("=================================================");

            // Create Local Path
            if (!CreateLocalPath(data.Location)) return false;

            wl("-------------------------------------------------");

            // Copy Files to Path

            // - Start File Copy Code
            if (data.InstallSourceCode)
            {
                FileCopyNoBackup(data.SourceFolder, data.Location);
            }
            // End File Copy Code

            wl("-------------------------------------------------");

            wl("Updating web.config file...");
            UpdateWebConfig(data);
            wl("-------------------------------------------------");

            //System.Threading.Thread.Sleep(500);
            wl("Files Installed");
            wl("=================================================");
            wl("");
            wl("What's Next?");
            wl("-------------------------------------------------");
            wl("1) Create an Empty Database in SQL Server");
            wl("2) Run the 3 SQL Scripts in Order ");
            wl("3) Create an IIS Web Application");
            wl("4) Open the web site in a browser");
            wl("");
            wl("Instructions.RTF file has more details");
            wl("");
            
            return true;
        }

        private void wl(string message)
        {
            if (this.ProgressReport != null)
            {
                this.ProgressReport(message);
            }
        }

        private bool CreateLocalPath(string requestedPath)
        {
            wl("Checking Local Path...");
            try
            {
                if (Directory.Exists(requestedPath) == false)
                {
                    wl("Creating path...");
                    Directory.CreateDirectory(requestedPath);
                }
                else
                {
                    wl("Path found.");
                }
                wl("Finished checking path.");
                return true;
            }
            catch (IOException ioex)
            {
                wl(ioex.Message);
                return false;
            }
        }

        private void FileCopyNoBackup(string source, string dest)
        {
            int i = 0;
            string[] subDirs;
            string[] files;
            string destinationFileName = string.Empty;

            files = Directory.GetFiles(source);
            subDirs = Directory.GetDirectories(source);

            // Create dest dir
            if (!CreateAndCheckDirectory(dest))
            {
                throw new ApplicationException(dest + " does not exist and cannot be created.");
            }

            wl("Copying... " + source);

            for (i = 0; i < files.Length; i++)
            {
                destinationFileName = dest + "\\" + Path.GetFileName(files[i]);

                File.Copy(files[i], destinationFileName, true);
                if (File.Exists(destinationFileName))
                {
                    File.SetAttributes(destinationFileName, FileAttributes.Normal);
                }
                else
                {
                    throw new ApplicationException("Unable to copy file " + files[i]);
                }
            }

            for (i = 0; i < subDirs.Length; i++)
            {
                FileCopyNoBackup(source + "\\" + Path.GetFileName(subDirs[i]),
                    dest + "\\" + Path.GetFileName(subDirs[i]));
            }
        }

        private bool CreateAndCheckDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (!Directory.Exists(dir))
            {
                return false;
            }
            return true;
        }

        private bool UpdateWebConfig(SiteData data)
        {
            bool result = false;
            try
            {

                string configFile = Path.Combine(data.Location, "MerchantTribeStore\\web.config");
                if (data.InstallSourceCode == false)
                {
                    configFile = Path.Combine(data.Location, "web.config");
                }

                if (File.Exists(configFile))
                {

                    FileInfo writerInfo = this.GetTemporaryFileInfo();
                    StreamWriter writer = new StreamWriter(writerInfo.OpenWrite());

                    using (StreamReader reader = File.OpenText(configFile))
                    {
                        string line = reader.ReadLine();

                        while (line != null)
                        {
                            bool foundReplacement = false;

                            // Connection Strings
                            if (line.Trim().StartsWith("<add name=\"commerce6ConnectionString\""))
                            {
                                wl("Found Match for Connection 1");
                                writer.WriteLine("<add name=\"commerce6ConnectionString\" connectionString=\"" + data.ConnectionString() + "\"/>");
                                foundReplacement = true;
                            }

                            if (TrySettingReplace(writer, line,
                               "BaseApplicationUrl", data.WebSiteAddress)) foundReplacement = true;


                            if (data.ForceDebug)
                            {                                
                                string _CompileMatch = "<compilation debug=\"false\" targetFramework=\"4.0\">";
                                string _CompileReplace = "<compilation debug=\"true\" targetFramework=\"4.0\">";
                                if (line.Trim().StartsWith(_CompileMatch))
                                {
                                    wl("Found Match for Debug Switch");
                                    writer.WriteLine(_CompileReplace);
                                    foundReplacement = true;
                                }        
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
                    File.Copy(writerInfo.FullName, configFile, true);

                    result = true;
                }

            }
            catch (Exception ex)
            {
                wl(ex.Message);
            }

            return result;
        }

        private bool TrySettingReplace(StreamWriter writer, string line, string key, string value)
        {
            string _appStringFormat = "<add key=\"{0}\" value=\"{1}\" />";
            string _appMatch = "<add key=\"{0}\"";

            if (line.Trim().StartsWith(String.Format(_appMatch, key)))
            {
                wl("Found Match for " + key);
                writer.WriteLine(String.Format(_appStringFormat, key, value));
                return true;
            }

            return false;
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
