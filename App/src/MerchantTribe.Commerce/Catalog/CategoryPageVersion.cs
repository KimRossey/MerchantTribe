using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Commerce.Content.Parts;
using MerchantTribe.Commerce.Content;

namespace MerchantTribe.Commerce.Catalog
{
    public class CategoryPageVersion
    {
        public long Id { get; set; }
        public string PageId { get; set; }
        public string AdminName { get; set; }
        public long AvailableScheduleId { get; set; }
        public DateTime AvailableStartDateUtc { get; set; }
        public DateTime AvailableEndDateUtc { get; set; }        
        public RootColumn Root { get; set; }
        public PublishStatus PublishedStatus { get; set; }

        public CategoryPageVersion()
        {
            Id = 0;
            PageId = string.Empty;            
            AdminName = string.Empty;
            AvailableScheduleId = 0;
            AvailableStartDateUtc = new DateTime(1900, 1, 1);
            AvailableEndDateUtc = new DateTime(9999, 1, 1);
            Root = new RootColumn();            
            PublishedStatus = PublishStatus.Draft;
        }
       
    }
}
