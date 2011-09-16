using System;
using System.Web;
using System.Web.UI.WebControls;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Content;

namespace BVCommerce
{

    partial class BVAdmin_Controls_ProductEditingDisplay : BVUserControl
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Request.QueryString["id"] != null)
            {
                Product prod = MyPage.BVApp.CatalogServices.Products.Find(Request.QueryString["id"]);
                productImage.ImageUrl = BVSoftware.Commerce.Storage.DiskStorage.ProductImageUrlSmall(this.MyPage.BVApp.CurrentStore.Id, prod.Bvin, prod.ImageFileSmall, true);
                productLabel.Text = prod.ProductName;
                productSkuLabel.Text = prod.Sku;
                productPrice.Text = prod.SitePrice.ToString("C");
            }

        }
    }
}