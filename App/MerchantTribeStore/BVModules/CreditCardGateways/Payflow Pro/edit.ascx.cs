using System;
using System.Web.UI.WebControls;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce.Payment;
using MerchantTribe.Payment.Methods;

namespace BVCommerce
{

    partial class BVModules_CreditCardGateways_BV_PayFlowProNET_edit : BVModule
    {

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveData();
            this.NotifyFinishedEditing();
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.NotifyFinishedEditing();
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            PayFlowProSettings settings = new PayFlowProSettings();
            settings.Merge(MyPage.BVApp.CurrentStore.Settings.PaymentSettingsGet(this.BlockId));

            this.txtMerchantVendor.Text = settings.MerchantPartner;
            this.txtMerchantLogin.Text = settings.MerchantLogin;
            this.txtMerchantUser.Text = settings.MerchantUser;
            if (settings.MerchantPassword.Length > 0)
            {
                this.txtMerchantPassword.Text = "**********";
            }
            //this.txtMerchantPassword.Text = settings.MerchantPassword;
            this.chkTestMode.Checked = settings.TestMode;
            this.chkDebugMode.Checked = settings.DeveloperMode;
            this.CurrencyCodeDropDownList.SelectedValue = settings.CurrencyCode;
        }

        private void SaveData()
        {
            PayFlowProSettings settings = new PayFlowProSettings();
            settings.Merge(MyPage.BVApp.CurrentStore.Settings.PaymentSettingsGet(this.BlockId));

            settings.MerchantPartner = this.txtMerchantVendor.Text.Trim();
            settings.MerchantLogin = this.txtMerchantLogin.Text.Trim();
            settings.MerchantUser = this.txtMerchantUser.Text.Trim();
            if (this.txtMerchantPassword.Text != "**********")
            {
                settings.MerchantPassword = this.txtMerchantPassword.Text.Trim();
            }
            settings.TestMode = this.chkTestMode.Checked;
            settings.DeveloperMode = this.chkDebugMode.Checked;
            settings.CurrencyCode = this.CurrencyCodeDropDownList.SelectedValue;

            MyPage.BVApp.CurrentStore.Settings.PaymentSettingsSet(this.BlockId, settings);

            MyPage.BVApp.AccountServices.Stores.Update(MyPage.BVApp.CurrentStore);
        }

    }
}