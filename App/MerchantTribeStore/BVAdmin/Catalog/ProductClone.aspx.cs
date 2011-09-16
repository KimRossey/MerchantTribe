using System.Web.UI;
using System.Collections.Generic;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Membership;

namespace BVCommerce
{

    partial class BVAdmin_Catalog_ProductClone : BaseProductAdminPage
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (Request.QueryString["id"] == null)
            {
                Response.Redirect("~/BVAdmin/Catalog/default.aspx");
            }
            else
            {
                ViewState["id"] = Request.QueryString["id"];
                if (!Page.IsPostBack)
                {
                    Product product = BVApp.CatalogServices.Products.Find((string)ViewState["id"]);
                    if (product != null)
                    {
                        this.ProductChoicesCheckBox.Checked = true;
                        this.CategoryPlacementCheckBox.Checked = true;
                        this.ImagesCheckBox.Checked = true;
                        this.NameTextBox.Text = product.ProductName + " Copy";
                        this.SkuTextBox.Text = product.Sku + "-COPY";
                    }
                }
            }
        }

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Product Clone";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected void CloneButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (Page.IsValid)
            {
                Save();
            }
        }

        protected override bool Save()
        {
            Product product = BVApp.CatalogServices.Products.Find((string)ViewState["id"]);
            if (product != null)
            {
                if (product.Bvin != string.Empty)
                {
                    Product newProduct = product.Clone(ProductChoicesCheckBox.Checked, ImagesCheckBox.Checked);
                    if (InactiveCheckBox.Checked)
                    {
                        newProduct.Status = ProductStatus.Disabled;
                    }
                    newProduct.ProductName = NameTextBox.Text;
                    newProduct.Sku = SkuTextBox.Text;
                    if (BVApp.CatalogServices.ProductsCreateWithInventory(newProduct, true))
                    {
                        if (product.ProductTypeId != string.Empty)
                        {
                            List<ProductProperty> productTypes = BVApp.CatalogServices.ProductPropertiesFindForType(product.ProductTypeId);
                            foreach (ProductProperty item in productTypes)
                            {
                                string value = BVApp.CatalogServices.ProductPropertyValues.GetPropertyValue(product.Bvin, item.Id);
                                BVApp.CatalogServices.ProductPropertyValues.SetPropertyValue(newProduct.Bvin, item.Id, value);
                            }
                        }

                        //if (CategoryPlacementCheckBox.Checked)
                        //{
                        //    if (!Product.CopyCategoryPlacement(product.Bvin, newProduct.Bvin))
                        //    {
                        //        MessageBox1.ShowError("An error occurred while trying to copy the category placement.");
                        //    }
                        //}

                        Response.Redirect("~/BVAdmin/Catalog/default.aspx");
                    }
                    else
                    {
                        MessageBox1.ShowError("An error occurred while trying to save the clone. Please try again.");
                        return false;
                    }
                }
            }
            return true;
        }

        protected void CancelButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("~/BVAdmin/Catalog/default.aspx");
        }
    }
}