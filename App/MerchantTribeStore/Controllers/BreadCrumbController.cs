using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribeStore.Controllers.Shared;
using MerchantTribeStore.Models;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore.Controllers
{
    public class BreadCrumbController : BaseAppController
    {
        //
        // GET: /BreadCrumb/
        [ChildActionOnly]
        public ActionResult CategoryTrail(Category cat, List<BreadCrumbItem> extras)
        {
            CategorySnapshot snap = new CategorySnapshot(cat);

            BreadCrumbViewModel model = new BreadCrumbViewModel();
            model.HomeName = MerchantTribe.Commerce.Content.SiteTerms.GetTerm(MerchantTribe.Commerce.Content.SiteTermIds.Home);

            LoadTrailForCategory(model, snap);

            if (extras != null)
            {                
                foreach (BreadCrumbItem item in extras)
                {
                    model.Items.Enqueue(item);
                }                
            }
            return View("BreadCrumb", model);
        }
        private void LoadTrailForCategory(BreadCrumbViewModel model, CategorySnapshot cat)
        {
            if (cat == null) return;
            if (cat.Hidden) return;

            List<CategorySnapshot> allCats = MTApp.CatalogServices.Categories.FindAllPaged(1, int.MaxValue);
                                    

            List<CategorySnapshot> trail = new List<CategorySnapshot>();
            trail = Category.BuildTrailToRoot(cat.Bvin, MTApp.CurrentRequestContext);

            if (trail == null) return;
            
            // Walk list backwards
            for (int j = trail.Count - 1; j >= 0; j += -1)
            {
                if (j != 0)
                {
                    model.Items.Enqueue(AddCategoryLink(trail[j]));                    
                }
                else
                {
                    model.Items.Enqueue(new BreadCrumbItem() { Name = trail[j].Name });
                }
            }                                    
        }
        private BreadCrumbItem AddCategoryLink(CategorySnapshot c)
        {
            BreadCrumbItem result = new BreadCrumbItem();
            result.Name = c.Name;
            result.Title = c.MetaTitle;            
            result.Link = UrlRewriter.BuildUrlForCategory(c, 
                MTApp.CurrentRequestContext.RoutingContext);
            return result;
        }
                        
    }
}
