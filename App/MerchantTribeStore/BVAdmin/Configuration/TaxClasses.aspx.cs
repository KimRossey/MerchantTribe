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

    partial class BVAdmin_Configuration_TaxClasses : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Tax Schedules";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {

                LoadSchedules();

            }

        }

        protected void btnAddNewRegion_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            msg.ClearMessage();
            try
            {
                BVSoftware.Commerce.Taxes.TaxSchedule t = new TaxSchedule();
                t.Name = this.DisplayNameField.Text.Trim();
                BVApp.OrderServices.TaxSchedules.Create(t);
                msg.ShowOk("Added: " + t.Name);
                LoadSchedules();
                DisplayNameField.Text = "";
            }
            catch (Exception Ex)
            {
                msg.ShowException(Ex);
            }
        }

        void LoadSchedules()
        {
            try
            {
                dgTaxClasses.DataSource = BVApp.OrderServices.TaxSchedules.FindAllAndCreateDefault(BVApp.CurrentStore.Id);
                dgTaxClasses.DataBind();
            }
            catch (Exception Ex)
            {
                msg.ShowException(Ex);
            }
        }

        public void dgTaxClasses_Delete(System.Object sender, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            long editID = (long)dgTaxClasses.DataKeys[e.Item.ItemIndex];
            BVApp.OrderServices.TaxSchedulesDestroy(editID);
            LoadSchedules();
        }


    }
}