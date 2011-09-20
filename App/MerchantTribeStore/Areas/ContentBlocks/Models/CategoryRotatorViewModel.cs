using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.Areas.ContentBlocks.Models
{
    public class CategoryRotatorViewModel
    {
        public string AltText { get; set; }
        public string IconUrl { get; set; }
        public string LinkUrl { get; set; }
        public string Name { get; set; }
        public bool OpenInNewWindow { get; set; }

        public CategoryRotatorViewModel()
        {
            this.AltText = string.Empty;
            this.IconUrl = string.Empty;
            this.LinkUrl = string.Empty;
            this.Name = string.Empty;
            this.OpenInNewWindow = false;
        }
    }
}