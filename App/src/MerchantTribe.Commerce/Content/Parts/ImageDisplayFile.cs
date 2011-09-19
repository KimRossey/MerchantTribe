using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Parts
{
    public class ImageDisplayFile
    {
        public int SortOrder { get; set; }
        public string FileName { get; set; }
        public string AltText { get; set; }

        public ImageDisplayFile()
        {
            this.SortOrder = 0;
            this.FileName = string.Empty;
            this.AltText = string.Empty;
        }
    }
}
