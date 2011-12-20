using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce;
using MerchantTribeStore.Filters;
using MerchantTribe.Commerce.Membership;

namespace MerchantTribeStore.Areas.AdminCatalog.Controllers
{
    [HandleError]
    [ValidateInput(false)]
    public class CategoriesCustomController : MerchantTribeStore.Controllers.Shared.BaseAdminController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Set Tab Type before other stuff happens
            base.SelectedTab = AdminTabType.Catalog;
            base.OnActionExecuting(filterContext);
        }

        //
        // GET: /BVAdmin/Catalog/Categories/Custom/Edit                
        public ActionResult Edit(string id)
        {            
            Category c = MTApp.CatalogServices.Categories.Find(id);            
            return View(c);
        }

    }
}
