using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Storage
{
    public class StoreAssetSnapshot
    {
        private string _FileName = string.Empty;
        private MerchantTribeApplication _app = null;

        public string FileName { get { return _FileName; } }

        public StoreAssetSnapshot(string fileName, MerchantTribeApplication app)
        {
            _FileName = fileName;
            _app = app;
        }

        public string Url()
        {
            return Url(false);
        }
        public string Url(bool isSecure)
        {
            return DiskStorage.StoreAssetUrl(_app, _FileName, isSecure);
        }

    }
}
