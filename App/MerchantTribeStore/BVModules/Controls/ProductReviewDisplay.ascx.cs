using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;

namespace BVCommerce
{

    partial class BVModules_Controls_ProductReviewDisplay : MerchantTribe.Commerce.Content.BVUserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.btnSubmitReview.ImageUrl = MyPage.BVApp.ThemeManager().ButtonUrl("submit", Request.IsSecureConnection);
        }
        public void LoadReviews(string productBvin)
        {
            Product p = MyPage.BVApp.CatalogServices.Products.Find(productBvin);
            LoadReviews(p);
        }

        public void LoadReviews(Product p)
        {
            this.dlReviews.Visible = true;

            this.lblRating.Visible = false;
            this.imgAverageRating.Visible = false;
            this.lnkAllReviews.Visible = false;

            if ((p == null))
            {
                return;
            }
            this.bvinField.Value = p.Bvin;

            List<ProductReview> reviews = p.ReviewsApproved;
            if (reviews == null)
            {
                return;
            }

            //Average Rating Code
            if ((reviews.Count > 0))
            {
                this.lblRating.Visible = true;
                this.imgAverageRating.Visible = true;
                this.lnkAllReviews.Visible = true;
                this.lnkAllReviews.Text = "View All " + reviews.Count + " Reviews";
                this.lnkAllReviews.NavigateUrl = Page.ResolveUrl("~/ProductReviews/" + p.UrlSlug);
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

            switch (AverageRating)
            {
                case 1:
                    this.imgAverageRating.ImageUrl = MyPage.BVApp.ThemeManager().ButtonUrl("stars1", Request.IsSecureConnection);
                    break;
                case 2:
                    this.imgAverageRating.ImageUrl = MyPage.BVApp.ThemeManager().ButtonUrl("stars2", Request.IsSecureConnection);
                    break;
                case 3:
                    this.imgAverageRating.ImageUrl = MyPage.BVApp.ThemeManager().ButtonUrl("stars3", Request.IsSecureConnection);
                    break;
                case 4:
                    this.imgAverageRating.ImageUrl = MyPage.BVApp.ThemeManager().ButtonUrl("stars4", Request.IsSecureConnection);
                    break;
                case 5:
                    this.imgAverageRating.ImageUrl = MyPage.BVApp.ThemeManager().ButtonUrl("stars5", Request.IsSecureConnection);
                    break;
            }
            this.imgAverageRating.ImageUrl = this.imgAverageRating.ImageUrl;

            // Trim Reviews if more than count and display
            if (MyPage.BVApp.CurrentStore.Settings.ProductReviewCount < reviews.Count)
            {
                this.dlReviews.DataSource = TrimReviewList(reviews, MyPage.BVApp.CurrentStore.Settings.ProductReviewCount);
            }
            else
            {
                this.dlReviews.DataSource = reviews;
            }

            this.dlReviews.DataBind();

        }

        private List<ProductReview> TrimReviewList(List<ProductReview> source, int maxReviews)
        {
            List<ProductReview> result = new List<ProductReview>();
            for (int i = 0; i <= source.Count - 1; i++)
            {
                if (i < maxReviews)
                {
                    result.Add(source[i]);
                }
            }
            return result;
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

                ratingImage = (Image)e.Item.FindControl("imgReviewRating");
                ratingDescription = (Label)e.Item.FindControl("lblReviewDescription");
                karmaPanel = (Panel)e.Item.FindControl("pnlKarma");

                if (ratingImage != null)
                {
                    if (MyPage.BVApp.CurrentStore.Settings.ProductReviewShowRating == true)
                    {
                        ratingImage.Visible = true;
                        switch ((int)DataBinder.Eval(e.Item.DataItem, "Rating"))
                        {
                            case 1:
                                ratingImage.ImageUrl = MyPage.BVApp.ThemeManager().ButtonUrl("stars1", Request.IsSecureConnection);
                                break;
                            case 2:
                                ratingImage.ImageUrl = MyPage.BVApp.ThemeManager().ButtonUrl("stars2", Request.IsSecureConnection);
                                break;
                            case 3:
                                ratingImage.ImageUrl = MyPage.BVApp.ThemeManager().ButtonUrl("stars3", Request.IsSecureConnection);
                                break;
                            case 4:
                                ratingImage.ImageUrl = MyPage.BVApp.ThemeManager().ButtonUrl("stars4", Request.IsSecureConnection);
                                break;
                            case 5:
                                ratingImage.ImageUrl = MyPage.BVApp.ThemeManager().ButtonUrl("stars5", Request.IsSecureConnection);
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
                        karmaYes.ImageUrl = MyPage.BVApp.ThemeManager().ButtonUrl("yes", Request.IsSecureConnection);
                    }
                    if (karmaNo != null)
                    {
                        karmaNo.ImageUrl = MyPage.BVApp.ThemeManager().ButtonUrl("no", Request.IsSecureConnection);
                    }

                }
            }
        }

        protected void dlReviews_EditCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            string reviewID = (string)dlReviews.DataKeys[e.Item.ItemIndex];
            MyPage.BVApp.CatalogServices.ProductReviews.UpdateKarma(reviewID, 1);
        }

        protected void dlReviews_DeleteCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            string reviewID = (string)dlReviews.DataKeys[e.Item.ItemIndex];
            MyPage.BVApp.CatalogServices.ProductReviews.UpdateKarma(reviewID, -1);
        }

        protected void btnSubmitReview_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.litReviewMessage.Text = string.Empty;
            try
            {
                if (this.NewReviewDescription.Text.Trim().Length < 1)
                {
                    this.litReviewMessage.Text = "<div class=\"flash-message-warning\">Reviews can not be blank. Please enter your review and try again.</div>";
                }
                else
                {
                    ProductReview rev = new ProductReview();
                    rev.ProductBvin = this.bvinField.Value;
                    rev.Karma = 0;
                    if (SessionManager.IsUserAuthenticated(MyPage.BVApp) == true)
                    {
                        rev.UserID = SessionManager.GetCurrentUserId();
                    }
                    else
                    {
                        rev.UserID = "0";
                    }

                    rev.Description = HttpUtility.HtmlEncode(this.NewReviewDescription.Text.Trim());
                    rev.ReviewDate = System.DateTime.Now;
                    rev.Rating = (ProductReviewRating)int.Parse(this.lstNewReviewRating.SelectedValue);

                    if (MyPage.BVApp.CurrentStore.Settings.ProductReviewModerate == false)
                    {
                        rev.Approved = true;
                    }
                    else
                    {
                        rev.Approved = false;
                    }

                    MyPage.BVApp.CatalogServices.ProductReviews.Create(rev);

                    this.litReviewMessage.Text = "<div class=\"flash-message-success\">Thank you for your review!</div>";
                    this.pnlNewReview.Visible = false;
                }
            }

            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
            }
        }
    }
}