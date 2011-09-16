using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Search
{
    public class SearchObjectWord
    {
        public long SearchObjectId { get; set; }
        public long SiteId { get; set; }
        public long WordId { get; set; }
        public int Score { get; set; }

        public SearchObjectWord()
        {
            SearchObjectId = 0;
            SiteId = 0;
            WordId = 0;
            Score = 0;
        }
    }
}
