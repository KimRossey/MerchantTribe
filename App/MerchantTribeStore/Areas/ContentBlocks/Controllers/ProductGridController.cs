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

namespace MerchantTribeStore.Areas.ContentBlocks.Controllers
{
    public class ProductGridController : BaseAppController
    {
        //
        // GET: /ContentBlocks/ProductGrid/
        public ActionResult Index(ContentBlock block)
        {
            List<SingleProductViewModel> model = LoadProductGrid(block);
            return View(model);
        }

        private List<SingleProductViewModel> LoadProductGrid(ContentBlock b)
        {
            List<SingleProductViewModel> result = new List<SingleProductViewModel>();

            List<ContentBlockSettingListItem> myProducts = b.Lists.FindList("ProductGrid");
            if (myProducts != null)
            {
                int column = 1;

                if (b != null)
                {
                    int maxColumns = b.BaseSettings.GetIntegerSetting("GridColumns");
                    if (maxColumns < 1) maxColumns = 3;

                    foreach (ContentBlockSettingListItem sett in myProducts)
                    {
                        string bvin = sett.Setting1;
                        Product p = MTApp.CatalogServices.Products.Find(bvin);
                        if (p != null)
                        {
                            bool isLastInRow = false;
                            bool isFirstInRow = false;
                            if ((column == 1))
                            {
                                isFirstInRow = true;
                            }

                            if ((column == maxColumns))
                            {
                                column = 1;
                                isLastInRow = true;
                            }
                            else
                            {
                                column += 1;
                            }
                            UserSpecificPrice price = MTApp.PriceProduct(p, MTApp.CurrentCustomer, null);
                            SingleProductViewModel vm = new SingleProductViewModel();                            
                            vm.IsFirstItem = isFirstInRow;
                            vm.IsLastItem = isLastInRow;
                            vm.Item = p;                            
                            vm.UserPrice = price;
                            vm.ImageUrl = MerchantTribe.Commerce.Storage.DiskStorage.ProductImageUrlSmall(
                             MTApp.CurrentStore.Id,
                            p.Bvin,
                            p.ImageFileSmall,
                            Request.IsSecureConnection);
                            vm.ProductLink = UrlRewriter.BuildUrlForProduct(p, MTApp.CurrentRequestContext.RoutingContext, string.Empty);

                            result.Add(vm);
                        }
                    }
                }                
            }

            return result;
        }

    }
}
