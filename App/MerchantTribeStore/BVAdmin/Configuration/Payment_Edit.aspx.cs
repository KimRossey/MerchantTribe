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

namespace BVCommerce
{

    partial class BVAdmin_Configuration_Payment_Edit : BaseAdminPage
    {

        private DisplayPaymentMethod m;
        private BVModule editor;
        private BVSoftware.Commerce.Payment.AvailablePayments availablePayments = new BVSoftware.Commerce.Payment.AvailablePayments();

        public override bool RequiresSSL
        {
            get { return true; }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Payment Settings";
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
                this.MethodIdField.Value = Request.QueryString["id"];
            }
            for (int i = 0; i <= availablePayments.Methods.Count - 1; i++)
            {
                if (string.Compare(availablePayments.Methods[i].MethodId, this.MethodIdField.Value, true) == 0)
                {
                    m = availablePayments.Methods[i];
                }
            }
            LoadEditor();
        }

        private void LoadEditor()
        {
            System.Web.UI.Control tempControl = ModuleController.LoadPaymentMethodEditor(m.MethodName, this);

            if (tempControl is BVModule)
            {
                editor = (BVModule)tempControl;
                if (editor != null)
                {
                    editor.BlockId = m.MethodId;
                    this.phEditor.Controls.Add(editor);
                    this.editor.EditingComplete += this.editor_EditingComplete;
                }
            }
            else
            {
                this.phEditor.Controls.Add(new System.Web.UI.LiteralControl("Error, editor is not based on Content.BVModule class"));
            }
        }

        protected void editor_EditingComplete(object sender, BVSoftware.Commerce.Content.BVModuleEventArgs e)
        {
            Response.Redirect("Payment.aspx");
        }

    }
}