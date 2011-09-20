using System.Web.UI;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVModules_Controls_AdditionalImages : MerchantTribe.Commerce.Content.BVUserControl
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                ThemeManager themes = MyPage.MTApp.ThemeManager();
                this.imgZoom.ImageUrl = themes.ButtonUrl("MorePictures", Request.IsSecureConnection);
            }

            string id = Request.QueryString["ProductID"];
            this.ZoomLink.Style.Add("CURSOR", "pointer");
            this.ZoomLink.Attributes.Add("onclick", ViewUtilities.GetAdditionalImagesPopupJavascript(id, this.Page));

            Product baseProd = MyPage.MTApp.CatalogServices.Products.Find(id);
            if (baseProd != null)
            {
                List<ProductImage> images = MyPage.MTApp.CatalogServices.ProductImages.FindByProductId(baseProd.Bvin);

                if (images.Count <= 0)
                {
                    this.ZoomLink.Visible = false;
                }
            }

        }

    }
}