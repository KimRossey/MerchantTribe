using System.Collections.ObjectModel;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Utilities;
using System.Text;
using System.Collections.Generic;

namespace BVCommerce
{

    partial class BVModules_ContentBlocks_Product_Rotator_adminview : BVModule
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            LoadProduct();
        }

        private void LoadProduct()
        {
            ContentBlock b = MyPage.BVApp.ContentServices.Columns.FindBlock(this.BlockId);
            List<ContentBlockSettingListItem> myProducts = b.Lists.FindList("Products");
            if (myProducts != null)
            {
                if (myProducts.Count > 0)
                {
                    int displayIndex = GetProductIndex(myProducts.Count - 1);
                    SetProducts(myProducts[displayIndex]);
                }
            }
        }

        private void SetProducts(ContentBlockSettingListItem data)
        {
            string bvin = data.Setting1;
            Product p = MyPage.BVApp.CatalogServices.Products.Find(bvin);
            StringBuilder sb = new StringBuilder();
            UserSpecificPrice price = MyPage.BVApp.PriceProduct(p, MyPage.BVApp.CurrentCustomer, null);
            HtmlRendering.RenderSingleProduct(ref sb, p, false, false, this.Page, price);
            this.litMain.Text = sb.ToString();
        }

        private int GetProductIndex(int maxIndex)
        {
            int result = 0;

            result = MerchantTribe.Web.RandomNumbers.RandomInteger(maxIndex, 0);

            return result;
        }

    }
}