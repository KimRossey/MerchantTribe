using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;
using System.Text;

namespace BVCommerce.Controllers
{    
    public class SiteMapController : Shared.BaseStoreController
    {
        //
        // GET: /SiteMap/        
        public ActionResult Index()
        {
            List<CategorySnapshot> allCats = BVApp.CatalogServices.Categories.FindAllPaged(1, 5000);
            ViewBag.Title = SiteTerms.GetTerm(SiteTermIds.SiteMap);            
            return View(allCats);
        }

        [OutputCache(VaryByHeader = "Host", VaryByParam = "none", VaryByCustom = "disablecsscaching", Duration = 150)]
        public ActionResult Xml()
        {
            string sitemap = SiteMapGenerator.BuildForStore(this.BVApp);
            return Content(sitemap, "text/xml");             
        }
     
    }
}
