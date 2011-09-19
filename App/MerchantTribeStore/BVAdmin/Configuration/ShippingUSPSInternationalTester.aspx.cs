﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Shipping.USPostal;


namespace BVCommerce.BVAdmin.Configuration
{
    public partial class ShippingUSPSInternationalTester : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "USPS International Rates Tester";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                LoadServices();
                LoadCountries();
                this.FromZipField.Text = BVApp.ContactServices.Addresses.FindStoreContactAddress().PostalCode;
            }
        }

        private void LoadCountries()
        {
            this.lstCountries.DataSource = MerchantTribe.Web.Geography.Country.FindAll();
            this.lstCountries.DataTextField = "DisplayName";
            this.lstCountries.DataValueField = "USPostalServiceName";
            this.lstCountries.DataBind();
        }

        private void LoadServices()
        {
            MerchantTribe.Shipping.IShippingService uspostal = AvailableServices.FindById(WebAppSettings.ShippingUSPostalInternationalId, BVApp.CurrentStore);
            this.lstServiceTypes.DataSource = uspostal.ListAllServiceCodes();
            this.lstServiceTypes.DataTextField = "DisplayName";
            this.lstServiceTypes.DataValueField = "Code";
            this.lstServiceTypes.DataBind();
        }

        protected void btnGetRates_Click(object sender, EventArgs e)
        {


            MerchantTribe.Shipping.Shipment shipment = new MerchantTribe.Shipping.Shipment();
            shipment.DestinationAddress.CountryData.Name = this.lstCountries.SelectedItem.Value;
            shipment.SourceAddress.PostalCode = this.FromZipField.Text.Trim();

            // box
            MerchantTribe.Shipping.Shippable item = new MerchantTribe.Shipping.Shippable();
            decimal length = 0m;
            decimal.TryParse(this.LengthField.Text.Trim(), out length);
            decimal height = 0m;
            decimal.TryParse(this.HeightField.Text.Trim(), out height);
            decimal width = 0m;
            decimal.TryParse(this.WidthField.Text.Trim(), out width);
            decimal weightPounds = 0m;
            decimal.TryParse(this.WeightPoundsField.Text.Trim(), out weightPounds);
            decimal weightOunces = 0m;
            decimal.TryParse(this.WeightOuncesField.Text.Trim(), out weightOunces);
            item.BoxLength = length;
            item.BoxHeight = height;
            item.BoxWidth = width;
            item.BoxLengthType = MerchantTribe.Shipping.LengthType.Inches;
            item.BoxWeight = weightPounds + MerchantTribe.Web.Conversions.OuncesToDecimalPounds(weightOunces);
            item.BoxWeightType = MerchantTribe.Shipping.WeightType.Pounds;
            item.QuantityOfItemsInBox = 1;

            shipment.Items.Add(item);

            // Global Settings
            MerchantTribe.Shipping.USPostal.USPostalServiceGlobalSettings globalSettings = new USPostalServiceGlobalSettings();
            globalSettings.DiagnosticsMode = true;
            globalSettings.IgnoreDimensions = false;

            // Settings
            MerchantTribe.Shipping.USPostal.USPostalServiceSettings settings = new USPostalServiceSettings();
            MerchantTribe.Shipping.ServiceCode code = new MerchantTribe.Shipping.ServiceCode();
            code.Code = this.lstServiceTypes.SelectedItem.Value;
            code.DisplayName = this.lstServiceTypes.SelectedItem.Text;
            List<MerchantTribe.Shipping.IServiceCode> codes = new List<MerchantTribe.Shipping.IServiceCode>();
            codes.Add(code);
            settings.ServiceCodeFilter = codes;
            
            // Provider
            MerchantTribe.Web.Logging.TextLogger logger = new MerchantTribe.Web.Logging.TextLogger();
            MerchantTribe.Shipping.USPostal.InternationalProvider provider = new InternationalProvider(globalSettings, logger);
            provider.Settings = settings;

            List<MerchantTribe.Shipping.IShippingRate> rates = provider.RateShipment(shipment);

            this.litRates.Text = "<ul>";
            foreach (MerchantTribe.Shipping.IShippingRate rate in rates)
            {
                this.litRates.Text += "<li>" + rate.EstimatedCost.ToString("c") + " - " + rate.DisplayName + "</li>";
            }
            this.litRates.Text += "</ul>";

            this.litMessages.Text = "<ul>";
            foreach (MerchantTribe.Shipping.ShippingServiceMessage msg in provider.LatestMessages)
            {
                switch (msg.MessageType)
                {
                    case MerchantTribe.Shipping.ShippingServiceMessageType.Diagnostics:
                        this.litMessages.Text += "<li>DIAGNOSTICS:";
                        break;
                    case MerchantTribe.Shipping.ShippingServiceMessageType.Information:
                        this.litMessages.Text += "<li>INFO:";
                        break;
                    case MerchantTribe.Shipping.ShippingServiceMessageType.Error:
                        this.litMessages.Text += "<li>ERROR:";
                        break;
                }
                this.litMessages.Text += System.Web.HttpUtility.HtmlEncode(msg.Description + " " + msg.Code) + "</li>";
            }
            this.litMessages.Text += "</ul>";


            this.litXml.Text = "";
            while (logger.Messages.Count > 0)
            {
                string tempXml = logger.Messages.Dequeue();
                tempXml = tempXml.Replace("\n", "<br />");
                tempXml = tempXml.Replace("\r", "<br />");
                tempXml = tempXml.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
                this.litXml.Text += "<li>" + System.Web.HttpUtility.HtmlEncode(tempXml) + "</li>";
            }
        }

    }
}