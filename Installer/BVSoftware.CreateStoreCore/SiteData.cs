using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.CreateStoreCore
{
    public class SiteData
    {
        public string Location { get; set; }
        public string SQLServer { get; set; }
        public string SQLDatabase { get; set; }
        public string SQLUsername { get; set; }
        public string SQLPassword { get; set; }
        public string SourceFolder { get; set; }
        public bool InstallSourceCode { get; set; }
        public string WebSiteAddress { get; set; }
        public bool ForceDebug { get; set; }
        public string PatcherBackup { get; set; }
        public string PatcherSource { get; set; }

        public string ConnectionString()
        {
            string result = "server=" + SQLServer + ";";
            result += "database=" + SQLDatabase + ";";
            result += "uid=" + SQLUsername + ";";
            result += "pwd=" + SQLPassword + ";";
            return result;
        }

        public void PrepArgs()
        {
            this.WebSiteAddress = this.WebSiteAddress.Trim();

            if (!this.WebSiteAddress.StartsWith("http://") &&
                !this.WebSiteAddress.StartsWith("https://"))
            {
                this.WebSiteAddress = "http://" + this.WebSiteAddress;
            }
            if (this.WebSiteAddress.EndsWith("/") == false)
            {
                this.WebSiteAddress += "/";
            }
        }
    }
}
