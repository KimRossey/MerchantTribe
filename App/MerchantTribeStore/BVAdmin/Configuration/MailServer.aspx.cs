
using System.Collections.ObjectModel;
using System.Collections.Generic;
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

    partial class BVAdmin_Configuration_MailServer : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Mail Server Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            string valueToSet = "0";
            this.pnlMain.Visible = false;

            if (BVApp.CurrentStore.Settings.MailServer.UseCustomMailServer)
            { valueToSet = "1"; this.pnlMain.Visible = true; }

            if (WebAppSettings.IsIndividualMode)
            {
                valueToSet = "1";
                this.pnlMain.Visible = true;
                this.lstMailServerChoice.Visible = false;
            }
           

            if (this.lstMailServerChoice.Items.FindByValue(valueToSet) != null)
            {
                this.lstMailServerChoice.ClearSelection();
                this.lstMailServerChoice.Items.FindByValue(valueToSet).Selected = true;
            }

            MailServerField.Text = BVApp.CurrentStore.Settings.MailServer.HostAddress;
            this.chkMailServerAuthentication.Checked = BVApp.CurrentStore.Settings.MailServer.UseAuthentication;
            this.UsernameField.Text = BVApp.CurrentStore.Settings.MailServer.Username;
            this.PasswordField.Text = "****************";

            this.chkSSL.Checked = BVApp.CurrentStore.Settings.MailServer.UseSsl;
            this.SmtpPortField.Text = BVApp.CurrentStore.Settings.MailServer.Port;

        }


        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveData();
            this.msg.ShowOk("Settings saved successfully.");
        }

        private void SaveData()
        {
            BVApp.CurrentStore.Settings.MailServer.HostAddress = MailServerField.Text.Trim();
            BVApp.CurrentStore.Settings.MailServer.UseAuthentication = this.chkMailServerAuthentication.Checked;
            BVApp.CurrentStore.Settings.MailServer.Username = this.UsernameField.Text.Trim();
            if (this.PasswordField.Text.Trim().Length > 0)
            {
                if (this.PasswordField.Text != "****************")
                {
                    BVApp.CurrentStore.Settings.MailServer.Password = this.PasswordField.Text.Trim();
                    this.PasswordField.Text = "****************";
                }
            }

            BVApp.CurrentStore.Settings.MailServer.UseSsl = this.chkSSL.Checked;
            BVApp.CurrentStore.Settings.MailServer.Port = this.SmtpPortField.Text.Trim();
            if (this.lstMailServerChoice.SelectedItem.Value == "1")
            {
                BVApp.CurrentStore.Settings.MailServer.UseCustomMailServer = true;
            }
            else
            {
                BVApp.CurrentStore.Settings.MailServer.UseCustomMailServer = false;
            }
            BVApp.UpdateCurrentStore();
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void btnSendTest_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveData();

            msg.ClearMessage();

            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage("testemail@bvcommerce.com", this.TestToField.Text.Trim());
            m.Subject = "Mail Server Test Message";
            m.Body = "Your mail server appears to be correctly configured!";
            m.IsBodyHtml = false;

            if (BVSoftware.Commerce.Utilities.MailServices.SendMail(m) == true)
            {
                msg.ShowOk("Test Message Sent");
            }
            else
            {
                msg.ShowError("Test Failed. Please check your settings and try again.");
            }

            m = null;
        }
        protected void lstMailServerChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstMailServerChoice.SelectedItem.Value == "0")
            {
                this.pnlMain.Visible = false;
            }
            else
            {
                this.pnlMain.Visible = true;
            }
        }
    }
}