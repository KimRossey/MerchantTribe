using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.Areas.AdminContent.Models
{
    public class FileManagerViewModel
    {
        public List<string> Directories { get; set; }
        public List<string> Files { get; set; }        
        public string Path { get; set; }
        public string ParentPath { 
            get
            {
                string potential = this.Path.TrimEnd('\\');
                string parent = string.Empty;
                if (potential.Length > 0)
                {
                    parent = System.IO.Path.GetDirectoryName(potential);
                }
                return parent;
            }
        }
        public bool AllowParentPath
        {
            get
            {
                return Path.Trim().Length > 0;
            }
            
        }
        
        public FileManagerViewModel(string path)
        {
            this.Path = path.TrimStart('\\');
            this.Directories = new List<string>();
            this.Files = new List<string>();            
        }

        public string ChildPath(string directoryName)
        {
            return Path + "\\" + directoryName;
        }
    }
}