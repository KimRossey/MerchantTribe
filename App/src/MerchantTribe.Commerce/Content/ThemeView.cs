using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MerchantTribe.Commerce.Content
{
    public class ThemeView
    {
        private ThemeInfo _Info = new ThemeInfo();
        private string _PreviewImageUrl = string.Empty;        
        private bool _IsCustomized = false;

        public ThemeInfo Info
        {
            get { return _Info; }
            set { _Info = value; }
        }
        public string PreviewImageUrl
        {
            get { return _PreviewImageUrl; }
            set { _PreviewImageUrl = value; }
        }
        public bool IsCustomized
        {
            get { return _IsCustomized; }
            set { _IsCustomized = value; }
        }      
  
        public void LoadInstalledTheme(long storeId, string themeId)
        {
            string themePhysicalRoot = Storage.DiskStorage.BaseStoreThemePhysicalPath(storeId, themeId);

            if (!Directory.Exists(themePhysicalRoot))
            {
                return;
            }

            _Info = new ThemeInfo();
            if (File.Exists(Path.Combine(themePhysicalRoot,"bvtheme.xml")) )
            {
                _Info.LoadFromString(File.ReadAllText(Path.Combine(themePhysicalRoot,"bvtheme.xml")));
            }

            if (File.Exists(Path.Combine(themePhysicalRoot,".customized")))
            {
                _IsCustomized = true;
            }
            else
            {
                _IsCustomized = false;  
            }

            if (File.Exists(Path.Combine(themePhysicalRoot,"preview.png")))
            {
                _PreviewImageUrl = Storage.DiskStorage.BaseStoreThemeUrl(storeId, themeId, true) + "preview.png";
            }
            else
            {
                _PreviewImageUrl = string.Empty;
            }
        }

        public void LoadAvailableTheme(string themeId)
        {
            string themePhysicalRoot = WebAppSettings.ApplicationBuiltinThemesPath + "\\theme-" + themeId + "\\";

            if (!Directory.Exists(themePhysicalRoot))
            {
                return;
            }

            _Info = new ThemeInfo();
            if (File.Exists(Path.Combine(themePhysicalRoot, "bvtheme.xml")))
            {
                _Info.LoadFromString(File.ReadAllText(Path.Combine(themePhysicalRoot, "bvtheme.xml")));
            }

            if (File.Exists(Path.Combine(themePhysicalRoot, ".customized")))
            {
                _IsCustomized = true;
            }
            else
            {
                _IsCustomized = false;
            }

            if (File.Exists(Path.Combine(themePhysicalRoot, "preview.png")))
            {
                _PreviewImageUrl = "/content/themes/theme-" + themeId + "/preview.png";
            }
            else
            {
                _PreviewImageUrl = string.Empty;
            }
        }
    }
}
