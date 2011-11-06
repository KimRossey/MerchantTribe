using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildMaker.Core.Commands
{
    class Releases
    {
        private Writer _Writer = null;

        public Releases(Writer writer)
        {
            _Writer = writer;
        }
          
        private bool BuildRelease(string releaseName)
        {
            string releaseFolder = System.IO.Path.Combine(FileTools.CurrentWorkingDirectory(), "releases\\" + releaseName + "\\src\\App");
            _Writer.WriteLine("Release Folder: " + releaseFolder);
            if (!System.IO.Directory.Exists(releaseFolder))
            {
                _Writer.WriteLine("That release couldn't be found.");
                return false;
            }

            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = "msbuild";
            pProcess.StartInfo.Arguments = "/t:Rebuild /p:Configuration=Release";
            pProcess.StartInfo.UseShellExecute = true;
            pProcess.StartInfo.RedirectStandardOutput = false;
            pProcess.StartInfo.WorkingDirectory = releaseFolder;

            _Writer.WriteLine(pProcess.StartInfo.FileName + " " + pProcess.StartInfo.Arguments);
            _Writer.WriteLine("Building Release " + releaseName + ". Please Wait... ");

            pProcess.Start();            
            pProcess.WaitForExit();

            _Writer.WriteLine("Finished Building.");
            return true;
        }
    
    }
}
