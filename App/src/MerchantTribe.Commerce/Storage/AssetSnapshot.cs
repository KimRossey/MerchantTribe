using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Storage
{
    public class AssetSnapshot
    {
        private string _FileName = string.Empty;
        private MerchantTribeApplication _app = null;
        private string _themeId = string.Empty;

        public string FileName { get { return _FileName; } }

        public AssetSnapshot(string fileName, MerchantTribeApplication app, string themeId)
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
            return DiskStorage.AssetUrl(_app, _themeId, _FileName, isSecure);
        }
    }
}
