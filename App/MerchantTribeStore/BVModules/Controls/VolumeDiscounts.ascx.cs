using System.Collections.ObjectModel;
using System.Web.UI;
using BVSoftware.Commerce.Catalog;
using System.Collections.Generic;

namespace BVCommerce
{

    partial class BVModules_Controls_VolumeDiscounts : BVSoftware.Commerce.Content.BVUserControl
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                if (this.Page is IProductPage)
                {
                    IProductPage prodPage = (IProductPage)this.Page;
                    List<ProductVolumeDiscount> volumeDiscounts;

                    volumeDiscounts = MyPage.BVApp.CatalogServices.VolumeDiscounts.FindByProductId(prodPage.LocalProduct.Bvin);

                    if (volumeDiscounts.Count > 0)
                    {
                        pnlVolumeDiscounts.Visible = true;
                        dgVolumeDiscounts.DataSource = volumeDiscounts;
                        dgVolumeDiscounts.DataBind();
                    }
                    else
                    {
                        pnlVolumeDiscounts.Visible = false;
                    }

                }
            }
        }
    }
}