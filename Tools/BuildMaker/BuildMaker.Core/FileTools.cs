using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BuildMaker.Core
{
    class FileTools
    {
        public static string CurrentWorkingDirectory()
        {
             return Directory.GetCurrentDirectory();
        }
        public static string ReleaseFolder(string versionNumber)
        {
            if (versionNumber.StartsWith("v"))
            {
                return Path.Combine(CurrentWorkingDirectory(), "releases\\" + versionNumber);
            }
            else
            {
                return Path.Combine(CurrentWorkingDirectory(), "releases\\v" + versionNumber);
            }
        }
        public static string CurrentBvisorConfigFolder()
        {
            return Path.Combine(CurrentWorkingDirectory(), ".bvisor");
        }
        public static string CurrentSourceFolder()
        {
            return Path.Combine(CurrentWorkingDirectory(), "source");
        }
        
        //public static string CurrentSourceFolderWithProject()
        //{
        //    ConfigurationFile config = new ConfigurationFile();
        //    config.ReadFromDisk(CurrentBvisorConfigFolder());
        //    return Path.Combine(CurrentSourceFolder(), config.Core.SCMFolder);
        //}
        //public static string CurrentFullVersion()
        //{
        //    ConfigurationFile config = new ConfigurationFile();
        //    config.ReadFromDisk(CurrentBvisorConfigFolder());

        //    string ver = "1.0.0";
        //    string build = "0";

        //    string versionFile = Path.Combine(CurrentSourceFolder(),config.Core.SCMFolder + "\\versionnumber.txt");
        //    if (File.Exists(versionFile))
        //    {
        //        string[] verlines = File.ReadAllLines(versionFile);
        //        if (verlines.Length > 0)
        //        {
        //            ver = verlines[0];
        //        }
        //    }

        //    string buildFile = Path.Combine(CurrentSourceFolder(), config.Core.SCMFolder + "\\buildnumber.txt");
        //    if (File.Exists(buildFile))
        //    {
        //        string[] buildlines = File.ReadAllLines(buildFile);
        //        if (buildlines.Length > 0)
        //        {
        //           build = buildlines[0];
        //        }
        //    }

        //    return ver + "." + build;
        //}


        public static void SingleFileCopy(string source, string dest, Writer writer)
        {
            writer.WriteLine("Copying file " + source);

            if (!File.Exists(source))
            {
                writer.WriteLine("Source file missing!");
                return;
            }

            try
            {                
                File.Copy(source, dest);
            }
            catch (Exception ex)
            {
                writer.WriteLine(ex.Message + " | " + ex.StackTrace);
            }            
        }

        public static void FileCopyNoBackup(string sourceFolder, string destFolder, Writer writer)
        {
            FileCopyNoBackup(sourceFolder, destFolder, writer, new List<string>());
        }

        public static void FileCopyNoBackup(string sourceFolder, string destFolder, Writer writer, List<string> foldersToIgnore)
        {
            writer.WriteLine("Copying to folder: " + destFolder);

            int i = 0;
            string[] subDirs;
            string[] files;
            string destinationFileName = string.Empty;

            files = Directory.GetFiles(sourceFolder);
            subDirs = Directory.GetDirectories(sourceFolder);

            // Create dest dir
            if (!CreateAndCheckDirectory(destFolder))
            {
                writer.WriteLine(destFolder + " does not exist and cannot be created.");
                throw new ApplicationException(destFolder + " does not exist and cannot be created.");
            }

            for (i = 0; i < files.Length; i++)
            {
                destinationFileName = destFolder + "\\" + Path.GetFileName(files[i]);
                
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
                // Optionally skip over Obj and Debug folders when copying
                bool doChild = true;
                if (foldersToIgnore != null)
                {
                    string toTest = Path.GetFileName(subDirs[i]).Trim().ToLowerInvariant();
                    if (foldersToIgnore.Contains(toTest))
                    {
                        doChild = false;
                    }                   
                }

                if (doChild)
                {
                    FileCopyNoBackup(sourceFolder + "\\" + Path.GetFileName(subDirs[i]),
                        destFolder + "\\" + Path.GetFileName(subDirs[i]), writer, foldersToIgnore);
                }
            }
        }

        public static bool CreateAndCheckDirectory(string dir)
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
        public static bool CreateAndCheckRelativeDirectory(string relativeFolderName)
        {
            return CreateAndCheckDirectory(Path.Combine(CurrentWorkingDirectory(), relativeFolderName));
        }
                    
        private static int FileCount(string dirName)
        {
            int iCount = 0;
            if (Directory.Exists(dirName))
            {
                iCount = Directory.GetFiles(dirName).Length;
                if (Directory.GetDirectories(dirName).Length > 0)
                {
                    string[] subdirs = Directory.GetDirectories(dirName);
                    int i = 0;
                    for (i = 0; i < subdirs.Length; i++)
                    {
                        iCount += FileCount(subdirs[i]);
                    }
                }
            }
            return iCount;
        }

        public static void ChangeFileAttributesAndRemoveDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);

                foreach (string f in files)
                {
                    File.SetAttributes(f, FileAttributes.Normal);
                    File.Delete(f);
                }

                string[] dirs = Directory.GetDirectories(path);

                foreach (string d in dirs)
                {
                    ChangeFileAttributesAndRemoveDirectory(d);
                    Directory.Delete(d);
                }
            }
        }

        public static FileInfo GetTempFileInfo()
        {
            string tempFileName = string.Empty;
            FileInfo myFileInfo = null;

            try
            {                
                tempFileName = Path.GetTempFileName();
                myFileInfo = new FileInfo(tempFileName);
                myFileInfo.Attributes = FileAttributes.Temporary;
            }
            catch (Exception e)
            {                
                return null;
            }
            return myFileInfo;
        }

        public static void RemoveFile(string fileName, Writer writer)
        {
            writer.WriteLine("Removing: " + fileName);
            if (File.Exists(fileName))
            {                                
                    File.SetAttributes(fileName, FileAttributes.Normal);
                    File.Delete(fileName);                                
            }
            else
            {
                writer.WriteLine("Couldn't locate file to remove: " + fileName);
            }
        }
        public static void RemoveDirectory(string path, Writer writer)
        {
            RemoveDirectory(path, false, writer);
        }
        public static void RemoveDirectory(string path, bool removeDirectory, Writer writer)
        {
            writer.WriteLine("Removing: " + path);
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);

                foreach (string f in files)
                {
                    File.SetAttributes(f, FileAttributes.Normal);
                    File.Delete(f);
                }

                string[] dirs = Directory.GetDirectories(path);

                foreach (string d in dirs)
                {
                    RemoveDirectory(d, writer);
                    Directory.Delete(d);
                }

                if (removeDirectory)
                {
                    Directory.Delete(path);
                }
            }
            else
            {
                writer.WriteLine("Couldn't locate path to change attributes: " + path);
            }
        }

    }
}
