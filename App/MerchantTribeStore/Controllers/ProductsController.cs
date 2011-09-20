using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribeStore.Models;
using MerchantTribe.Commerce.Catalog;
using MerchantTribeStore.Controllers.Shared;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore.Controllers
{
    public class ProductsController : BaseAppController
    {
        // GET: /products/rendersingleproduct
        [ChildActionOnly]
        public ActionResult RenderSingleProduct(Product p)
        {
            if (p == null) return Content("");
            
            SingleProductViewModel model = new SingleProductViewModel();
            model.Item = p;            
            model.UserPrice = MTApp.PriceProduct(p, MTApp.CurrentCustomer, null);            
            model.ImageUrl = MerchantTribe.Commerce.Storage.DiskStorage.ProductImageUrlSmall(
                             MTApp.CurrentStore.Id,                    
                            p.Bvin,
                            p.ImageFileSmall,
                            Request.IsSecureConnection);
            model.ProductLink = UrlRewriter.BuildUrlForProduct(p, MTApp.CurrentRequestContext.RoutingContext, string.Empty);

            return View(model);
        }
    }
}
