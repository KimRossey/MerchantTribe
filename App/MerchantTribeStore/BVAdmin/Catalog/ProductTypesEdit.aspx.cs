using System.Web.UI;
using System.Collections.Generic;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Membership;

namespace BVCommerce
{

    partial class Product_ProductTypes_Edit : BaseAdminPage
    {
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Product Types Edit";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                this.CurrentTab = AdminTabType.Catalog;

                string productID = string.Empty;
                if (Request.QueryString["ID"] != null)
                {
                    productID = Request.QueryString["id"];
                }
                else
                {
                    msg.ShowError("No product type ID was found.");
                }

                ViewState["ID"] = productID;
                LoadType();
            }

        }

        private void LoadType()
        {
            ProductType prodType = new ProductType();
            prodType = BVApp.CatalogServices.ProductTypes.Find((string)ViewState["ID"]);
            if (prodType != null)
            {

                this.ProductTypeNameField.Text = prodType.ProductTypeName;

                LoadPropertyLists();

                if (BVApp.CatalogServices.Products.FindCountByProductType(prodType.Bvin) > 0)
                {
                    msg.ShowWarning("Editing this type will affect all existing products of this type. Any default values you set here will NOT overwrite existing products but any properties that you add or remove will affect existing products.");
                }
            }
            else
            {
                msg.ShowError("Unable to load Product Type ID " + ViewState["ID"]);
            }
        }

        protected void btnCancel_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            // Delete newly created item if user cancels so we don't leave a bunch of "new property"
            if (Request["newmode"] == "1")
            {
                BVApp.CatalogServices.ProductTypeDestroy((string)ViewState["ID"]);
            }
            Response.Redirect("ProductTypes.aspx");
        }

        protected void btnSave_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            msg.ClearMessage();
            ProductType prodType = new ProductType();
            prodType = BVApp.CatalogServices.ProductTypes.Find((string)ViewState["ID"]);
            if (prodType != null)
            {

                prodType.ProductTypeName = this.ProductTypeNameField.Text;

                if (BVApp.CatalogServices.ProductTypes.Update(prodType) == true)
                {
                    prodType = null;
                    Response.Redirect("ProductTypes.aspx");
                }
                else
                {
                    prodType = null;
                    msg.ShowError("Error: Couldn't Save Product Type!");
                }
            }
            else
            {
                msg.ShowError("Couldn't Load Product Type to Update!");
            }

        }

        private void LoadPropertyLists()
        {

            this.lstProperties.DataSource = BVApp.CatalogServices.ProductPropertiesFindForType((string)ViewState["ID"]);
            this.lstProperties.DataTextField = "PropertyName";
            this.lstProperties.DataValueField = "id";
            this.lstProperties.DataBind();

            this.lstAvailableProperties.DataSource = BVApp.CatalogServices.ProductPropertiesFindNotAssignedToType((string)ViewState["ID"]);
            this.lstAvailableProperties.DataTextField = "PropertyName";
            this.lstAvailableProperties.DataValueField = "id";
            this.lstAvailableProperties.DataBind();
        }

        protected void btnAddProperty_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {            
            string typeBvin = (string)ViewState["ID"];
            long propertyId = long.Parse(lstAvailableProperties.SelectedValue);
            BVApp.CatalogServices.ProductTypeAddProperty(typeBvin, propertyId);
            this.LoadPropertyLists();
        }

        protected void btnRemoveProperty_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string typeBvin = (string)ViewState["ID"];
            long propertyId = long.Parse(lstAvailableProperties.SelectedValue);
            BVApp.CatalogServices.ProductTypeRemoveProperty(typeBvin, propertyId);            
            this.LoadPropertyLists();
        }

        protected void btnMovePropertyUp_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string typeBvin = (string)ViewState["ID"];
            long selected = long.Parse(this.lstProperties.SelectedValue);
            BVApp.CatalogServices.ProductTypeMovePropertyUp(typeBvin, selected);            
            this.LoadPropertyLists();
            foreach (System.Web.UI.WebControls.ListItem li in this.lstProperties.Items)
            {
                if (li.Value == selected.ToString())
                {
                    lstProperties.ClearSelection();
                    li.Selected = true;
                }
            }
        }

        protected void btnMovePropertyDown_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string typeBvin = (string)ViewState["ID"];
            long selected = long.Parse(this.lstProperties.SelectedValue);
            BVApp.CatalogServices.ProductTypeMovePropertyDown(typeBvin, selected);            
            this.LoadPropertyLists();
            foreach (System.Web.UI.WebControls.ListItem li in this.lstProperties.Items)
            {
                if (li.Value == selected.ToString())
                {
                    lstProperties.ClearSelection();
                    li.Selected = true;
                }
            }
        }
    }
}