using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVSoftware.Commerce.Contacts;
using BVSoftware.Commerce.Content;
using MerchantTribe.Web.Geography;

namespace BVCommerce
{

    partial class BVModules_Controls_AddressShipping : BVSoftware.Commerce.Content.BVUserControl, MerchantTribe.Web.Validation.IValidatable
    {

        private bool _RequireName = true;

        public bool RequireName
        {
            get { return _RequireName; }
            set { _RequireName = value; }
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            string ns = this.shippingcountryname.NamingContainer.ClientID;
            ns = ns.Replace("_", "$");
            LoadRegions(Page.Request[ns + "$" + this.shippingcountryname.ID]);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadCountries();
            }
        }

        private void LoadCountries()
        {
            List<Country> countries = MyPage.BVApp.CurrentStore.Settings.FindActiveCountries();
            this.shippingcountryname.Items.Clear();
            foreach (Country c in countries)
            {
                this.shippingcountryname.Items.Add(new System.Web.UI.WebControls.ListItem(c.DisplayName, c.Bvin));
            }
            if (this.shippingcountryname.Items.FindByValue(Country.UnitedStatesCountryBvin) != null)
            {
                this.shippingcountryname.ClearSelection();
                this.shippingcountryname.Items.FindByValue(Country.UnitedStatesCountryBvin).Selected = true;
            }
            LoadRegions();
        }

        private void LoadRegions()
        {
            if (this.shippingcountryname.SelectedItem != null)
            {
                string countryBvin = this.shippingcountryname.SelectedItem.Value;
                LoadRegions(countryBvin);
            }
        }

        private void LoadRegions(string countryBvin)
        {
            Country c = Country.FindByBvin(countryBvin);
            if ((c != null))
            {
                this.shippingstate.Items.Clear();
                this.shippingstate.Items.Add(new ListItem("- Make a Selection -", ""));
                foreach (Region r in c.Regions)
                {
                    this.shippingstate.Items.Add(new System.Web.UI.WebControls.ListItem(r.Name, r.Abbreviation));
                }
            }
        }

        public System.Collections.Generic.List<MerchantTribe.Web.Validation.RuleViolation> GetRuleViolations()
        {
            List<MerchantTribe.Web.Validation.RuleViolation> violations = new List<MerchantTribe.Web.Validation.RuleViolation>();

            MerchantTribe.Web.Validation.ValidationHelper.Required("Shipping Country Name", this.shippingcountryname.SelectedValue, violations, "shippingcountryname");
            MerchantTribe.Web.Validation.ValidationHelper.Required("Shipping First Name", this.shippingfirstname.Text, violations, "shippingfirstname");
            MerchantTribe.Web.Validation.ValidationHelper.Required("Shipping Last Name", this.shippinglastname.Text, violations, "shippinglastname");
            MerchantTribe.Web.Validation.ValidationHelper.Required("Shipping Street", this.shippingaddress.Text, violations, "shippingaddress");
            MerchantTribe.Web.Validation.ValidationHelper.Required("Shipping City", this.shippingcity.Text, violations, "shippingcity");
            MerchantTribe.Web.Validation.ValidationHelper.Required("Shipping Postal Code", this.shippingzip.Text, violations, "shippingzip");

            MerchantTribe.Web.Validation.ValidationHelper.Required("Shipping Region/State",
                                                                this.shippingstate.Items[this.shippingstate.SelectedIndex].Value,
                                                                violations, "shippingstate");

            SetErrorCss(violations);

            return violations;
        }

        public bool IsValid()
        {
            if (GetRuleViolations().Count > 0)
            {
                return false;
            }
            return true;
        }

        private void SetErrorCss(List<MerchantTribe.Web.Validation.RuleViolation> violations)
        {

            // Clear Out Previous Error Classes
            this.shippingcountryname.CssClass = "";
            this.shippingfirstname.CssClass = "";
            this.shippinglastname.CssClass = "";
            this.shippingcompany.CssClass = "";
            this.shippingaddress.CssClass = "";
            this.shippingstate.Attributes["class"] = "";
            this.shippingcity.CssClass = "";
            this.shippingzip.CssClass = "";

            // Tag controls with violations with CSS class
            foreach (MerchantTribe.Web.Validation.RuleViolation v in violations)
            {
                if (v.ControlName == "shippingstate")
                {
                    System.Web.UI.HtmlControls.HtmlSelect cntrl = (System.Web.UI.HtmlControls.HtmlSelect)this.FindControl(v.ControlName);
                    if ((cntrl != null))
                    {
                        cntrl.Attributes["class"] = "input-validation-error";
                    }
                }
                else
                {
                    WebControl cntrl = (WebControl)this.FindControl(v.ControlName);
                    if ((cntrl != null))
                    {
                        cntrl.CssClass = "input-validation-error";
                    }
                }
            }

        }

        public void LoadFromAddress(Address a)
        {
            this.LoadCountries();

            if (a != null)
            {
                this.shippingaddressbvin.Value = a.Bvin;
                if (this.shippingcountryname.Items.FindByValue(a.CountryBvin) != null)
                {
                    this.shippingcountryname.ClearSelection();
                    this.shippingcountryname.Items.FindByValue(a.CountryBvin).Selected = true;
                }
                this.LoadRegions(this.shippingcountryname.SelectedValue);

                if (this.shippingstate.Items.Count > 0)
                {
                    this.shippingstate.SelectedIndex = 0;
                    for (int i = 0; i <= this.shippingstate.Items.Count - 1; i++)
                    {
                        if (this.shippingstate.Items[i].Value == a.RegionBvin)
                        {
                            this.shippingstate.SelectedIndex = i;
                        }
                    }
                }

                this.shippingfirstname.Text = a.FirstName;
                this.shippinglastname.Text = a.LastName;
                this.shippingcompany.Text = a.Company;
                this.shippingaddress.Text = a.Line1;
                this.shippingcity.Text = a.City;
                this.shippingzip.Text = a.PostalCode;
                this.shippingphone.Text = a.Phone;
            }
        }

        public Address GetAsAddress()
        {
            Address a = new Address();
            if (this.shippingcountryname.Items.Count > 0)
            {
                a.CountryBvin = shippingcountryname.SelectedValue;
                a.CountryName = shippingcountryname.SelectedItem.ToString();
            }
            else
            {
                a.CountryBvin = "";
                a.CountryName = "Unknown";
            }
            if (this.shippingstate.Items.Count > 0)
            {
                a.RegionName = shippingstate.Items[shippingstate.SelectedIndex].Text;
                a.RegionBvin = shippingstate.Items[shippingstate.SelectedIndex].Value;
            }
            a.FirstName = this.shippingfirstname.Text.Trim();
            a.LastName = this.shippinglastname.Text.Trim();
            a.Company = this.shippingcompany.Text.Trim();
            a.Line1 = this.shippingaddress.Text.Trim();
            a.City = this.shippingcity.Text.Trim();
            a.PostalCode = this.shippingzip.Text.Trim();
            if (this.shippingaddressbvin.Value != string.Empty)
            {
                a.Bvin = this.shippingaddressbvin.Value;
            }
            a.Phone = this.shippingphone.Text.Trim();
            return a;
        }

    }
}