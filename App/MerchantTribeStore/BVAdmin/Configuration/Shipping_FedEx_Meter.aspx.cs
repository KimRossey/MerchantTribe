using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Commerce.BusinessRules;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Metrics;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce.Taxes;
using MerchantTribe.Commerce.Utilities;
using System.Collections.ObjectModel;
using MerchantTribe.Web.Geography;

namespace MerchantTribeStore.BVAdmin.Configuration
{
    public partial class Shipping_FedEx_Meter : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "FedEx Meter Number Registration";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadCountries();
                LoadStates();
                LoadData();
            }
        }

        private void LoadCountries()
        {
            Country defaultCountry = Country.FindByBvin(WebAppSettings.ApplicationCountryBvin);
            List<Country> countries = Country.FindAll();

            this.CountryCodeField.Items.Clear();
            foreach (Country c in countries)
            {
                this.CountryCodeField.Items.Add(new System.Web.UI.WebControls.ListItem(c.DisplayName, c.IsoCode));
            }

            if (this.CountryCodeField.Items.FindByValue(defaultCountry.IsoCode) != null)
            {
                this.CountryCodeField.ClearSelection();
                this.CountryCodeField.Items.FindByValue(defaultCountry.IsoCode).Selected = true;
            }

        }

        private void LoadStates()
        {
            Country c = Country.FindByISOCode(this.CountryCodeField.SelectedValue);
            if (c != null)
            {

                this.lstState.Items.Clear();

                if (c.Regions.Count > 0)
                {
                    this.lstState.Visible = true;
                    this.StateCodeField.Visible = false;
                    this.lstState.DataSource = c.Regions;
                    this.lstState.DataTextField = "Name";
                    this.lstState.DataTextField = "Abbreviation";
                    this.lstState.DataBind();
                }
                else
                {
                    this.lstState.Visible = false;
                    this.StateCodeField.Visible = true;
                }
            }
        }

        private void LoadData()
        {
            this.AccountNumberField.Text = MTApp.CurrentStore.Settings.ShippingFedExAccountNumber;
            if (MTApp.CurrentStore.Settings.ShippingFedExMeterNumber.Length > 0)
            {
                this.lblCurrentMeterNumber.Text = MTApp.CurrentStore.Settings.ShippingFedExMeterNumber;
            }
            else
            {
                this.lblCurrentMeterNumber.Text = "No Meter Number";
            }
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Shipping.aspx");
        }

        protected void btnSubmit_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

            MTApp.CurrentStore.Settings.ShippingFedExAccountNumber = this.AccountNumberField.Text.Trim();



            MerchantTribe.Shipping.FedEx.SubscriptionRequest req = new MerchantTribe.Shipping.FedEx.SubscriptionRequest();
            LoadRequest(req);

            MerchantTribe.Shipping.FedEx.SubscriptionResponse res = req.Send();

            if (res.Errors.Count > 0)
            {
                this.MessageBox1.ShowError(res.Errors[0].Message);
            }
            else
            {
                this.MessageBox1.ShowOk("Your meter number is: " + res.ReplyHeader.MeterNumber);
                MTApp.CurrentStore.Settings.ShippingFedExMeterNumber = res.ReplyHeader.MeterNumber;                
                this.lblCurrentMeterNumber.Text = res.ReplyHeader.MeterNumber;
            }

            // Save Changes
            MTApp.AccountServices.Stores.Update(MTApp.CurrentStore);
        }

        private void LoadRequest(MerchantTribe.Shipping.FedEx.SubscriptionRequest req)
        {
            req.RequestAddress.City = this.CityNameField.Text.Trim();
            req.RequestAddress.CountryCode = this.CountryCodeField.SelectedValue;
            req.RequestAddress.Line1 = this.Line1Field.Text.Trim();
            req.RequestAddress.Line2 = this.Line1Field.Text.Trim();
            req.RequestAddress.PostalCode = MerchantTribe.Commerce.Utilities.CreditCardValidator.CleanCardNumber(this.PostalCodeField.Text.Trim());
            if (this.lstState.Visible == true)
            {
                req.RequestAddress.StateOrProvinceCode = this.lstState.SelectedValue;
            }
            else
            {
                req.RequestAddress.StateOrProvinceCode = this.StateCodeField.Text.Trim();
            }
            req.RequestContact.CompanyName = this.CompanyNameField.Text.Trim();
            req.RequestContact.EmailAddress = this.EmailAddress.Text.Trim();
            //req.RequestContact.FaxNumber = Me.FaxNumberField.Text.Trim
            //req.RequestContact.PagerNumber = Me.PagerNumberField.Text.Trim
            req.RequestContact.PersonName = this.PersonNameField.Text.Trim();
            req.RequestContact.PhoneNumber = MerchantTribe.Commerce.Utilities.CreditCardValidator.CleanCardNumber(this.PhoneNumber.Text.Trim());
            req.RequestHeader.AccountNumber = this.AccountNumberField.Text.Trim();
        }

        protected void CountryCodeField_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadStates();
        }
    }
}