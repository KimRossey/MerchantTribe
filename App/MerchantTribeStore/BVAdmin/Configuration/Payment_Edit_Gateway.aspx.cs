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
using BVSoftware.Payment;
using System.Collections.Generic;

namespace BVCommerce
{

    partial class BVAdmin_Configuration_Payment_Edit_Gateway : BaseAdminPage
    {

        private BVSoftware.Payment.Methods.AvailableMethod g;
        private BVModule editor;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Payment Gateway Settings";
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
                this.GatewayIdField.Value = Request.QueryString["id"];
            }
            if (Request.QueryString["payid"] != null)
            {
                this.MethodIdField.Value = Request.QueryString["payid"];
            }
            //if (AvailableGateways.CurrentGateway().GatewayId == this.GatewayIdField.Value) {
            //    g = AvailableGateways.CurrentGateway();
            //}
            //else {
            List<BVSoftware.Payment.Methods.AvailableMethod> AvailableGateways = BVSoftware.Payment.Methods.AvailableMethod.FindAll();

            for (int i = 0; i <= AvailableGateways.Count - 1; i++)
            {
                if (string.Compare(AvailableGateways[i].Id, this.GatewayIdField.Value, true) == 0)
                {
                    g = AvailableGateways[i];
                }
            }
            //}

            LoadEditor();
        }

        private void LoadEditor()
        {
            System.Web.UI.Control tempControl = ModuleController.LoadCreditCardGatewayEditor(g.Name, this);

            if (tempControl is BVModule)
            {
                editor = (BVModule)tempControl;
                if (editor != null)
                {
                    editor.BlockId = g.Id;
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
            Response.Redirect("Payment_Edit.aspx?id=" + this.MethodIdField.Value);
        }


    }

}