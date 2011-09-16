
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce.Utilities;

namespace BVCommerce
{

    partial class BVModules_ContentBlocks_Category_Menu_view : BVModule
    {

        private bool _showProductCounts = false;
        private bool _showCategoryCounts = false;
        
        protected string currentId = "0";

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            LoadMenu();
        }

        private string LocateCurrentCategory()
        {
            string result = string.Empty;


            if (Request.QueryString["categoryId"] != null)
            {
                result = Request.QueryString["categoryId"];
            }
            else
            {
                if (this.Page is IProductPage)
                {
                    Product p = ((IProductPage)this.Page).LocalProduct;
                    if (p != null)
                    {
                        CatalogService CatalogServices = MyPage.BVApp.CatalogServices;
                        List<CategorySnapshot> categories = CatalogServices.FindCategoriesForProduct(p.Bvin);
                        if (categories != null)
                        {
                            if (categories.Count > 0)
                            {

                                bool found = false;

                                // scan category list to see if the last visited is in the collection
                                foreach (CategorySnapshot c in categories)
                                {
                                    if (c.Bvin == SessionManager.CategoryLastId)
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
                    result = SessionManager.CategoryLastId;
                }
            }

            // Always reset to zero if we can't find anything
            if (result == string.Empty)
            {
                result = "0";
            }

            return result;
        }

        private void LoadMenu()
        {
            currentId = LocateCurrentCategory();
            ContentBlock b = MyPage.BVApp.ContentServices.Columns.FindBlock(this.BlockId);

            if (b != null)
            {
                this.TitlePlaceHolder.Controls.Clear();
                string title = b.BaseSettings.GetSettingOrEmpty("Title");
                if (title.Trim().Length > 0)
                {
                    this.TitlePlaceHolder.Controls.Add(new LiteralControl("<h4>" + title + "</h4>"));
                }

                this.MenuControl.Controls.Clear();
                MenuControl.EnableViewState = false;

                _showProductCounts = b.BaseSettings.GetBoolSetting("ShowProductCount");
                _showCategoryCounts = b.BaseSettings.GetBoolSetting("ShowCategoryCount");
                this.MenuControl.Controls.Add(new LiteralControl("<ul>"));

                if (b.BaseSettings.GetBoolSetting("HomeLink") == true)
                {
                    AddHomeLink();
                }

                string mode = b.BaseSettings.GetSettingOrEmpty("CategoryMenuMode");
                switch (mode)
                {
                    case "0":
                        // Root Categories Only
                        LoadRoots();
                        break;
                    case "1":
                        // All Categories
                        LoadAllCategories(b.BaseSettings.GetIntegerSetting("MaximumDepth"));
                        break;
                    case "2":
                        // Peers, Children and Parents
                        LoadPeersAndChildren();
                        break;
                    case "3":
                        // Show root and expanded children
                        LoadRootPlusExpandedChildren();
                        break;
                    default:
                        // All Categories
                        LoadRoots();
                        break;
                }

                this.MenuControl.Controls.Add(new LiteralControl("</ul>"));
            }
        }

        private void LoadRoots()
        {
            List<CategorySnapshot> cats = MyPage.BVApp.CatalogServices.Categories.FindVisibleChildren("0");
            AddCategoryCollection(cats, cats, 1, 1);
        }

        private void AddHomeLink()
        {
            this.MenuControl.Controls.Add(new LiteralControl("<li>"));
            HyperLink m = new HyperLink();
            m.ToolTip = "Home";
            m.Text = "Home";
            m.NavigateUrl = "~/default.aspx";
            m.EnableViewState = false;
            this.MenuControl.Controls.Add(m);
            this.MenuControl.Controls.Add(new LiteralControl("</li>"));
        }

        private void AddSingleLink(CategorySnapshot c, List<CategorySnapshot> allCats)
        {
            AddSingleLink(c, currentId, allCats);
        }

        private void AddSingleLink(CategorySnapshot c, string currentCategoryId, List<CategorySnapshot> allCats)
        {
            if (c.Bvin == currentCategoryId)
            {
                if (this.Page is BaseStoreCategoryPage)
                {
                    this.MenuControl.Controls.Add(new LiteralControl("<li class=\"current\">"));
                }
                else
                {
                    this.MenuControl.Controls.Add(new LiteralControl("<li class=\"current\">"));
                }
            }
            else
            {
                this.MenuControl.Controls.Add(new LiteralControl("<li>"));
            }

            HyperLink m = new HyperLink();
            m.ToolTip = c.MetaTitle;
            m.Text = c.Name;

            int childCount = 0;
            if (this._showProductCounts)
            {
                childCount += (MyPage.BVApp.CatalogServices.CategoriesXProducts.FindForCategory(c.Bvin, 1, int.MaxValue).Count);
            }

            if (this._showCategoryCounts)
            {
                childCount += Category.FindChildrenInList(allCats, c.Bvin, false).Count;
            }

            if (childCount > 0)
            {
                m.Text = m.Text + " (" + childCount.ToString() + ")";
            }

            m.NavigateUrl = UrlRewriter.BuildUrlForCategory(c, MyPage.BVApp.CurrentRequestContext.RoutingContext);
            if (c.SourceType == CategorySourceType.CustomLink)
            {
                if (c.CustomPageOpenInNewWindow == true)
                {
                    m.Target = "_blank";
                }
            }

            m.EnableViewState = false;
            this.MenuControl.Controls.Add(m);
        }

        private void LoadRootPlusExpandedChildren()
        {
            List<CategorySnapshot> allCats = MyPage.BVApp.CatalogServices.Categories.FindAll();

            // Get Current Category
            CategorySnapshot currentCategory = Category.FindInList(allCats, currentId);

            if (currentCategory != null)
            {
                if (currentCategory.Bvin != string.Empty)
                {
                    currentId = currentCategory.Bvin;
                }

                // Find the trail from this category back to the root of the site
                List<CategorySnapshot> trail = new List<CategorySnapshot>();
                BuildParentTrail(allCats, currentId, ref trail);                
                if (trail == null)
                {
                    trail = new List<CategorySnapshot>();
                }

                if (trail.Count < 1)
                {
                    // Load Roots Only
                    LoadRoots();
                }
                else
                {

                    string StartingRootCategoryId = currentCategory.Bvin;
                    StartingRootCategoryId = trail[trail.Count - 1].Bvin;


                    List<CategorySnapshot> roots = Category.FindChildrenInList(allCats, "0", false);
                    if (roots != null)
                    {
                        this.MenuControl.Controls.Add(new LiteralControl("<ul>" + System.Environment.NewLine));

                        foreach (CategorySnapshot c in roots)
                        {
                            if (IsInTrail(c.Bvin, trail))
                            {
                                AddSingleLink(c, currentId, allCats);
                                List<CategorySnapshot> children = new List<CategorySnapshot>();
                                children = Category.FindChildrenInList(allCats, StartingRootCategoryId, false);
                                if (children != null)
                                {
                                    this.MenuControl.Controls.Add(new LiteralControl("<ul>" + System.Environment.NewLine));
                                    ExpandInTrail(allCats, children, trail);
                                    this.MenuControl.Controls.Add(new LiteralControl("</ul>" + System.Environment.NewLine));
                                }
                                this.MenuControl.Controls.Add(new LiteralControl("</li>"));

                                break;
                            }
                        }

                        this.MenuControl.Controls.Add(new LiteralControl("</ul>" + System.Environment.NewLine));
                    }

                }
            }
            else
            {
                this.MenuControl.Controls.Add(new LiteralControl("Invalid Category Id. Contact Administrator"));
            }

        }

        private bool IsInTrail(string testBvin, List<CategorySnapshot> trail)
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

        private void ExpandInTrail(List<CategorySnapshot> allCats, List<CategorySnapshot> cats, List<CategorySnapshot> trail)
        {
            if (cats != null)
            {
                foreach (CategorySnapshot c in cats)
                {

                    if (c.Hidden == false)
                    {


                        AddSingleLink(c, allCats);

                        if (IsInTrail(c.Bvin, trail))
                        {
                            List<CategorySnapshot> children = Category.FindChildrenInList(allCats, c.Bvin, false);
                            if (children != null)
                            {
                                if (children.Count > 0)
                                {
                                    this.MenuControl.Controls.Add(new LiteralControl("<ul>" + System.Environment.NewLine));
                                    ExpandInTrail(allCats, children, trail);
                                    this.MenuControl.Controls.Add(new LiteralControl("</ul>" + System.Environment.NewLine));
                                }
                            }
                        }

                        this.MenuControl.Controls.Add(new LiteralControl("</li>"));
                    }
                }
            }
        }

        private void LoadAllCategories(int maxDepth)
        {
            List<CategorySnapshot> allCats = MyPage.BVApp.CatalogServices.Categories.FindAll();                    
            List<CategorySnapshot> children = Category.FindChildrenInList(allCats, "0", false);
            AddCategoryCollection(allCats, children, 0, maxDepth);
        }

        private void AddCategoryCollection(List<CategorySnapshot> allCats, List<CategorySnapshot> cats, int currentDepth, int maxDepth)
        {
            if (cats != null)
            {
                foreach (CategorySnapshot c in cats)
                {

                    if (c.Hidden == false)
                    {

                        AddSingleLink(c, allCats);

                        if ((maxDepth == 0) | (currentDepth + 1 < maxDepth))
                        {
                            List<CategorySnapshot> children = MyPage.BVApp.CatalogServices.Categories.FindVisibleChildren(c.Bvin);
                            if (children != null)
                            {
                                if (children.Count > 0)
                                {
                                    this.MenuControl.Controls.Add(new LiteralControl("<ul>" + System.Environment.NewLine));
                                    AddCategoryCollection(allCats, children, currentDepth + 1, maxDepth);
                                    this.MenuControl.Controls.Add(new LiteralControl("</ul>" + System.Environment.NewLine));
                                }
                            }
                        }

                        this.MenuControl.Controls.Add(new LiteralControl("</li>"));
                    }
                }
            }
        }

        private void BuildParentTrail(List<CategorySnapshot> allCats,string currentId,ref List<CategorySnapshot> trail)
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
                        BuildParentTrail(allCats, current.ParentId, ref trail);
                    }
                }
            }

        }

        private CategoryPeerSet GetPeerSet(List<CategorySnapshot> allCats, CategorySnapshot cat)
         {
            CategoryPeerSet result = new CategoryPeerSet();

             CategorySnapshot parent = Category.FindInList(allCats, cat.ParentId);
            if (parent != null)
            {
                result.Parents = Category.FindChildrenInList(allCats,parent.ParentId, false);
            }
            result.Peers = Category.FindChildrenInList(allCats, cat.ParentId, false);
            result.Children = Category.FindChildrenInList(allCats, cat.Bvin, false);

            return result;
         }

        private void LoadPeersAndChildren()
        {
            List<CategorySnapshot> allCats = MyPage.BVApp.CatalogServices.Categories.FindAll();

            // Get Current Category
            CategorySnapshot currentCategory = Category.FindInList(allCats, currentId);

            // Trick system into accepting root category of zero which never exists in database
            if (currentId == "0")
            {
                currentCategory = new CategorySnapshot();
                currentCategory.Bvin = "0";
            }



            if (currentCategory != null)
            {
                if (currentCategory.Bvin != string.Empty)
                {
                    currentId = currentCategory.Bvin;
                }

                // Find the trail from this category back to the root of the site
                List<CategorySnapshot> trail = new List<CategorySnapshot>();
                BuildParentTrail(allCats, currentId,ref trail);
                if (trail == null)
                {
                    trail = new List<CategorySnapshot>();
                }

                if (trail.Count < 1)
                {
                    // Load Roots Only
                    LoadRoots();
                }
                else
                {
                                      
                    CategoryPeerSet neighbors = GetPeerSet(allCats, currentCategory);

                    if (trail.Count == 1)
                    {
                        // special case where we want only peers and children
                        RenderPeersChildren(neighbors, currentCategory, allCats);
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
                                
                                neighbors = GetPeerSet(allCats, parent);
                                RenderParentsPeersChildren(neighbors, trail[1], allCats);
                            }
                            else
                            {
                                RenderParentsPeersChildren(neighbors, currentCategory, allCats);
                            }
                        }
                        else
                        {
                            // normal load of peers
                            RenderParentsPeersChildren(neighbors, currentCategory, allCats);
                        }
                    }
                }
            }

            else
            {
                this.MenuControl.Controls.Add(new LiteralControl("Invalid Category Id. Contact Administrator"));
            }

        }

        private void RenderPeersChildren(CategoryPeerSet neighbors, CategorySnapshot currentCategory, List<CategorySnapshot> allCats)
        {
            // No Parents, start with peers
            foreach (CategorySnapshot peer in neighbors.Peers)
            {
                if (!peer.Hidden)
                {
                    AddSingleLink(peer, allCats);
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
                                        this.MenuControl.Controls.Add(new LiteralControl("<ul>" + System.Environment.NewLine));
                                        initialized = true;
                                    }

                                    AddSingleLink(child, allCats);
                                    this.MenuControl.Controls.Add(new LiteralControl("</li>" + System.Environment.NewLine));
                                }
                            }
                            if (initialized)
                            {
                                this.MenuControl.Controls.Add(new LiteralControl("</ul>" + System.Environment.NewLine));
                            }
                        }

                    }
                    this.MenuControl.Controls.Add(new LiteralControl("</li>" + System.Environment.NewLine));
                }
            }
        }

        private void RenderParentsPeersChildren(CategoryPeerSet neighbors, CategorySnapshot currentCategory, List<CategorySnapshot> allCats)
        {
            if (neighbors.Parents.Count < 1)
            {
                RenderPeersChildren(neighbors, currentCategory, allCats);
            }
            else
            {

                // Add Parents
                foreach (CategorySnapshot parent in neighbors.Parents)
                {
                    if (!parent.Hidden)
                    {
                        AddSingleLink(parent, allCats);

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
                                        this.MenuControl.Controls.Add(new LiteralControl("<ul>"));
                                        peerInitialized = true;
                                    }
                                    AddSingleLink(peer, allCats);
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
                                                        this.MenuControl.Controls.Add(new LiteralControl("<ul>" + System.Environment.NewLine));
                                                        childInitialized = true;
                                                    }
                                                    AddSingleLink(child, allCats);
                                                    this.MenuControl.Controls.Add(new LiteralControl("</li>" + System.Environment.NewLine));
                                                }
                                            }
                                            if (childInitialized)
                                            {
                                                this.MenuControl.Controls.Add(new LiteralControl("</ul>" + System.Environment.NewLine));
                                            }
                                        }


                                    }
                                    this.MenuControl.Controls.Add(new LiteralControl("</li>" + System.Environment.NewLine));
                                }
                            }

                            if (peerInitialized)
                            {
                                this.MenuControl.Controls.Add(new LiteralControl("</ul>" + System.Environment.NewLine));
                            }

                        }

                    }

                    this.MenuControl.Controls.Add(new LiteralControl("</li>" + System.Environment.NewLine));

                }

            }
        }

    }

}