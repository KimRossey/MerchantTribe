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
using BVSoftware.Shipping;

namespace BVCommerce
{

    partial class BVAdmin_Configuration_Shipping_EditMethod : BaseAdminPage
    {

        private ShippingMethod m;
        private BVShippingModule editor;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {

            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Shipping Method";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
            if (this.editor != null)
            {
                this.editor.EditingComplete += this.editor_EditingComplete;
            }
        }


        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            if (Request.QueryString["id"] != null)
            {
                this.BlockIDField.Value = Request.QueryString["id"];
            }
            m = BVApp.OrderServices.ShippingMethods.Find(this.BlockIDField.Value);
            LoadEditor();
        }

        private IShippingService FindProvider(string bvin)
        {
            return BVSoftware.Commerce.Shipping.AvailableServices.FindById(bvin, BVApp.CurrentStore);
        }

        private void LoadEditor()
        {
            System.Web.UI.Control tempControl = null;

            BVSoftware.Shipping.IShippingService p = BVSoftware.Commerce.Shipping.AvailableServices.FindById(m.ShippingProviderId, BVApp.CurrentStore);

            tempControl = ModuleController.LoadShippingEditor(p.Name, this);

            if (tempControl is BVShippingModule)
            {
                editor = (BVShippingModule)tempControl;
                if (editor != null)
                {
                    editor.BlockId = m.Bvin;
                    editor.ShippingMethod = m;
                    this.phEditor.Controls.Add(editor);
                    this.editor.EditingComplete += this.editor_EditingComplete;
                }
            }
            else
            {
                this.phEditor.Controls.Add(new System.Web.UI.LiteralControl("Error, editor is not based on Content.BVShippingModule class"));
            }
        }

        protected void editor_EditingComplete(object sender, BVSoftware.Commerce.Content.BVModuleEventArgs e)
        {
            if ((e.Info.ToUpper() == "CANCELED"))
            {
                if ((Request.QueryString["doc"] == "1"))
                {
                    BVApp.OrderServices.ShippingMethods.Delete(m.Bvin);
                }
            }
            else
            {
                if (e.Info != string.Empty)
                {
                    m.Name = e.Info;
                    BVApp.OrderServices.ShippingMethods.Update(m);
                }
            }
            Response.Redirect("Shipping.aspx");
        }

    }
}