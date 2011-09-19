using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;

namespace BVCommerce
{

    partial class ProductReviews : BaseStorePage
    {

        private Product _LocalProduct = null;

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            string slug = (string)Page.RouteData.Values["slug"];

            if (slug != string.Empty)
            {
                _LocalProduct = BVApp.CatalogServices.Products.FindBySlug(slug);
                if (_LocalProduct == null)
                {
                    UrlRewriter.RedirectToErrorPage(MerchantTribe.Commerce.ErrorTypes.Product, Response);
                }
                else if (_LocalProduct.Status == ProductStatus.Disabled)
                {
                    UrlRewriter.RedirectToErrorPage(MerchantTribe.Commerce.ErrorTypes.Product, Response);
                }
            }
            else
            {
                UrlRewriter.RedirectToErrorPage(MerchantTribe.Commerce.ErrorTypes.Product, Response);
            }

            if (_LocalProduct.Bvin == string.Empty)
            {
                EventLog.LogEvent("Product Page", "Requested Product slug " + slug + " was not found", MerchantTribe.Commerce.Metrics.EventLogSeverity.Error);
            }

            // Page Title
            if (_LocalProduct.MetaTitle.Trim().Length > 0)
            {
                this.Page.Title = "Product Reviews: " + _LocalProduct.MetaTitle;
            }
            else
            {
                this.Page.Title = "Product Reviews: " + _LocalProduct.ProductName;
            }

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RenderProductMini();
            LoadReviews();
        }

        private void RenderProductMini()
        {
            StringBuilder sb = new StringBuilder();
            UserSpecificPrice price = BVApp.PriceProduct(_LocalProduct, BVApp.CurrentCustomer, null);
            HtmlRendering.RenderSingleProduct(ref sb, _LocalProduct, false, false, this.Page, price);
            this.litProduct.Text = sb.ToString();
            this.litProductName.Text = _LocalProduct.ProductName;
            this.MetaDescription = _LocalProduct.MetaDescription;
            this.MetaKeywords = "Review," + _LocalProduct.MetaKeywords;
        }

        private void LoadReviews()
        {
            this.dlReviews.Visible = true;

            this.lblRating.Visible = false;
            this.imgAverageRating.Visible = false;

            List<ProductReview> reviews = _LocalProduct.ReviewsApproved;
            if (reviews == null)
            {
                return;
            }
            if (reviews.Count < 1)
            {
                return;
            }

            //Average Rating Code
            if ((reviews.Count > 0))
            {
                this.lblRating.Visible = true;
                this.imgAverageRating.Visible = true;
            }

            int AverageRating = 3;
            double tempRating = 3.0;
            double sumRatings = 0.0;
            for (int i = 0; i <= reviews.Count - 1; i++)
            {
                sumRatings += (int)reviews[i].Rating;
            }
            if (sumRatings > 0)
            {
                tempRating = sumRatings / reviews.Count;
                AverageRating = (int)Math.Ceiling(tempRating);
            }

            this.lblRating.Text = SiteTerms.GetTerm(SiteTermIds.AverageRating);

            ThemeManager tm = this.BVApp.ThemeManager();

            switch (AverageRating)
            {
                case 1:
                    this.imgAverageRating.ImageUrl = tm.ButtonUrl("Stars1", Request.IsSecureConnection);
                    break;
                case 2:
                    this.imgAverageRating.ImageUrl = tm.ButtonUrl("Stars2", Request.IsSecureConnection);
                    break;
                case 3:
                    this.imgAverageRating.ImageUrl = tm.ButtonUrl("Stars3", Request.IsSecureConnection);
                    break;
                case 4:
                    this.imgAverageRating.ImageUrl = tm.ButtonUrl("Stars4", Request.IsSecureConnection);
                    break;
                case 5:
                    this.imgAverageRating.ImageUrl = tm.ButtonUrl("Stars5", Request.IsSecureConnection);
                    break;
            }
            this.imgAverageRating.ImageUrl = this.imgAverageRating.ImageUrl;

            this.dlReviews.DataSource = reviews;
            this.dlReviews.DataBind();

        }

        protected void dlReviews_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem | e.Item.ItemType == ListItemType.Item)
            {
                System.Web.UI.WebControls.Image ratingImage;
                System.Web.UI.WebControls.Label ratingDescription;
                System.Web.UI.WebControls.Panel karmaPanel;
                System.Web.UI.WebControls.Label karmaLabel;
                System.Web.UI.WebControls.ImageButton karmaYes;
                System.Web.UI.WebControls.ImageButton karmaNo;

                ratingImage = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgReviewRating");
                ratingDescription = (Label)e.Item.FindControl("lblReviewDescription");
                karmaPanel = (Panel)e.Item.FindControl("pnlKarma");

                ThemeManager tm = this.BVApp.ThemeManager();

                if (ratingImage != null)
                {
                    if (BVApp.CurrentStore.Settings.ProductReviewShowRating == true)
                    {
                        ratingImage.Visible = true;
                        switch ((int)DataBinder.Eval(e.Item.DataItem, "Rating"))
                        {
                            case 1:
                                ratingImage.ImageUrl = tm.ButtonUrl("Stars1", Request.IsSecureConnection);
                                break;
                            case 2:
                                ratingImage.ImageUrl = tm.ButtonUrl("Stars2", Request.IsSecureConnection);
                                break;
                            case 3:
                                ratingImage.ImageUrl = tm.ButtonUrl("Stars3", Request.IsSecureConnection);
                                break;
                            case 4:
                                ratingImage.ImageUrl = tm.ButtonUrl("Stars4", Request.IsSecureConnection);
                                break;
                            case 5:
                                ratingImage.ImageUrl = tm.ButtonUrl("Stars5", Request.IsSecureConnection);
                                break;
                        }
                    }
                    else
                    {
                        ratingImage.Visible = false;
                    }
                }

                if (ratingDescription != null)
                {
                    ratingDescription.Text = (string)DataBinder.Eval(e.Item.DataItem, "Description");
                }

                if (karmaPanel != null)
                {

                    karmaPanel.Visible = false;

                    karmaPanel.Visible = true;

                    karmaLabel = (Label)e.Item.FindControl("lblProductReviewKarma");
                    karmaYes = (ImageButton)e.Item.FindControl("btnReviewKarmaYes");
                    karmaNo = (ImageButton)e.Item.FindControl("btnReviewKarmaNo");

                    if (karmaLabel != null)
                    {
                        karmaLabel.Text = SiteTerms.GetTerm(SiteTermIds.WasThisReviewHelpful);
                    }
                    if (karmaYes != null)
                    {
                        karmaYes.ImageUrl = tm.ButtonUrl("Yes", Request.IsSecureConnection);
                    }
                    if (karmaNo != null)
                    {
                        karmaNo.ImageUrl = tm.ButtonUrl("No", Request.IsSecureConnection);
                    }

                }
            }
        }

        protected void dlReviews_EditCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            string reviewID = (string)dlReviews.DataKeys[e.Item.ItemIndex];
            BVApp.CatalogServices.ProductReviews.UpdateKarma(reviewID, 1);
        }

        protected void dlReviews_DeleteCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            string reviewID = (string)dlReviews.DataKeys[e.Item.ItemIndex];
            BVApp.CatalogServices.ProductReviews.UpdateKarma(reviewID, -1);
        }

    }
}