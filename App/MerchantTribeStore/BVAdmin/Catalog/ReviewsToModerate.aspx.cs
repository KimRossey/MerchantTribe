using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;

namespace BVCommerce
{

    partial class Products_ReviewsToModerate : BaseAdminPage
    {
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Moderate Reviews";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                this.CurrentTab = AdminTabType.Catalog;
                LoadReviews();
            }
        }

        private void LoadReviews()
        {
            if (this.BVApp.CatalogServices.ProductReviews.FindNotApproved(1,1).Count == 0)
            {
                lblNoReviews.Visible = true;
            }
            else
            {
                this.dlReviews.DataSource = this.BVApp.CatalogServices.ProductReviews.FindNotApproved(1,100);
                this.dlReviews.DataBind();
            }

        }

        protected void dlReviews_DeleteCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            string reviewID = (string)dlReviews.DataKeys[e.Item.ItemIndex];
            this.BVApp.CatalogServices.ProductReviews.Delete(reviewID);
            Response.Redirect("ReviewsToModerate.aspx");
            //LoadReviews()
        }

        protected void dlReviews_EditCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            string reviewID = (string)dlReviews.DataKeys[e.Item.ItemIndex];
            string pid = Request.QueryString[ID];
            Response.Redirect("Reviews_Edit.aspx?reviewID=" + reviewID);
        }

        protected void dlReviews_UpdateCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            string reviewID = (string)dlReviews.DataKeys[e.Item.ItemIndex];
            ProductReview r;
            r = this.BVApp.CatalogServices.ProductReviews.Find(reviewID);
            r.Approved = true;
            this.BVApp.CatalogServices.ProductReviews.Update(r);
            //LoadReviews()
            Response.Redirect("ReviewsToModerate.aspx");
        }

        protected void dlReviews_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem | e.Item.ItemType == ListItemType.Item)
            {
                System.Web.UI.WebControls.Image imgRating;
                imgRating = (Image)e.Item.FindControl("imgRating");
                if (imgRating != null)
                {
                    ProductReviewRating rating = (ProductReviewRating)DataBinder.Eval(e.Item.DataItem, "Rating");
                    switch (rating)
                    {
                        case ProductReviewRating.ZeroStars:
                            imgRating.ImageUrl = this.BVApp.ThemeManager().ButtonUrl("stars0", Request.IsSecureConnection);
                            break;
                        case ProductReviewRating.OneStar:
                            imgRating.ImageUrl = this.BVApp.ThemeManager().ButtonUrl("stars1", Request.IsSecureConnection);
                            break;
                        case ProductReviewRating.TwoStars:
                            imgRating.ImageUrl = this.BVApp.ThemeManager().ButtonUrl("stars2", Request.IsSecureConnection);
                            break;
                        case ProductReviewRating.ThreeStars:
                            imgRating.ImageUrl = this.BVApp.ThemeManager().ButtonUrl("stars3", Request.IsSecureConnection);
                            break;
                        case ProductReviewRating.FourStars:
                            imgRating.ImageUrl = this.BVApp.ThemeManager().ButtonUrl("stars4", Request.IsSecureConnection);
                            break;
                        case ProductReviewRating.FiveStars:
                            imgRating.ImageUrl = this.BVApp.ThemeManager().ButtonUrl("stars5", Request.IsSecureConnection);
                            break;
                    }
                }
            }
        }

    }
}