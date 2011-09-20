using System.Collections.ObjectModel;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Utilities;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class category : BaseStoreCategoryPage
    {
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.BlockId = "D2D63F6A-2480-42a1-A593-FCFA83A2C8B8";
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            this.AddBodyClass("store-category-page");

            if (LocalCategory.PreContentColumnId != string.Empty)
            {
                this.PreContentColumn.ColumnID = LocalCategory.PreContentColumnId;
                this.PreContentColumn.LoadColumn();
            }
            if (LocalCategory.PostContentColumnId != string.Empty)
            {
                this.PostContentColumn.ColumnID = LocalCategory.PostContentColumnId;
                this.PostContentColumn.LoadColumn();
            }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (LocalCategory != null)
            {
                int itemsPerPage = 9;

                Pager1.ItemsPerPage = itemsPerPage;
                Pager2.ItemsPerPage = itemsPerPage;
                PopulateCategoryInfo();

                int rowCount = 0;
                
                List<Product> displayProducts = MTApp.CatalogServices.FindProductForCategoryWithSort(LocalCategory.Bvin,
                                                                                                this.SortOrder, 
                                                                                                false,
                                                                                                Pager1.CurrentPage, 
                                                                                                Pager1.ItemsPerPage, 
                                                                                                ref rowCount);                
                RenderProducts(displayProducts);
                this.lblResults.Text = "Found " + rowCount + " items";
                Pager1.RowCount = rowCount;
                Pager2.RowCount = rowCount;

                LoadSubCategories();

                RecordCategoryView();
            }
        }

        private void RecordCategoryView()
        {
            SessionManager.CategoryLastId = LocalCategory.Bvin;
        }

        private void LoadSubCategories()
        {
            this.DataList2.DataSource = _CategoryRepository.FindVisibleChildren(LocalCategory.Bvin);
            this.DataList2.DataBind();
        }

        public void PopulateCategoryInfo()
        {

            // Page Title
            if (LocalCategory.MetaTitle.Trim().Length > 0)
            {
                this.Title = LocalCategory.MetaTitle;
            }
            else
            {
                this.Title = LocalCategory.Name;
            }

            // Meta Keywords
            if (LocalCategory.MetaKeywords.Trim().Length > 0)
            {
                Page.MetaKeywords = LocalCategory.MetaKeywords;
            }

            // Meta Description
            if (LocalCategory.MetaDescription.Trim().Length > 0)
            {
                Page.MetaDescription = LocalCategory.MetaDescription;
            }

            // Title
            if (LocalCategory.ShowTitle == false)
            {
                this.lblTitle.Visible = false;
            }
            else
            {
                this.lblTitle.Visible = true;
                this.lblTitle.Text = "<h1>" + LocalCategory.Name + "</h1>";
            }

            //Description
            if (LocalCategory.Description.Trim().Length > 0)
            {
                this.DescriptionLiteral.Text = LocalCategory.Description;
            }
            else
            {
                this.DescriptionLiteral.Text = string.Empty;
            }

            if (LocalCategory.BannerImageUrl.Trim().Length > 0)
            {
                this.BannerImage.Visible = true;
                this.BannerImage.ImageUrl = MerchantTribe.Commerce.Storage.DiskStorage.CategoryBannerUrl(MTApp.CurrentStore.Id, LocalCategory.Bvin, LocalCategory.BannerImageUrl, Request.IsSecureConnection);
                this.BannerImage.AlternateText = LocalCategory.Name;
            }
            else
            {
                this.BannerImage.Visible = false;
            }

        }

        private void RenderProducts(List<Product> displayProducts)
        {

            StringBuilder sb = new StringBuilder();

            int columnCount = 1;

            foreach (Product p in displayProducts)
            {

                bool isLastInRow = false;
                bool isFirstInRow = false;
                if ((columnCount == 1))
                {
                    isFirstInRow = true;
                }

                if ((columnCount == 3))
                {
                    isLastInRow = true;
                    columnCount = 1;
                }
                else
                {
                    columnCount += 1;
                }
                UserSpecificPrice price = MTApp.PriceProduct(p, MTApp.CurrentCustomer, null);                
                MerchantTribe.Commerce.Utilities.HtmlRendering.RenderSingleProduct(ref sb, p, isLastInRow, isFirstInRow, this.Page, price);
            }

            this.categoryitems.Text = sb.ToString();
        }

        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
            if (Pager1.Visible)
            {
                Pager2.Visible = true;
            }
        }

        protected void DataList2_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item | e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CategorySnapshot c = (CategorySnapshot)e.Item.DataItem;
                if (c != null)
                {

                    Literal litRecord = (Literal)e.Item.FindControl("litRecord");
                    if (litRecord != null)
                    {

                        string destinationLink = UrlRewriter.BuildUrlForCategory(c, MTApp.CurrentRequestContext.RoutingContext);
                        string imageUrl = MerchantTribe.Commerce.Storage.DiskStorage.CategoryIconUrl(MTApp.CurrentStore.Id, c.Bvin, c.ImageUrl, Request.IsSecureConnection);

                        //Image
                        HtmlAnchor imageAnchor = (HtmlAnchor)e.Item.FindControl("recordimageanchor");
                        imageAnchor.HRef = destinationLink;
                        HtmlImage image = (HtmlImage)e.Item.FindControl("recordimageimg");
                        if ((c.ImageUrl.Length > 0))
                        {
                            image.Src = imageUrl;
                            image.Alt = c.Name;
                        }

                        // Force Image Size
                        //ViewUtilities.ForceImageSize(image, c.ImageUrl, ViewUtilities.Sizes.Small, Me.Page)

                        //Name
                        HtmlAnchor nameAnchor = (HtmlAnchor)e.Item.FindControl("recordnameanchor");
                        nameAnchor.HRef = destinationLink;
                        nameAnchor.InnerText = c.Name;
                    }

                    else
                    {
                        litRecord.Text = "Product could not be located";
                    }
                }
            }
        }
    }
}