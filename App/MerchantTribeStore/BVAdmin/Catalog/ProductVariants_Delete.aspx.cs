using System.Web.UI;
using MerchantTribe.Commerce.Catalog;

namespace BVCommerce
{

    partial class BVAdmin_Catalog_ProductVariants_Delete : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string ids = Request.Form["id"];
                Remove(ids);
            }

        }

        private void Remove(string ids)
        {
            if ((BVApp.CatalogServices.ProductVariants.Delete(ids)))
            {
                this.litOutput.Text = "{\"result\":true}";
            }
            else
            {
                this.litOutput.Text = "{\"result\":false}";
            }
        }

    }
}