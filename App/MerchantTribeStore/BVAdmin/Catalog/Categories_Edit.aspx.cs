using System;
using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce.Membership;
using BVSoftware.Commerce.Utilities;
using System.Collections.Generic;

namespace BVCommerce
{

    partial class BVAdmin_Catalog_Categories_Edit : BaseAdminPage
    {

        public string IconImage = "";
        public string BannerImage = "";

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                //PopulateTemplates()
                PopulateColumns();

                this.NameField.Focus();

                if (Request.QueryString["id"] != null)
                {
                    this.BvinField.Value = Request.QueryString["id"];
                    if (Request.QueryString["type"] != null)
                    {
                        ViewState["type"] = Request.QueryString["type"];
                    }
                    Category category = LoadCategory();
                    if (category == null)
                    {
                        BVSoftware.Commerce.EventLog.LogEvent("Edit Category Page", 
                                                              "Could not find category with bvin " + this.BvinField.Value, 
                                                              BVSoftware.Commerce.Metrics.EventLogSeverity.Error);
                        Response.Redirect("categories.aspx");
                    }

                    if (ViewState["type"] == null)
                    {
                        ViewState["type"] = category.SourceType;
                    }

                    if (category != null)
                    {
                        if (category.SourceType == CategorySourceType.CustomPage)
                        {
                            Response.Redirect("Categories_EditCustom.aspx?id=" + category.Bvin);
                        }
                    }

                    CategoryBreadCrumbTrail1.LoadTrail(Request.QueryString["id"]);
                    this.UrlsAssociated1.ObjectId = category.Bvin;
                    this.UrlsAssociated1.LoadUrls();
                    PopulateStoreLink(category);
                }
                else
                {
                    
                    this.BvinField.Value = string.Empty;
                    if (Request.QueryString["ParentID"] != null)
                    {
                        CategoryBreadCrumbTrail1.LoadTrail(Request.QueryString["ParentID"]);
                        if (Request.QueryString["type"] != null)
                        {
                            ViewState["type"] = Request.QueryString["type"];
                        }
                        this.ParentIDField.Value = (string)Request.QueryString["ParentID"];
                    }
                    else
                    {
                        Response.Redirect("~/BVAdmin/Catalog/Categories.aspx");
                    }
                }
            }
            //Me.TemplatePreviewImage.ImageUrl = Content.ModuleController.FindCategoryTemplatePreviewImage(Me.TemplateList.SelectedValue, Request.PhysicalApplicationPath)
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Category";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        //Private Sub PopulateTemplates()
        //Me.TemplateList.DataSource = Content.ModuleController.FindCategoryTemplates
        //Me.TemplateList.DataBind()
        //End Sub

        private void PopulateColumns()
        {
            List<ContentColumn> columns = BVApp.ContentServices.Columns.FindAll();
            foreach (ContentColumn col in columns)
            {
                this.PreContentColumnIdField.Items.Add(new System.Web.UI.WebControls.ListItem(col.DisplayName, col.Bvin));
                this.PostContentColumnIdField.Items.Add(new System.Web.UI.WebControls.ListItem(col.DisplayName, col.Bvin));
            }
        }

        private Category LoadCategory()
        {
            Category c = BVApp.CatalogServices.Categories.Find(this.BvinField.Value);
            if (c != null)
            {

                if (c.Bvin != string.Empty)
                {
                    this.NameField.Text = c.Name;
                    this.DescriptionField.Text = c.Description;
                    if (this.DescriptionField.SupportsTransform == true)
                    {
                        if (c.PreTransformDescription.Trim().Length > 0)
                        {
                            this.DescriptionField.Text = c.PreTransformDescription;
                        }
                    }
                    this.MetaDescriptionField.Text = c.MetaDescription;
                    this.MetaKeywordsField.Text = c.MetaKeywords;
                    this.MetaTitleField.Text = c.MetaTitle;
                    this.chkHidden.Checked = c.Hidden;
                    //Me.chkShowInTopMenu.Checked = c.ShowInTopMenu

                    //If TemplateList.Items.FindByValue(c.TemplateName) IsNot Nothing Then
                    //Me.TemplateList.ClearSelection()
                    //Me.TemplateList.Items.FindByValue(c.TemplateName).Selected = True
                    //End If
                    if (c.PreContentColumnId.Trim() != string.Empty)
                    {
                        if (this.PreContentColumnIdField.Items.FindByValue(c.PreContentColumnId) != null)
                        {
                            this.PreContentColumnIdField.Items.FindByValue(c.PreContentColumnId).Selected = true;
                        }
                    }
                    if (c.PostContentColumnId.Trim() != string.Empty)
                    {
                        if (this.PostContentColumnIdField.Items.FindByValue(c.PostContentColumnId) != null)
                        {
                            this.PostContentColumnIdField.Items.FindByValue(c.PostContentColumnId).Selected = true;
                        }
                    }

                    if (Enum.IsDefined(typeof(CategorySortOrder), c.DisplaySortOrder) && c.DisplaySortOrder != CategorySortOrder.None)
                    {
                        this.SortOrderDropDownList.SelectedValue = ((int)c.DisplaySortOrder).ToString();
                    }
                    else
                    {
                        this.SortOrderDropDownList.SelectedValue = ((int)CategorySortOrder.ManualOrder).ToString();
                    }

                    this.RewriteUrlField.Text = c.RewriteUrl;
                    this.chkShowTitle.Checked = c.ShowTitle;
                    this.keywords.Text = c.Keywords.ToString();

                    //Me.CustomerOverridableSortOrderCheckBox.Checked = c.CustomerChangeableSortOrder

                    this.ParentIDField.Value = c.ParentId;

                    UpdateBannerImage(c);
                    UpdateIconImage(c);
                }
            }
            return c;
        }

        private void PopulateStoreLink(Category c)
        {

            HyperLink m = new HyperLink();
            m.ImageUrl = "~/BVAdmin/Images/Buttons/ViewInStore.png";
            m.ToolTip = c.MetaTitle;
            m.NavigateUrl = UrlRewriter.BuildUrlForCategory(new CategorySnapshot(c), BVApp.CurrentRequestContext.RoutingContext);
            m.EnableViewState = false;
            this.inStore.Controls.Add(m);

        }
        private void UpdateIconImage(Category c)
        {
            IconImage = BVSoftware.Commerce.Storage.DiskStorage.CategoryIconUrl(BVApp.CurrentStore.Id, c.Bvin, c.ImageUrl, true);
            if (IconImage == string.Empty || c.ImageUrl == string.Empty)
            {
                IconImage = Page.ResolveUrl("~/content/admin/images/MissingImage.png");
            }
        }
        private void UpdateBannerImage(Category c)
        {
            BannerImage = BVSoftware.Commerce.Storage.DiskStorage.CategoryBannerUrl(BVApp.CurrentStore.Id, c.Bvin, c.BannerImageUrl, true);
            if (BannerImage == string.Empty || c.BannerImageUrl == string.Empty)
            {
                BannerImage = Page.ResolveUrl("~/content/admin/images/MissingImage.png");
            }
        }


        private bool Save()
        {
            Category c = BVApp.CatalogServices.Categories.Find(this.BvinField.Value);
            if (c == null)
            {
                c = new Category();
            }
            return Save(c);
        }
        private bool Save(Category c)
        {
            bool result = false;

            if (c != null)
            {
                c.Name = this.NameField.Text.Trim();
                c.Description = this.DescriptionField.Text.Trim();
                c.PreTransformDescription = this.DescriptionField.PreTransformText;
                c.MetaDescription = this.MetaDescriptionField.Text.Trim();
                c.MetaTitle = this.MetaTitleField.Text.Trim();
                c.MetaKeywords = this.MetaKeywordsField.Text.Trim();
                c.ShowInTopMenu = false;
                // Me.chkShowInTopMenu.Checked
                c.Hidden = this.chkHidden.Checked;

                // Icon Image Upload
                if ((this.iconupload.HasFile))
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(iconupload.FileName);
                    string ext = System.IO.Path.GetExtension(iconupload.FileName);

                    if (BVSoftware.Commerce.Storage.DiskStorage.ValidateImageType(ext))
                    {
                        fileName = MerchantTribe.Web.Text.CleanFileName(fileName);
                        if ((BVSoftware.Commerce.Storage.DiskStorage.UploadCategoryIcon(BVApp.CurrentStore.Id, c.Bvin, this.iconupload.PostedFile)))
                        {
                            c.ImageUrl = fileName + ext;
                        }
                    }
                    else
                    {
                        result = false;
                        this.MessageBox1.ShowError("Only .PNG, .JPG, .GIF file types are allowed for icon images");
                    }
                }

                // Banner Image Upload
                if ((this.bannerupload.HasFile))
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(bannerupload.FileName);
                    string ext = System.IO.Path.GetExtension(bannerupload.FileName);

                    if (BVSoftware.Commerce.Storage.DiskStorage.ValidateImageType(ext))
                    {
                        fileName = MerchantTribe.Web.Text.CleanFileName(fileName);
                        if ((BVSoftware.Commerce.Storage.DiskStorage.UploadCategoryBanner(BVApp.CurrentStore.Id, c.Bvin, this.bannerupload.PostedFile)))
                        {
                            c.BannerImageUrl = fileName + ext;
                        }
                    }
                    else
                    {
                        result = false;
                        this.MessageBox1.ShowError("Only .PNG, .JPG, .GIF file types are allowed for icon images");
                    }
                }

                //if ((CategorySourceType)ViewState["type"] == CategorySourceType.ByRules) {
                //    c.SourceType = CategorySourceType.ByRules;
                //}
                //else if ((CategorySourceType)ViewState["type"] == CategorySourceType.CustomLink) {
                //    c.SourceType = CategorySourceType.CustomLink;
                //}
                //else if ((CategorySourceType)ViewState["type"] == CategorySourceType.Manual) {
                c.SourceType = CategorySourceType.Manual;
                //}
                //else if ((CategorySourceType)ViewState["type"] == CategorySourceType.CustomPage) {
                //    c.SourceType = CategorySourceType.CustomPage;
                //}
                //If Me.TemplateList.SelectedValue IsNot Nothing Then
                //c.TemplateName = Me.TemplateList.SelectedValue
                //Else
                //c.TemplateName = String.Empty
                //End If
                c.PreContentColumnId = this.PreContentColumnIdField.SelectedValue;
                c.PostContentColumnId = this.PostContentColumnIdField.SelectedValue;
                c.DisplaySortOrder = (CategorySortOrder)int.Parse(this.SortOrderDropDownList.SelectedValue);

                string oldUrl = c.RewriteUrl;

                // no entry, generate one
                if (c.RewriteUrl.Trim().Length < 1)
                {
                    c.RewriteUrl = MerchantTribe.Web.Text.Slugify(c.Name, true, true);
                }
                else
                {
                    c.RewriteUrl = MerchantTribe.Web.Text.Slugify(this.RewriteUrlField.Text, true, true);
                }
                this.RewriteUrlField.Text = c.RewriteUrl;

                if (UrlRewriter.IsCategorySlugInUse(c.RewriteUrl, c.Bvin, BVApp.CurrentRequestContext))
                {
                    this.MessageBox1.ShowWarning("The requested URL is already in use by another item.");
                    return false;
                }
               
                c.ShowTitle = this.chkShowTitle.Checked;
                c.Keywords = this.keywords.Text.Trim();

                c.CustomerChangeableSortOrder = true;
                // Me.CustomerOverridableSortOrderCheckBox.Checked

                if (this.BvinField.Value == string.Empty)
                {
                    c.ParentId = this.ParentIDField.Value;
                    result = BVApp.CatalogServices.Categories.Create(c);
                    if (result)
                    {
                        result = BVApp.CatalogServices.Categories.SubmitChanges();
                    }
                }
                else
                {
                    result = BVApp.CatalogServices.Categories.Update(c);
                    if (result)
                    {
                        result = BVApp.CatalogServices.Categories.SubmitChanges();
                    }
                }

                if (result == false)
                {
                    this.lblError.Text = "Unable to save category. Uknown error.";
                }
                else
                {
                    // Update bvin field so that next save will call updated instead of create
                    this.BvinField.Value = c.Bvin;

                    if (oldUrl != string.Empty)
                    {
                        if (oldUrl != c.RewriteUrl)
                        {
                            BVApp.ContentServices.CustomUrls.Register301(oldUrl,c.RewriteUrl,
                                                  c.Bvin, CustomUrlType.Category, BVApp.CurrentRequestContext, BVApp);
                            this.UrlsAssociated1.LoadUrls();
                        }
                    }
                }

            }

            return result;
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Categories.aspx?id=" + this.ParentIDField.Value);
        }

        protected void btnSaveChanges_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.lblError.Text = string.Empty;

            if (Save() == true)
            {
                Response.Redirect("Categories.aspx?id=" + this.ParentIDField.Value);
            }
        }

        //Protected Sub TemplateList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TemplateList.SelectedIndexChanged
        //Me.TemplatePreviewImage.ImageUrl = Content.ModuleController.FindCategoryTemplatePreviewImage(Me.TemplateList.SelectedValue, Request.PhysicalApplicationPath)
        //End Sub

        protected void btnSelectProducts_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.Save() == true)
            {
                Response.Redirect("Categories_ManualSelection.aspx?id=" + this.BvinField.Value + "&type=" + ViewState["type"]);
            }
        }

        protected void UpdateButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.lblError.Text = string.Empty;
            if (this.Save())
            {
                MessageBox1.ShowOk("Category Updated Successfully.");
                Category cat = BVApp.CatalogServices.Categories.Find(this.BvinField.Value);
                if (cat != null && cat.Bvin != string.Empty)
                {
                    PopulateStoreLink(cat);
                    UpdateBannerImage(cat);
                    UpdateIconImage(cat);
                }
            }
            else
            {
                MessageBox1.ShowError("Error during update. Please check event log.");
            }
        }

        protected void delIcon_Click(object sender, ImageClickEventArgs e)
        {
            Category c = BVApp.CatalogServices.Categories.Find(this.BvinField.Value);
            if (c != null)
            {
                c.ImageUrl = string.Empty;
                Save(c);
                LoadCategory();            
            }
        }

        protected void delBanner_Click(object sender, ImageClickEventArgs e)
        {
            Category c = BVApp.CatalogServices.Categories.Find(this.BvinField.Value);
            if (c != null)
            {
                c.BannerImageUrl = string.Empty;
                Save(c);
                LoadCategory();
            }
        }
    }
}