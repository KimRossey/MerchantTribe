using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce;
using MerchantTribeStore.Filters;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Utilities;

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

        private Category EditorSetup(string id)
        {
            Category result = MTApp.CatalogServices.Categories.Find(id);
            ViewData["ViewInStoreUrl"] = UrlRewriter.BuildUrlForCategory(new CategorySnapshot(result), MTApp.CurrentRequestContext.RoutingContext);
            LoadTemplates();
            //this.UrlsAssociated1.LoadUrls();
            return result;
        }

                
        [HttpGet]
        public ActionResult Edit(string id)
        {
            Category c = EditorSetup(id);
            return View(c);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditPost(string id, Category posted)
        {
            Category c = EditorSetup(id);

            bool success = this.Save(c, posted);
            if (!success)
            {
                this.FlashFailure("Error during update. Please check event log.");
                
            }
            else
            {
                this.FlashSuccess("Category Updated Successfully at " + DateTime.Now.ToLongTimeString());
                c = EditorSetup(id);

                if (Request.Form["SaveButton"] != null || Request.Form["SaveButton.x"] != null)
                {
                    return Redirect("~/bvadmin/catalog.categories.aspx");
                }                
            }
            
            return View(c);
        }

        private bool Save(Category current, Category posted)
        {

            bool result = false;

            if (current != null)
            {
                current.Name = posted.Name.Trim();
                //current.Description = posted.Description.Trim();
                //current.PreTransformDescription = posted.PreTransformDescription.Trim();
                current.MetaDescription = posted.MetaDescription.Trim();
                current.MetaTitle = posted.MetaTitle.Trim();
                current.MetaKeywords = posted.MetaKeywords.Trim();
                current.ShowInTopMenu = false; // This could be changed to support show in top menu functionality
                current.SourceType = CategorySourceType.CustomPage;

                string oldUrl = current.RewriteUrl;

                // no entry, generate one
                if (current.RewriteUrl.Trim().Length < 1)
                {
                    current.RewriteUrl = MerchantTribe.Web.Text.Slugify(current.Name, true, true);
                }
                else
                {
                    current.RewriteUrl = MerchantTribe.Web.Text.Slugify(posted.RewriteUrl, true, true);
                }
                
                if (UrlRewriter.IsCategorySlugInUse(current.RewriteUrl, current.Bvin, MTApp.CurrentRequestContext))
                {
                    FlashWarning("The requested URL is already in use by another item.");
                    return false;
                }

                current.TemplateName = posted.TemplateName;
                
                current.CustomerChangeableSortOrder = true;

                
                result = MTApp.CatalogServices.Categories.Update(current);
                                
                if (result == true)
                {
                    if (oldUrl != string.Empty)
                    {
                        if (oldUrl != current.RewriteUrl)
                        {
                            MTApp.ContentServices.CustomUrls.Register301(oldUrl,
                                                  current.RewriteUrl,
                                                  current.Bvin, CustomUrlType.Category, MTApp.CurrentRequestContext,
                                                  MTApp);
                            
                        }
                    }
                }
            }

            return result;
        }


        private void LoadTemplates()
        {
            List<string> templates = MTApp.ThemeManager().ListTemplatesForCurrentTheme();
            ViewData["templates"] = templates;
        }
    }
}
