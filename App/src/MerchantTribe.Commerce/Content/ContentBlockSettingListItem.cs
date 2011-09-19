using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content
{
    public class ContentBlockSettingListItem
    {
        public string Id { get; set; }
        public string ListName { get; set; }
        public string Setting1 { get; set; }
        public string Setting2 { get; set; }
        public string Setting3 { get; set; }
        public string Setting4 { get; set; }
        public string Setting5 { get; set; }
        public string Setting6 { get; set; }
        public string Setting7 { get; set; }
        public string Setting8 { get; set; }
        public string Setting9 { get; set; }
        public string Setting10 { get; set; }
        public int SortOrder { get; set; }

        public ContentBlockSettingListItem()
        {
            Id = System.Guid.NewGuid().ToString();
            ListName = string.Empty;
            Setting1 = string.Empty;
            Setting2 = string.Empty;
            Setting3 = string.Empty;
            Setting4 = string.Empty;
            Setting5 = string.Empty;
            Setting6 = string.Empty;
            Setting7 = string.Empty;
            Setting8 = string.Empty;
            Setting9 = string.Empty;
            Setting10 = string.Empty;
            SortOrder = 1;
        }

        public ContentBlockSettingListItem Clone()
        {
            ContentBlockSettingListItem clone = new ContentBlockSettingListItem();

            //clone.Id = System.Guid.NewGuid().ToString();
            clone.ListName = this.ListName;
            clone.Setting1 = this.Setting1;
            clone.Setting2 = this.Setting2;
            clone.Setting3 = this.Setting3;
            clone.Setting4 = this.Setting4;
            clone.Setting5 = this.Setting5;
            clone.Setting6 = this.Setting6;
            clone.Setting7 = this.Setting7;
            clone.Setting8 = this.Setting8;
            clone.Setting9 = this.Setting9;
            clone.Setting10 = this.Setting10;
            clone.SortOrder = this.SortOrder;

            return clone;
        }
    }
}
