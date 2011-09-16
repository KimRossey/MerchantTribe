using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Accounts;
using BVSoftware.Commerce.BusinessRules;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Contacts;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce.Membership;
using BVSoftware.Commerce.Metrics;
using BVSoftware.Commerce.Orders;
using BVSoftware.Commerce.Payment;
using BVSoftware.Commerce.Shipping;
using BVSoftware.Commerce.Taxes;
using BVSoftware.Commerce.Utilities;
using System.Collections.Generic;

using System.Collections.ObjectModel;

namespace BVCommerce
{

    partial class BVAdmin_Configuration_Shipping : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Shipping Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                LoadMethods();
                LoadProviders();
            }
        }

        private void LoadMethods()
        {
            List<ShippingMethod> methods;
            methods = BVApp.OrderServices.ShippingMethods.FindAll(BVApp.CurrentStore.Id);
            this.GridView1.DataSource = methods;
            this.GridView1.DataBind();
        }

        private void LoadProviders()
        {
            this.lstProviders.ClearSelection();
            foreach (BVSoftware.Shipping.IShippingService p in BVSoftware.Commerce.Shipping.AvailableServices.FindAll(this.BVApp.CurrentStore))
            {
                this.lstProviders.Items.Add(new System.Web.UI.WebControls.ListItem(p.Name, p.Id));
            }
        }

        private void NewMethod()
        {
            ShippingMethod m = new ShippingMethod();
            m.ShippingProviderId = this.lstProviders.SelectedValue;
            m.Name = "New Shipping Method";
            if (BVApp.OrderServices.ShippingMethods.Create(m) == true)
            {
                Response.Redirect("Shipping_EditMethod.aspx?id=" + m.Bvin + "&doc=1");
            }
        }

        protected void GridView1_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            string bvin = (string)GridView1.DataKeys[e.RowIndex].Value;
            BVApp.OrderServices.ShippingMethods.Delete(bvin);
            LoadMethods();
        }

        protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            string bvin = (string)GridView1.DataKeys[e.NewEditIndex].Value;
            Response.Redirect("Shipping_EditMethod.aspx?id=" + bvin);
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            NewMethod();
        }

    }
}