using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;
using MerchantTribeStore.Controllers.Shared;
using MerchantTribeStore.Models;
using MerchantTribeStore.Areas.ContentBlocks.Models;

namespace MerchantTribeStore.Areas.ContentBlocks.Controllers
{
    public class FeaturedProductsController : BaseAppController
    {
        //
        // GET: /ContentBlocks/FeaturedProducts/
        public ActionResult Index(ContentBlock block)
        {
            ProductListViewModel model = new ProductListViewModel();
            model.Items = MTApp.CatalogServices.Products.FindFeatured(1, 100);            
            return View(model);
        }     
    }
}
