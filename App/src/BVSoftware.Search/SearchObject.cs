using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Search
{
    public class SearchObject
    {
        public long Id { get; set; }
        public int ObjectType { get; set; }
        public string ObjectId { get; set; }
        public long SiteId { get; set; }
        public string Title { get; set; }
        public DateTime LastIndexUtc { get; set; }

        public SearchObject()
        {
            Id = 0;
            ObjectType = 0;
            ObjectId = string.Empty;
            SiteId = 0;
            Title = string.Empty;
            LastIndexUtc = new DateTime(1900, 1, 1);
        }
    }
}
