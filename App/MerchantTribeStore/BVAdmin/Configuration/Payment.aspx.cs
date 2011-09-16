
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

namespace BVCommerce
{

    partial class BVAdmin_Configuration_Payment : BaseAdminPage
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadMethods();
                ShowFreeMessage();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Payment Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        private void LoadMethods()
        {
            BVSoftware.Commerce.Payment.AvailablePayments availablePayments = new BVSoftware.Commerce.Payment.AvailablePayments();
            this.PaymentMethodsGrid.DataSource = availablePayments.AvailableMethodsForPlan(BVApp.CurrentStore.PlanId);
            this.PaymentMethodsGrid.DataBind();
        }

        protected void btnSaveChanges_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveChanges();
            LoadMethods();
            this.MessageBox1.ShowOk("Settings saved successfully.");
        }

        private void SaveChanges()
        {
            Dictionary<string, string> newList = new Dictionary<string, string>();
            for (int i = 0; i <= PaymentMethodsGrid.Rows.Count - 1; i++)
            {

                CheckBox chkEnabled = (CheckBox)PaymentMethodsGrid.Rows[i].FindControl("chkEnabled");
                if (chkEnabled != null)
                {
                    if (chkEnabled.Checked)
                    {
                        newList.Add((string)PaymentMethodsGrid.DataKeys[i].Value, string.Empty);
                    }
                }
            }

            BVApp.CurrentStore.Settings.PaymentMethodsEnabled = newList;
            BVApp.UpdateCurrentStore();

        }

        protected void PaymentMethodsGrid_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            DisplayPaymentMethod m = (DisplayPaymentMethod)e.Row.DataItem;
            if (m != null)
            {
                CheckBox chkEnabled = (CheckBox)e.Row.FindControl("chkEnabled");
                if (chkEnabled != null)
                {
                    if (BVApp.CurrentStore.Settings.PaymentMethodsEnabled.ContainsKey(m.MethodId))
                    {
                        chkEnabled.Checked = true;
                    }
                    else
                    {
                        chkEnabled.Checked = false;
                    }
                }

                // Check for Editor
                ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                if (btnEdit != null)
                {
                    btnEdit.Visible = false;
                    System.Web.UI.Control editor = ModuleController.LoadPaymentMethodEditor(m.MethodName, this.Page);
                    if (editor != null)
                    {
                        if (editor is BVModule)
                        {
                            btnEdit.Visible = true;
                        }
                    }
                }
            }
        }

        protected void PaymentMethodsGrid_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            string bvin = this.PaymentMethodsGrid.DataKeys[e.NewEditIndex].Value.ToString();
            Response.Redirect("Payment_Edit.aspx?id=" + bvin);
        }

        private void ShowFreeMessage()
        {
            if (BVApp.CurrentStore.PlanId == 0)
            {
                this.MessageBox1.ShowInformation("Your store is on the Free plan. <a href=\"../ChangePlan.aspx\">Upgrade Your Store</a> to allow other credit card processors, purchase orders, and more.");
            }
        }
    }
}