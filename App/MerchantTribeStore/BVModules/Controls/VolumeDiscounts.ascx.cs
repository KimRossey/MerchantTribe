using System.Collections.ObjectModel;
using System.Web.UI;
using MerchantTribe.Commerce.Catalog;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVModules_Controls_VolumeDiscounts : MerchantTribe.Commerce.Content.BVUserControl
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

                    volumeDiscounts = MyPage.MTApp.CatalogServices.VolumeDiscounts.FindByProductId(prodPage.LocalProduct.Bvin);

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