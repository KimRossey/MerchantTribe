using System;
using System.Web.UI;
using BVSoftware.Commerce.Membership;
using BVSoftware.AcumaticaTools;

namespace BVCommerce.BVAdmin.Configuration
{
    public partial class Acumatica : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Acumatica Integration Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                this.chkEnableAcumatica.Checked = BVApp.CurrentStore.Settings.Acumatica.IntegrationEnabled;
                this.AcumaticaSiteUrl.Text = BVApp.CurrentStore.Settings.Acumatica.SiteUrl;
                this.AcumaticaUsername.Text = BVApp.CurrentStore.Settings.Acumatica.Username;
                if (BVApp.CurrentStore.Settings.Acumatica.Password.Trim().Length > 0)
                {
                    this.AcumaticaPassword.Text = "********";
                }
                else
                {
                    this.AcumaticaPassword.Text = "";
                }
                int frequency = BVApp.CurrentStore.Settings.Acumatica.MinutesBetweenDataPulls;
                if (this.lstFrequency.Items.FindByValue(frequency.ToString()) != null)
                {
                    this.lstFrequency.ClearSelection();
                    this.lstFrequency.Items.FindByValue(frequency.ToString()).Selected = true;
                }
                
            }

            AcumaticaIntegration integrator = AcumaticaIntegration.Factory(BVApp);            
            this.litLastPulled.Text = BVApp.CurrentStore.Settings.Acumatica.LastCustomerPullUtc.ToString();
            DateTime nextPullDate = integrator.NextScheduledPullTime(BVApp);            
            this.litNextPull.Text = (nextPullDate.Year == 1900) ? "Never" : nextPullDate.ToString();            
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.Save() == true)
            {
                this.MessageBox1.ShowOk("Settings saved successfully.");
            }
        }

        private bool Save()
        {
            bool result = false;

            BVApp.CurrentStore.Settings.Acumatica.IntegrationEnabled = this.chkEnableAcumatica.Checked;
            if (this.AcumaticaPassword.Text != "********")
            {
                BVApp.CurrentStore.Settings.Acumatica.Password = this.AcumaticaPassword.Text.Trim();
            }
            BVApp.CurrentStore.Settings.Acumatica.Username = this.AcumaticaUsername.Text.Trim();
            BVApp.CurrentStore.Settings.Acumatica.SiteUrl = this.AcumaticaSiteUrl.Text.Trim();

            BVApp.CurrentStore.Settings.Acumatica.MinutesBetweenDataPulls = -1;
            int frequency = -1;
            if (int.TryParse(this.lstFrequency.SelectedValue, out frequency))
            {
                BVApp.CurrentStore.Settings.Acumatica.MinutesBetweenDataPulls = frequency;
            }
            result = BVApp.UpdateCurrentStore();

            // Make sure we have a scheduled task (and only one)
            BVApp.ScheduleServices.RemoveAllTasksForProcessor(BVApp.CurrentStore.Id, AcumaticaIntegration.PullDataTaskProcessorId);
            AcumaticaIntegration integrator = AcumaticaIntegration.Factory(BVApp);            
            integrator.GenerateQueuedTask(BVApp);
            
            return result;
        }

        protected void lnkTest_Click(object sender, System.EventArgs e)
        {
            this.MessageBox1.ClearMessage();

            if (!Save()) { this.MessageBox1.ShowError("Unable to save settings. Please try again."); return; }

            string password = BVApp.CurrentStore.Settings.Acumatica.Password;
            string username = BVApp.CurrentStore.Settings.Acumatica.Username;
            string siteUrl = BVApp.CurrentStore.Settings.Acumatica.SiteUrl;

            ServiceContext result = Connections.Login(username, password, siteUrl);
            if (result.HasLoggedIn)
            {
                this.MessageBox1.ShowOk("Test worked!.");
            }
            else
            {
                this.MessageBox1.ShowError("Unable to login with these settings");
                foreach (ServiceError err in result.Errors)
                {
                    MessageBox1.ShowError(err.Description);
                }
            }

        }

        protected void btnPullData_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();
            AcumaticaIntegration acumatica = AcumaticaIntegration.Factory(BVApp);
            BVSoftware.Commerce.Scheduling.QueuedTaskResult result = acumatica.ProcessTaskPullData(BVApp);
            if (result.Success)
            {
                this.MessageBox1.ShowOk("Pulled Data");
            }
            else
            {
                this.MessageBox1.ShowWarning("Failed to Pull Data: " + result.Notes);
            }
            
        }

    }
}