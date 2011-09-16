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

    partial class BVAdmin_Configuration_Analytics : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Analytics Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {

                // Loading
                this.chkGoogleAdwords.Checked = BVApp.CurrentStore.Settings.Analytics.UseGoogleAdWords;
                this.GoogleAdwordsConversionIdField.Text = BVApp.CurrentStore.Settings.Analytics.GoogleAdWordsId;
                this.GoogleAdwordsLabelField.Text = BVApp.CurrentStore.Settings.Analytics.GoogleAdWordsLabel;
                this.GoogleAdwordsBackgroundColorField.Text = BVApp.CurrentStore.Settings.Analytics.GoogleAdWordsBgColor;

                this.chkGoogleEcommerce.Checked = BVApp.CurrentStore.Settings.Analytics.UseGoogleEcommerce;
                this.GoogleEcommerceCategoryNameField.Text = BVApp.CurrentStore.Settings.Analytics.GoogleEcommerceCategory;
                this.GoogleEcommerceStoreNameField.Text = BVApp.CurrentStore.Settings.Analytics.GoogleEcommerceStoreName;

                this.chkGoogleTracker.Checked = BVApp.CurrentStore.Settings.Analytics.UseGoogleTracker;
                this.GoogleTrackingIdField.Text = BVApp.CurrentStore.Settings.Analytics.GoogleTrackerId;

                this.chkYahoo.Checked = BVApp.CurrentStore.Settings.Analytics.UseYahooTracker;
                this.YahooAccountIdField.Text = BVApp.CurrentStore.Settings.Analytics.YahooAccountId;

                this.AdditionalMetaTagsField.Text = BVApp.CurrentStore.Settings.Analytics.AdditionalMetaTags;

            }
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

            BVApp.CurrentStore.Settings.Analytics.UseGoogleAdWords = this.chkGoogleAdwords.Checked;
            BVApp.CurrentStore.Settings.Analytics.GoogleAdWordsId = this.GoogleAdwordsConversionIdField.Text;
            BVApp.CurrentStore.Settings.Analytics.GoogleAdWordsLabel = this.GoogleAdwordsLabelField.Text;
            BVApp.CurrentStore.Settings.Analytics.GoogleAdWordsBgColor = this.GoogleAdwordsBackgroundColorField.Text;

            BVApp.CurrentStore.Settings.Analytics.UseGoogleEcommerce = this.chkGoogleEcommerce.Checked;
            BVApp.CurrentStore.Settings.Analytics.GoogleEcommerceCategory = this.GoogleEcommerceCategoryNameField.Text;
            BVApp.CurrentStore.Settings.Analytics.GoogleEcommerceStoreName = this.GoogleEcommerceStoreNameField.Text;

            BVApp.CurrentStore.Settings.Analytics.UseGoogleTracker = this.chkGoogleTracker.Checked;
            BVApp.CurrentStore.Settings.Analytics.GoogleTrackerId = this.GoogleTrackingIdField.Text;

            BVApp.CurrentStore.Settings.Analytics.UseYahooTracker = this.chkYahoo.Checked;
            BVApp.CurrentStore.Settings.Analytics.YahooAccountId = this.YahooAccountIdField.Text;

            BVApp.CurrentStore.Settings.Analytics.AdditionalMetaTags = this.AdditionalMetaTagsField.Text;

            BVApp.UpdateCurrentStore();

            this.MessageBox1.ShowOk("Settings Saved!");

        }

    }

}