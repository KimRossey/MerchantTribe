using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.Models
{
    public class PagerViewModel
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages
        {
            get { return MerchantTribe.Web.Paging.TotalPages(this.TotalItems, this.PageSize); }
        }
        public string PagerUrlFormat { get; set; }
        private string _PagerUrlFormatFirst = string.Empty;
        // Url for first page 
        public string PagerUrlFormatFirst 
        {
            get
            {
                if (_PagerUrlFormatFirst.Trim().Length < 1) return PagerUrlFormat;
                return _PagerUrlFormatFirst;
            }
            set { _PagerUrlFormatFirst = value; }         
        }

        public PagerViewModel()
        {
            PageSize = 9;
            CurrentPage = 1;
            TotalItems = 0;
            PagerUrlFormat = "";
            PagerUrlFormatFirst = "";
        }
    }
}