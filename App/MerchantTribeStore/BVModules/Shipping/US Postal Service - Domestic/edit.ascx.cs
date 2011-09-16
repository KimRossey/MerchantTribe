using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce.Shipping;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Datalayer;
using BVSoftware.Shipping;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using BVSoftware.Shipping.USPostal;
using BVSoftware.Shipping.USPostal.v4;

namespace BVCommerce
{

    partial class BVModules_Shipping_US_Postal_Service_edit : BVShippingModule
    {

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.NotifyFinishedEditing("Canceled");
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveData();
            this.NotifyFinishedEditing(this.NameField.Text.Trim());
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);            
            if (!Page.IsPostBack)
            {
                LoadZones();
                LoadServiceCodes();                
                LoadData();
            }
        }

        private void LoadZones()
        {
            this.lstZones.DataSource = MyPage.BVApp.OrderServices.ShippingZones.FindForStore(MyPage.BVApp.CurrentStore.Id);
            this.lstZones.DataTextField = "Name";
            this.lstZones.DataValueField = "id";
            this.lstZones.DataBind();
        }

        private void LoadServiceCodes()
        {
            BVSoftware.Shipping.IShippingService uspostal = AvailableServices.FindById(ShippingMethod.ShippingProviderId, CurrentStore);
            this.ShippingTypesCheckBoxList.DataSource = uspostal.ListAllServiceCodes();
            this.ShippingTypesCheckBoxList.DataTextField = "DisplayName";
            this.ShippingTypesCheckBoxList.DataValueField = "Code";
            this.ShippingTypesCheckBoxList.DataBind();
        }

        private void LoadData()
        {
            this.NameField.Text = ShippingMethod.Name;
            if (this.NameField.Text == string.Empty)
            {
                this.NameField.Text = "US Postal Service";
            }

            // Adjustment
            AdjustmentDropDownList.SelectedValue = ((int)ShippingMethod.AdjustmentType).ToString();
            if (ShippingMethod.AdjustmentType == ShippingMethodAdjustmentType.Amount)
            {
                AdjustmentTextBox.Text = string.Format("{0:c}", ShippingMethod.Adjustment);
            }
            else
            {
                AdjustmentTextBox.Text = string.Format("{0:f}", ShippingMethod.Adjustment);
            }


            // Zones
            if (this.lstZones.Items.FindByValue(ShippingMethod.ZoneId.ToString()) != null)
            {
                this.lstZones.ClearSelection();
                this.lstZones.Items.FindByValue(ShippingMethod.ZoneId.ToString()).Selected = true;
            }

            // Global
            this.Diagnostics.Checked = MyPage.BVApp.CurrentStore.Settings.ShippingUSPostalDiagnostics;

            USPostalServiceSettings settings = new USPostalServiceSettings();
            settings.Merge(ShippingMethod.Settings);

            foreach (BVSoftware.Shipping.ServiceCode code in settings.ServiceCodeFilter)
            {
                foreach (ListItem item in ShippingTypesCheckBoxList.Items)
                {
                    if (string.Compare(item.Value, code.Code, true) == 0)
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }

            string matchPackage = ((int)settings.PackageType).ToString();
            if (lstPackageType.Items.FindByValue(matchPackage) != null)
            {
                lstPackageType.ClearSelection();
                lstPackageType.Items.FindByValue(matchPackage).Selected = true;
            }
        }

        private void SaveData()
        {
            ShippingMethod.Name = this.NameField.Text.Trim();
            ShippingMethod.ZoneId = long.Parse(this.lstZones.SelectedItem.Value);
            ShippingMethod.AdjustmentType = (ShippingMethodAdjustmentType)int.Parse(AdjustmentDropDownList.SelectedValue);
            ShippingMethod.Adjustment = decimal.Parse(AdjustmentTextBox.Text, System.Globalization.NumberStyles.Currency);

            // Global Settings
            MyPage.BVApp.CurrentStore.Settings.ShippingUSPostalDiagnostics = this.Diagnostics.Checked;
            MyPage.BVApp.AccountServices.Stores.Update(MyPage.BVApp.CurrentStore);
            
            // Method Settings
            USPostalServiceSettings Settings = new USPostalServiceSettings();
            Settings.Merge(ShippingMethod.Settings);

            // Service Code
            List<BVSoftware.Shipping.IServiceCode> filter = new List<BVSoftware.Shipping.IServiceCode>();
                foreach (ListItem item in ShippingTypesCheckBoxList.Items)
                {
                    if (item.Selected)
                    {
                        BVSoftware.Shipping.ServiceCode code = new BVSoftware.Shipping.ServiceCode() { Code = item.Value, DisplayName = item.Text };
                        filter.Add(code);
                    }
                }
            Settings.ServiceCodeFilter = filter;
            
            // Package
            string packageCode = this.lstPackageType.SelectedItem.Value;
            int packageCodeInt = -1;
            if (int.TryParse(packageCode, out packageCodeInt))
            {
                Settings.PackageType = (BVSoftware.Shipping.USPostal.v4.DomesticPackageType)packageCodeInt;
            }


            ShippingMethod.Settings.Merge(Settings);
        }

        protected void CustomValidator1_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            decimal val = 0m;
            if (decimal.TryParse(AdjustmentTextBox.Text, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.CurrentCulture, out val))
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }

    }
}