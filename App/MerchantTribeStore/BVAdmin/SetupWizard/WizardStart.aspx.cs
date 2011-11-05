using System;
using System.Collections.Generic;
using MerchantTribe.Commerce.Content;
using System.Text;

namespace MerchantTribeStore.BVAdmin.SetupWizard
{
    public partial class WizardStart : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "End User License Agreement";
            this.CurrentTab = AdminTabType.Configuration;
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            LoadLicense();            
        }

        private void LoadLicense()
        {

            if (MerchantTribe.Commerce.WebAppSettings.IsIndividualMode)
            {
                
            }
            else
            {

            }
        }
    }
}