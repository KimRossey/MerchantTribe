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
using MerchantTribe.Web.Geography;

using System.Collections.ObjectModel;

namespace BVCommerce
{

    partial class BVAdmin_Configuration_Countries : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Countries";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
           
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadCountries();
            }
        }

        private void LoadCountries()
        {
            List<Country> disabled = Country.FindAllInList(BVApp.CurrentStore.Settings.DisabledCountryIso3Codes);
            List<Country> active = BVApp.CurrentStore.Settings.FindActiveCountries();

            this.lstActive.DataSource = active;
            this.lstActive.DataTextField = "DisplayName";
            this.lstActive.DataValueField = "IsoAlpha3";
            this.lstActive.DataBind();

            this.lstDisabled.DataSource = disabled;
            this.lstDisabled.DataTextField = "DisplayName";
            this.lstDisabled.DataValueField = "IsoAlpha3";
            this.lstDisabled.DataBind();
        }


        protected void btnDisabled_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();

            if (this.lstActive.SelectedItem != null)
            {
                string iso = this.lstActive.SelectedItem.Value;
                BVApp.CurrentStore.Settings.DisableCountry(iso);
                MessageBox1.ShowOk("Country Disabled");
            }

            BVApp.UpdateCurrentStore();

            LoadCountries();
        }

        protected void btnEnable_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();

            if (this.lstDisabled.SelectedItem == null)
            {
                this.MessageBox1.ShowWarning("Please select a country to enable first.");
            }
            else
            {
                string iso = this.lstDisabled.SelectedItem.Value;
                BVApp.CurrentStore.Settings.EnableCountry(iso);
                MessageBox1.ShowOk("Country enabled");
            }

            BVApp.UpdateCurrentStore();

            LoadCountries();
        }


    }
}