using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Storage
{
    public class StoreAssetSnapshot
    {
        private string _FileName = string.Empty;
        private long _storeId = 0;

        public string FileName { get { return _FileName; } }

        public StoreAssetSnapshot(string fileName, long storeId)
        {
            _FileName = fileName;
            _storeId = storeId;
        }

        public string Url()
        {
            return Url(false);
        }
        public string Url(bool isSecure)
        {
            return DiskStorage.StoreAssetUrl(_storeId, _FileName, isSecure);
        }

    }
}
