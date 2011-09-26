using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce.Catalog;
using MerchantTribeStore.Models;
using System.ComponentModel.DataAnnotations;

namespace MerchantTribeStore.Models
{
    public class CategoryPageViewModel
    {
        public Category LocalCategory { get; set; }
        public PagerViewModel PagerData { get; set; }
        public List<SingleCategoryViewModel> SubCategories { get; set; }
        public List<SingleProductViewModel> Products { get; set; }
        
        public CategoryPageViewModel()
        {
            LocalCategory = null;
            PagerData = new PagerViewModel();
            SubCategories = new List<SingleCategoryViewModel>();
            Products = new List<SingleProductViewModel>();
        }
    }
}