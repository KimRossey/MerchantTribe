using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BVSoftware.CreateStoreCore
{
    public class FileCopyHelper
    {

        private static string _patchNewFileName = "NewFiles.txt";

        public static bool CopyAllFiles(string source, string dest, string backupPath)
        {
            bool result = false;

            if (Directory.Exists(source) == false)
            {
                return false;
            }
            else
            {
                if (Directory.Exists(dest) == false)
                {
                    return false;
                }
                else
                {
                    try
                    {
                        FileCopy(source, dest, backupPath);
                        result = true;
                    }
                    catch
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        public static void FileCopy(string source, string dest, string backup)
        {
            FileCopy(source, dest, backup, backup);
        }

        private static void FileCopy(string source, string dest, string backup, string rootBackupPath)
        {
            int i = 0;
            string[] subDirs;
            string[] files;
            string destinationFileName = string.Empty;
            string backupFileName = string.Empty;

            files = Directory.GetFiles(source);
            subDirs = Directory.GetDirectories(source);

            // Create dest dir
            if (!CreateAndCheckDirectory(dest))
            {
                throw new ApplicationException(dest + " does not exist and cannot be created.");
            }

            for (i = 0; i < files.Length; i++)
            {
                destinationFileName = dest + "\\" + Path.GetFileName(files[i]);
                backupFileName = backup + "\\" + Path.GetFileName(files[i]);

                if (!CreateAndCheckDirectory(backup))
                {
                    throw new ArgumentException(backup + " does not exist and cannot be created.");
                }

                if (File.Exists(destinationFileName))
                {
                    //we are overwriting this file, we want to make sure that we have the oldest backup
                    if (!File.Exists(backupFileName))
                    {
                        File.Copy(destinationFileName, backupFileName, true);
                        if (!File.Exists(backupFileName))
                        {
                            throw new ApplicationException("Unable to backup file " + destinationFileName);
                        }
                    }
                }
                else
                {
                    //this is a new file, so add it to our new files list
                    AddToNewFileList(destinationFileName, rootBackupPath);
                }

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
                FileCopy(source + "\\" + Path.GetFileName(subDirs[i]),
                    dest + "\\" + Path.GetFileName(subDirs[i]),
                    backup + "\\" + Path.GetFileName(subDirs[i]),
                    rootBackupPath);
            }
        }

        public static void FileCopyNoBackup(string source, string dest)
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

        private static bool CreateAndCheckDirectory(string dir)
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

        private static void AddToNewFileList(string filename, string rootBackupPath)
        {
            string fileListPath = rootBackupPath + _patchNewFileName;

            if (!File.Exists(fileListPath))
            {
                using (StreamWriter sw = File.CreateText(fileListPath))
                {
                    sw.WriteLine(filename);
                    sw.Flush();
                    sw.Close();
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(fileListPath))
                {
                    sw.WriteLine(filename);
                    sw.Flush();
                    sw.Close();
                }
            }
        }

        public static void Rollback(string backupPath, string destination)
        {
            // delete all newly placed files
            if (File.Exists(backupPath + _patchNewFileName))
            {
                using (StreamReader sr = File.OpenText(backupPath + _patchNewFileName))
                {
                    while (sr.Peek() >= 0)
                    {
                        string fileName = sr.ReadLine();
                        if (File.Exists(fileName))
                        {
                            File.Delete(fileName);
                        }
                    }
                    sr.Close();
                }
            }

            if (File.Exists(destination + _patchNewFileName))
            {
                File.Delete(destination + _patchNewFileName);
            }

            RollbackFileCopy(backupPath, destination);
        }

        private static void RollbackFileCopy(string source, string dest)
        {
            int i = 0;
            string[] subDirs;
            string[] files;
            string destinationFileName = string.Empty;
            string backupFileName = string.Empty;

            files = Directory.GetFiles(source);
            subDirs = Directory.GetDirectories(source);

            for (i = 0; i < files.Length; i++)
            {
                destinationFileName = dest + "\\" + Path.GetFileName(files[i]);

                File.Copy(files[i], destinationFileName, true);
                if (File.Exists(destinationFileName))
                {
                    File.SetAttributes(destinationFileName, FileAttributes.Normal);
                }
            }

            for (i = 0; i < subDirs.Length; i++)
            {
                RollbackFileCopy(source + "\\" + Path.GetFileName(subDirs[i]), dest + "\\" + Path.GetFileName(subDirs[i]));
            }

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

        public static void ChangeFileAttributesAndRemove(string path)
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
                    ChangeFileAttributesAndRemove(d);
                    Directory.Delete(d);
                }
            }
        }
    }
}
