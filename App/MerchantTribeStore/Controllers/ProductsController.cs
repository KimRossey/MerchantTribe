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
        public ActionResult RenderSingleProduct(Product product)
        {
            if (product == null) return Content("");
            
            SingleProductViewModel model = new SingleProductViewModel();
            model.Item = product;            
            model.UserPrice = MTApp.PriceProduct(product, MTApp.CurrentCustomer, null);            
            model.ImageUrl = MerchantTribe.Commerce.Storage.DiskStorage.ProductImageUrlSmall(
                             MTApp.CurrentStore.Id,                    
                            product.Bvin,
                            product.ImageFileSmall,
                            Request.IsSecureConnection);
            model.ProductLink = UrlRewriter.BuildUrlForProduct(product, MTApp.CurrentRequestContext.RoutingContext, string.Empty);

            return View(model);
        }
    }
}
