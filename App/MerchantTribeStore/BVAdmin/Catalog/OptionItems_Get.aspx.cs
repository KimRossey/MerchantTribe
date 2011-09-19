using System.Web.UI;
using MerchantTribe.Commerce.Catalog;

namespace BVCommerce
{

    partial class BVAdmin_Catalog_OptionItems_Get : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);


            if (!Page.IsPostBack)
            {
                string ids = Request["id"];
                OptionItem result = BVApp.CatalogServices.ProductOptions.OptionItemFind(ids);
                if (result != null)
                {
                    this.litOutput.Text = MerchantTribe.Web.Json.ObjectToJson(result);
                }
            }

        }

    }

}