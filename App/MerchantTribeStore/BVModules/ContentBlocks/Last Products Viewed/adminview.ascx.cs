using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce.Utilities;

namespace BVCommerce
{

    partial class BVModules_ContentBlocks_Last_Products_Viewed_adminview : BVModule
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            this.LPVTitle.Text = SiteTerms.GetTerm(SiteTermIds.RecentlyViewedItems);
            LoadProductGrid();
        }

        private void LoadProductGrid()
        {
            List<Product> myProducts = PersonalizationServices.GetProductsViewed(MyPage.BVApp);

            List<Product> n = new List<Product>();
            int i = 0;

            foreach (Product p in myProducts)
            {
                if (i < WebAppSettings.LastProductsViewedMaxResults)
                {
                    n.Add(p);
                    i += 1;
                }
            }

        }

    }
}