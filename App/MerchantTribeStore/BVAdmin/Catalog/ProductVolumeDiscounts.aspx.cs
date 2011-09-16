using System.Web.UI;
using System.Collections.ObjectModel;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Membership;
using System.Collections.Generic;

namespace BVCommerce
{

    partial class BVAdmin_Catalog_ProductVolumeDiscounts : BaseProductAdminPage
    {

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Product Volume Discounts";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["id"] == null)
                {
                    Response.Redirect(DefaultCatalogPage);
                }
                else
                {
                    ViewState["id"] = Request.QueryString["id"];
                    BindGridViews();
                }
            }
        }

        protected void BindGridViews()
        {
            VolumeDiscountsGridView.DataSource = BVApp.CatalogServices.VolumeDiscounts.FindByProductId((string)ViewState["id"]);
            VolumeDiscountsGridView.DataBind();
        }

        protected void NewLevelImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            int quantity = 0;
            decimal amount = 0m;
            if (!int.TryParse(QuantityTextBox.Text, out quantity))
            {
                MessageBox1.ShowError("Quantity must be numeric.");
            }

            if (!decimal.TryParse(PriceTextBox.Text, out amount))
            {
                MessageBox1.ShowError("Price must be a monetary amount.");
            }

            List<ProductVolumeDiscount> volumeDiscounts = BVApp.CatalogServices.VolumeDiscounts.FindByProductId((string)ViewState["id"]);
            ProductVolumeDiscount volumeDiscount = null;
            foreach (ProductVolumeDiscount item in volumeDiscounts)
            {
                if (item.Qty == quantity)
                {
                    volumeDiscount = item;
                }
            }
            if (volumeDiscount == null)
            {
                volumeDiscount = new ProductVolumeDiscount();
            }

            volumeDiscount.DiscountType = ProductVolumeDiscountType.Amount;
            volumeDiscount.Amount = amount;
            volumeDiscount.Qty = quantity;
            volumeDiscount.ProductId = (string)ViewState["id"];

            bool result = false;
            if (volumeDiscount.Bvin == string.Empty)
            {
                result = BVApp.CatalogServices.VolumeDiscounts.Create(volumeDiscount);
            }
            else
            {
                result = BVApp.CatalogServices.VolumeDiscounts.Update(volumeDiscount);
            }
            if (result)
            {
                MessageBox1.ShowOk("Volume Discount Updated");
                QuantityTextBox.Text = "";
                PriceTextBox.Text = "";
            }
            else
            {
                MessageBox1.ShowError("Error occurred while inserting new volume discount");
            }
            BindGridViews();
        }

        protected void VolumeDiscountsGridView_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            if (BVApp.CatalogServices.VolumeDiscounts.Delete(VolumeDiscountsGridView.DataKeys[e.RowIndex].Value.ToString()))
            {
                MessageBox1.ShowOk("Volume discount level deleted");
            }
            else
            {
                MessageBox1.ShowOk("An error occurred while trying to delete the volume discount level");
            }
            BindGridViews();
        }

        protected override bool Save()
        {
            return true;
        }
    }
}