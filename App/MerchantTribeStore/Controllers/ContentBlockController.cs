using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Content;
using MerchantTribeStore.Models.ContentBlock;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore.Controllers
{
    public class ContentBlockController : Shared.BaseAppController
    {
        //
        // GET: /ContentBlock/

        [ChildActionOnly]
        public ActionResult DisplayBlock(ContentBlock block)
        {
            switch (block.ControlName.Trim().ToLowerInvariant())
            {
                
                case "banner ad":
                    return BannerAd(block);
                case "category menu":
                    return CategoryMenu(block);
                case "category rotator":
                    return CategoryRotator(block);
                case "featured products":
                    return FeaturedProducts(block);
                case "html":
                    return Html(block);
                case "last products viewed":
                    return LastProductsViewed(block);
                case "product grid":
                    return ProductGrid(block);
                case "product rotator":
                    return ProductRotator(block);
                case "rss feed viewer":
                    return RssFeedViewer(block);
                case "side menu":
                    return SideMenu(block);
                case "top 10 products":
                    return Top10Products(block);
                case "top weekly sellers":
                    return TopWeeklySellers(block);

            }
            return Content("");
        }

        private ActionResult BannerAd(ContentBlock block)
        {
            return View("BannerAd", block);
        }
        private ActionResult CategoryMenu(ContentBlock block)
        {
            CategoryMenuViewModel model = new CategoryMenuViewModel();
            CategoryMenuLoadMenu(model, block);
            return View("CategoryMenu", model);
        }
        #region CategoryMenuFunctions

        private void CategoryMenuLoadMenu(CategoryMenuViewModel model, ContentBlock b)
        {
            model.CurrentId = CategoryMenuLocateCurrentCategory();            
            if (b != null)
            {    
                // Load Title
                string title = b.BaseSettings.GetSettingOrEmpty("Title");
                if (title.Trim().Length > 0)
                {
                    model.Title = "<h4>" + title + "</h4>";
                }

                
                model.ShowProductCounts = b.BaseSettings.GetBoolSetting("ShowProductCount");
                model.ShowCategoryCounts = b.BaseSettings.GetBoolSetting("ShowCategoryCount");
                model.sb.Append("<ul>");

                if (b.BaseSettings.GetBoolSetting("HomeLink") == true)
                {
                    CategoryMenuAddHomeLink(model);
                }

                string mode = b.BaseSettings.GetSettingOrEmpty("CategoryMenuMode");
                switch (mode)
                {
                    case "0":
                        // Root Categories Only
                        CategoryMenuLoadRoots(model);
                        break;
                    case "1":
                        // All Categories
                        CategoryMenuLoadAllCategories(model, b.BaseSettings.GetIntegerSetting("MaximumDepth"));
                        break;
                    case "2":
                        // Peers, Children and Parents
                        CategoryMenuLoadPeersAndChildren(model);
                        break;
                    case "3":
                        // Show root and expanded children
                        CategoryMenuLoadRootPlusExpandedChildren(model);
                        break;
                    default:
                        // All Categories
                        CategoryMenuLoadRoots(model);
                        break;
                }

                model.sb.Append("</ul>");
            }
        }
        private string CategoryMenuLocateCurrentCategory()
        {
            string result = string.Empty;


            if (Request.QueryString["categoryId"] != null)
            {
                result = Request.QueryString["categoryId"];
            }
            else
            {
                if (this is IProductPage)
                {
                    Product p = ((IProductPage)this).LocalProduct;
                    if (p != null)
                    {
                        CatalogService CatalogServices = MTApp.CatalogServices;
                        List<CategorySnapshot> categories = CatalogServices.FindCategoriesForProduct(p.Bvin);
                        if (categories != null)
                        {
                            if (categories.Count > 0)
                            {

                                bool found = false;

                                // scan category list to see if the last visited is in the collection
                                foreach (CategorySnapshot c in categories)
                                {
                                    if (c.Bvin == MerchantTribe.Commerce.SessionManager.CategoryLastId)
                                    {
                                        result = c.Bvin;
                                        found = true;
                                        break;
                                    }
                                }

                                if (found == false)
                                {
                                    result = categories[0].Bvin;
                                }

                            }
                        }
                    }
                }
                else
                {
                    result = MerchantTribe.Commerce.SessionManager.CategoryLastId;
                }
            }

            // Always reset to zero if we can't find anything
            if (result == string.Empty)
            {
                result = "0";
            }

            return result;
        }
        private void CategoryMenuLoadRoots(CategoryMenuViewModel model)
        {
            List<CategorySnapshot> cats = MTApp.CatalogServices.Categories.FindVisibleChildren("0");
            CategoryMenuAddCategoryCollection(model, cats, cats, 1, 1);
        }
        private void CategoryMenuAddHomeLink(CategoryMenuViewModel model)
        {
            model.sb.Append("<li>");
            model.sb.Append("<a href=\"" + Url.Content("~") + "\" title=\"Home\">Home</a>");
            model.sb.Append("</li>");
        }
        private void CategoryMenuAddSingleLink(CategoryMenuViewModel model, CategorySnapshot c, List<CategorySnapshot> allCats)
        {
            if (c.Bvin == model.CurrentId)
            {
                model.sb.Append("<li class=\"current\">");
            }
            else
            {
                model.sb.Append("<li>");
            }

            string title = c.MetaTitle;
            string text = c.Name;
            
            int childCount = 0;
            if (model.ShowProductCounts)
            {
                childCount += (MTApp.CatalogServices.CategoriesXProducts.FindForCategory(c.Bvin, 1, int.MaxValue).Count);
            }

            if (model.ShowCategoryCounts)
            {
                childCount += Category.FindChildrenInList(allCats, c.Bvin, false).Count;
            }

            if (childCount > 0)
            {
                text += " (" + childCount.ToString() + ")";
            }

            string url = UrlRewriter.BuildUrlForCategory(c, MTApp.CurrentRequestContext.RoutingContext);
            bool openNew = false;

            if (c.SourceType == CategorySourceType.CustomLink)
            {
                openNew = c.CustomPageOpenInNewWindow;
            }

            model.sb.Append("<a href=\"" + url + "\" title=\"" + title + "\">" + text + "</a>");            
        }
        private void CategoryMenuLoadAllCategories(CategoryMenuViewModel model, int maxDepth)
        {
            List<CategorySnapshot> allCats = MTApp.CatalogServices.Categories.FindAll();
            List<CategorySnapshot> children = Category.FindChildrenInList(allCats, "0", false);
            CategoryMenuAddCategoryCollection(model, allCats, children, 0, maxDepth);
        }
        private void CategoryMenuAddCategoryCollection(CategoryMenuViewModel model, List<CategorySnapshot> allCats, List<CategorySnapshot> cats, int currentDepth, int maxDepth)
        {
            if (cats != null)
            {
                foreach (CategorySnapshot c in cats)
                {

                    if (c.Hidden == false)
                    {

                        CategoryMenuAddSingleLink(model, c, allCats);

                        if ((maxDepth == 0) | (currentDepth + 1 < maxDepth))
                        {
                            List<CategorySnapshot> children = MTApp.CatalogServices.Categories.FindVisibleChildren(c.Bvin);
                            if (children != null)
                            {
                                if (children.Count > 0)
                                {
                                    model.sb.Append("<ul>" + System.Environment.NewLine);
                                    CategoryMenuAddCategoryCollection(model, allCats, children, currentDepth + 1, maxDepth);
                                    model.sb.Append("</ul>" + System.Environment.NewLine);
                                }
                            }
                        }

                        model.sb.Append("</li>");
                    }
                }
            }
        }
        private void CategoryMenuLoadRootPlusExpandedChildren(CategoryMenuViewModel model)
        {
            List<CategorySnapshot> allCats = MTApp.CatalogServices.Categories.FindAll();

            // Get Current Category
            CategorySnapshot currentCategory = Category.FindInList(allCats, model.CurrentId);

            if (currentCategory != null)
            {
                if (currentCategory.Bvin != string.Empty)
                {
                    model.CurrentId = currentCategory.Bvin;
                }

                // Find the trail from this category back to the root of the site
                List<CategorySnapshot> trail = new List<CategorySnapshot>();
                CategoryMenuBuildParentTrail(allCats, model.CurrentId, ref trail);
                if (trail == null)
                {
                    trail = new List<CategorySnapshot>();
                }

                if (trail.Count < 1)
                {
                    // Load Roots Only
                    CategoryMenuLoadRoots(model);
                }
                else
                {

                    string StartingRootCategoryId = currentCategory.Bvin;
                    StartingRootCategoryId = trail[trail.Count - 1].Bvin;


                    List<CategorySnapshot> roots = Category.FindChildrenInList(allCats, "0", false);
                    if (roots != null)
                    {
                        model.sb.Append("<ul>" + System.Environment.NewLine);

                        foreach (CategorySnapshot c in roots)
                        {
                            if (CategoryMenuIsInTrail(c.Bvin, trail))
                            {
                                CategoryMenuAddSingleLink(model, c, allCats);
                                List<CategorySnapshot> children = new List<CategorySnapshot>();
                                children = Category.FindChildrenInList(allCats, StartingRootCategoryId, false);
                                if (children != null)
                                {
                                    model.sb.Append("<ul>" + System.Environment.NewLine);
                                    CategoryMenuExpandInTrail(model, allCats, children, trail);
                                    model.sb.Append("</ul>" + System.Environment.NewLine);
                                }
                                model.sb.Append("</li>");

                                break;
                            }
                        }

                        model.sb.Append("</ul>" + System.Environment.NewLine);
                    }

                }
            }
            else
            {
                model.sb.Append("Invalid Category Id. Contact Administrator");
            }

        }
        private bool CategoryMenuIsInTrail(string testBvin, List<CategorySnapshot> trail)
        {
            bool result = false;

            if (trail != null)
            {
                foreach (CategorySnapshot c in trail)
                {
                    if (c.Bvin == testBvin)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }
        private void CategoryMenuExpandInTrail(CategoryMenuViewModel model, List<CategorySnapshot> allCats, List<CategorySnapshot> cats, List<CategorySnapshot> trail)
        {
            if (cats != null)
            {
                foreach (CategorySnapshot c in cats)
                {

                    if (c.Hidden == false)
                    {


                        CategoryMenuAddSingleLink(model, c, allCats);

                        if (CategoryMenuIsInTrail(c.Bvin, trail))
                        {
                            List<CategorySnapshot> children = Category.FindChildrenInList(allCats, c.Bvin, false);
                            if (children != null)
                            {
                                if (children.Count > 0)
                                {
                                    model.sb.Append("<ul>" + System.Environment.NewLine);
                                    CategoryMenuExpandInTrail(model, allCats, children, trail);
                                    model.sb.Append("</ul>" + System.Environment.NewLine);
                                }
                            }
                        }

                        model.sb.Append("</li>");
                    }
                }
            }
        }
        private void CategoryMenuBuildParentTrail(List<CategorySnapshot> allCats, string currentId, ref List<CategorySnapshot> trail)
        {
            if (currentId == "0" || currentId == string.Empty)
            {
                return;
            }

            CategorySnapshot current = Category.FindInList(allCats, currentId);

            if (current != null)
            {

                trail.Add(current);
                if (current.ParentId == "0")
                {
                    return;
                }
                if (current.ParentId != null)
                {
                    if (current.ParentId != string.Empty)
                    {
                        CategoryMenuBuildParentTrail(allCats, current.ParentId, ref trail);
                    }
                }
            }

        }
        private void CategoryMenuLoadPeersAndChildren(CategoryMenuViewModel model)
        {
            List<CategorySnapshot> allCats = MTApp.CatalogServices.Categories.FindAll();

            // Get Current Category
            CategorySnapshot currentCategory = Category.FindInList(allCats, model.CurrentId);

            // Trick system into accepting root category of zero which never exists in database
            if (model.CurrentId == "0")
            {
                currentCategory = new CategorySnapshot();
                currentCategory.Bvin = "0";
            }



            if (currentCategory != null)
            {
                if (currentCategory.Bvin != string.Empty)
                {
                    model.CurrentId = currentCategory.Bvin;
                }

                // Find the trail from this category back to the root of the site
                List<CategorySnapshot> trail = new List<CategorySnapshot>();
                CategoryMenuBuildParentTrail(allCats, model.CurrentId, ref trail);
                if (trail == null)
                {
                    trail = new List<CategorySnapshot>();
                }

                if (trail.Count < 1)
                {
                    // Load Roots Only
                    CategoryMenuLoadRoots(model);
                }
                else
                {

                    CategoryPeerSet neighbors = CategoryMenuGetPeerSet(allCats, currentCategory);

                    if (trail.Count == 1)
                    {
                        // special case where we want only peers and children
                        CategoryMenuRenderPeersChildren(model, neighbors, currentCategory, allCats);
                    }
                    else
                    {
                        if (trail.Count >= 3)
                        {
                            if (neighbors.Children.Count < 1)
                            {
                                // Special case where we are at the end of the tree and have
                                // no children. Reset neighbors to parent's bvin

                                CategorySnapshot parent = Category.FindInList(allCats, currentCategory.ParentId);
                                if (parent == null)
                                {
                                    parent = new CategorySnapshot();
                                }

                                neighbors = CategoryMenuGetPeerSet(allCats, parent);
                                CategoryMenuRenderParentsPeersChildren(model, neighbors, trail[1], allCats);
                            }
                            else
                            {
                                CategoryMenuRenderParentsPeersChildren(model, neighbors, currentCategory, allCats);
                            }
                        }
                        else
                        {
                            // normal load of peers
                            CategoryMenuRenderParentsPeersChildren(model, neighbors, currentCategory, allCats);
                        }
                    }
                }
            }

            else
            {
                model.sb.Append("Invalid Category Id. Contact Administrator");
            }

        }
        private CategoryPeerSet CategoryMenuGetPeerSet(List<CategorySnapshot> allCats, CategorySnapshot cat)
        {
            CategoryPeerSet result = new CategoryPeerSet();

            CategorySnapshot parent = Category.FindInList(allCats, cat.ParentId);
            if (parent != null)
            {
                result.Parents = Category.FindChildrenInList(allCats, parent.ParentId, false);
            }
            result.Peers = Category.FindChildrenInList(allCats, cat.ParentId, false);
            result.Children = Category.FindChildrenInList(allCats, cat.Bvin, false);

            return result;
        }
        private void CategoryMenuRenderPeersChildren(CategoryMenuViewModel model, CategoryPeerSet neighbors, CategorySnapshot currentCategory, List<CategorySnapshot> allCats)
        {
            // No Parents, start with peers
            foreach (CategorySnapshot peer in neighbors.Peers)
            {
                if (!peer.Hidden)
                {
                    CategoryMenuAddSingleLink(model, peer, allCats);
                    if (peer.Bvin == currentCategory.Bvin)
                    {

                        // Load Children
                        if (neighbors.Children.Count > 0)
                        {
                            bool initialized = false;
                            foreach (CategorySnapshot child in neighbors.Children)
                            {
                                if (!child.Hidden)
                                {
                                    if (!initialized)
                                    {
                                        model.sb.Append("<ul>" + System.Environment.NewLine);
                                        initialized = true;
                                    }

                                    CategoryMenuAddSingleLink(model, child, allCats);
                                    model.sb.Append("</li>" + System.Environment.NewLine);
                                }
                            }
                            if (initialized)
                            {
                                model.sb.Append("</ul>" + System.Environment.NewLine);
                            }
                        }

                    }
                    model.sb.Append("</li>" + System.Environment.NewLine);
                }
            }
        }
        private void CategoryMenuRenderParentsPeersChildren(CategoryMenuViewModel model, CategoryPeerSet neighbors, CategorySnapshot currentCategory, List<CategorySnapshot> allCats)
        {
            if (neighbors.Parents.Count < 1)
            {
                CategoryMenuRenderPeersChildren(model, neighbors, currentCategory, allCats);
            }
            else
            {

                // Add Parents
                foreach (CategorySnapshot parent in neighbors.Parents)
                {
                    if (!parent.Hidden)
                    {
                        CategoryMenuAddSingleLink(model, parent, allCats);

                        // Add Peers
                        if (parent.Bvin == currentCategory.ParentId)
                        {

                            bool peerInitialized = false;

                            foreach (CategorySnapshot peer in neighbors.Peers)
                            {
                                if (!peer.Hidden)
                                {
                                    if (!peerInitialized)
                                    {
                                        model.sb.Append("<ul>");
                                        peerInitialized = true;
                                    }
                                    CategoryMenuAddSingleLink(model, peer, allCats);
                                    if (peer.Bvin == currentCategory.Bvin)
                                    {

                                        // Load Children
                                        if (neighbors.Children.Count > 0)
                                        {
                                            bool childInitialized = false;
                                            foreach (CategorySnapshot child in neighbors.Children)
                                            {
                                                if (!child.Hidden)
                                                {
                                                    if (!childInitialized)
                                                    {
                                                        model.sb.Append("<ul>" + System.Environment.NewLine);
                                                        childInitialized = true;
                                                    }
                                                    CategoryMenuAddSingleLink(model, child, allCats);
                                                    model.sb.Append("</li>" + System.Environment.NewLine);
                                                }
                                            }
                                            if (childInitialized)
                                            {
                                                model.sb.Append("</ul>" + System.Environment.NewLine);
                                            }
                                        }


                                    }
                                    model.sb.Append("</li>" + System.Environment.NewLine);
                                }
                            }

                            if (peerInitialized)
                            {
                                model.sb.Append("</ul>" + System.Environment.NewLine);
                            }

                        }

                    }
                    model.sb.Append("</li>" + System.Environment.NewLine);
                }
            }
        }
        #endregion

        private ActionResult CategoryRotator(ContentBlock block)
        {
            return View("CategoryRotator", block);
        }
        private ActionResult FeaturedProducts(ContentBlock block)
        {
            return View("FeaturedProducts", block);
        }
        private ActionResult Html(ContentBlock block)
        {
            string result = string.Empty;            
            if (block != null)
            {
                result = block.BaseSettings.GetSettingOrEmpty("HtmlData");
            }

            result = MerchantTribe.Commerce.Utilities.TagReplacer.ReplaceContentTags(result,
                                                                                    MTApp,
                                                                                    "",
                                                                                    Request.IsSecureConnection);            
            return Content(result);
        }
        private ActionResult LastProductsViewed(ContentBlock block)
        {
            return View("LastProductsViewed", block);
        }
        private ActionResult ProductGrid(ContentBlock block)
        {
            return View("ProductGrid", block);
        }
        private ActionResult ProductRotator(ContentBlock block)
        {
            return View("ProductRotator", block);
        }
        private ActionResult RssFeedViewer(ContentBlock block)
        {
            return View("RssFeedViewer", block);
        }
        private ActionResult SideMenu(ContentBlock block)
        {
            return View("SideMenu", block);
        }
        private ActionResult Top10Products(ContentBlock block)
        {
            return View("Top10Products", block);
        }
        private ActionResult TopWeeklySellers(ContentBlock block)
        {
            return View("TopWeeklySellers", block);
        }

    }
}
