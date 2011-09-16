using System.Web.UI;
using BVSoftware.Commerce.Catalog;

namespace BVCommerce
{

    partial class BVAdmin_Catalog_ProductVariants_Get : BaseAdminJsonPage
    {

        public class VariantInfo
        {
            public string Bvin = string.Empty;
            public string Sku = string.Empty;
            public decimal Price = 0;
            public string Description = string.Empty;
            public string ImageUrl = string.Empty;
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string ids = Request["id"];
                Variant result = BVApp.CatalogServices.ProductVariants.Find(ids);
                if (result != null)
                {
                    Product p = BVApp.CatalogServices.Products.Find(result.ProductId);
                    VariantInfo output = new VariantInfo();
                    output.Bvin = result.Bvin;
                    output.Sku = result.Sku;
                    if (output.Sku == string.Empty)
                    {
                        output.Sku = p.Sku;
                    }
                    output.Price = result.Price;
                    if (output.Price < 0)
                    {
                        output.Price = p.SitePrice;
                    }
                    output.ImageUrl = BVSoftware.Commerce.Storage.DiskStorage.ProductVariantImageUrlMedium(this.BVApp.CurrentStore.Id, p.Bvin, p.ImageFileSmall, result.Bvin, true);

                    foreach (string s in result.SelectionNames(p.Options.VariantsOnly()))
                    {
                        output.Description += s + ", ";
                    }
                    this.litOutput.Text = MerchantTribe.Web.Json.ObjectToJson(output);
                }
            }

        }

    }
}