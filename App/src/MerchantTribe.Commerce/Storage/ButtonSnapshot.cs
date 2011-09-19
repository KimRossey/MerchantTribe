using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Storage
{
    public class ButtonSnapshot
    {
        private string _FileName = string.Empty;
        private long _storeId = 0;
        private string _themeId = string.Empty;

        public string FileName { get { return _FileName; } }

        public ButtonSnapshot(string fileName, long storeId, string themeId)
        {
            _FileName = fileName;
            _storeId = storeId;
            _themeId = themeId;
        }

        public string Url()
        {
            return Url(false);
        }
        public string Url(bool isSecure)
        {
            return DiskStorage.ThemeButtonUrl(_storeId, _themeId, _FileName, isSecure);
        }
    }
}
