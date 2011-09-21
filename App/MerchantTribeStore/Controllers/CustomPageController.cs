using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribeStore.Controllers.Shared;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore.Controllers
{
    public class CustomPageController : BaseStoreController
    {
        //
        // GET: /CustomPage/
        public ActionResult Index(string slug)
        {
            Category cat = MTApp.CatalogServices.Categories.FindBySlugForStore(slug, 
                                        MTApp.CurrentRequestContext.CurrentStore.Id);
            if (cat == null) cat = new Category();
            
            ViewBag.Title = cat.MetaTitle;
            ViewBag.MetaKeywords = cat.MetaKeywords;
            ViewBag.MetaDescription = cat.MetaDescription;

            ViewBag.DisplayHtml = TagReplacer.ReplaceContentTags(cat.Description,
                                                                 this.MTApp,
                                                                 "", 
                                                                 Request.IsSecureConnection);

            // Record Category View
            MerchantTribe.Commerce.SessionManager.CategoryLastId = cat.Bvin;

            return View(cat);
        }
        
    }
}
