using System.Collections.ObjectModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;
using System.Text;
using System.Collections.Generic;

namespace MerchantTribeStore
{

    partial class BVModules_ContentBlocks_Last_Products_Viewed_view : BVModule
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            this.LPVTitle.Text = SiteTerms.GetTerm(SiteTermIds.RecentlyViewedItems);
            LoadProductGrid();
        }

        private void LoadProductGrid()
        {
            List<Product> myProducts = PersonalizationServices.GetProductsViewed(MyPage.MTApp);

            if (myProducts.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<ul>");

                int i = 0;
                foreach (Product p in myProducts)
                {
                    if (i < 5)
                    {
                        UserSpecificPrice price = MyPage.MTApp.PriceProduct(p, MyPage.MTApp.CurrentCustomer, null);
                        MerchantTribe.Commerce.Utilities.HtmlRendering.RenderSingleProduct(ref sb, p, false, false, this.Page, price);
                    }
                }

                sb.Append("</ul>");
                this.litItems.Text = sb.ToString();
            }
        }

    }
}