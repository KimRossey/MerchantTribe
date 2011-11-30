using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Storage
{
    public class ButtonSnapshot
    {
        private string _FileName = string.Empty;
        private MerchantTribeApplication _app = null;
        private string _themeId = string.Empty;

        public string FileName { get { return _FileName; } }

        public ButtonSnapshot(string fileName, MerchantTribeApplication app, string themeId)
        {
            _FileName = fileName;
            _app = app;
            _themeId = themeId;
        }

        public string Url()
        {
            return Url(false);
        }
        public string Url(bool isSecure)
        {
            return DiskStorage.ThemeButtonUrl(_app, _themeId, _FileName, isSecure);
        }
    }
}
