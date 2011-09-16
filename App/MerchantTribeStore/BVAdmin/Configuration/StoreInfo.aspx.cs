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

    partial class BVAdmin_Content_StoreInfo : BaseAdminPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            this.ShipFromAddressField.RequireFirstName = false;
            this.ShipFromAddressField.RequireLastName = false;
            this.ShipFromAddressField.RequirePhone = false;
            this.ShipFromAddressField.RequirePostalCode = false;
            this.ShipFromAddressField.RequireRegion = false;
            this.ShipFromAddressField.RequireAddress = false;
            this.ShipFromAddressField.RequireCity = false;
            this.ShipFromAddressField.RequireCompany = false;

            if (!Page.IsPostBack)
            {

                LoadData();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Store's Address";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            if (this.Save() == true)
            {
                this.MessageBox1.ShowOk("Changes Saved!");
            }
            else
            {
                this.MessageBox1.ShowWarning("Unable to save changes.");
            }
        }

        private void LoadData()
        {
            this.ShipFromAddressField.LoadFromAddress(BVApp.ContactServices.Addresses.FindStoreContactAddress());
        }

        private bool Save()
        {
            bool result = false;

            Address toUpdate = this.ShipFromAddressField.GetAsAddress();
            toUpdate.AddressType = AddressTypes.StoreContact;
            if (toUpdate.Bvin == string.Empty)
            {
                result = BVApp.ContactServices.Addresses.Create(toUpdate);
            }
            else
            {
                result = BVApp.ContactServices.Addresses.Update(toUpdate);                
            }
            BVApp.ContactServices.Addresses.SubmitChanges();

            return result;

        }

    }
}