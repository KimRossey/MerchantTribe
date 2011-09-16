using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVSoftware.Commerce.Contacts;
using BVSoftware.Commerce.Content;
using MerchantTribe.Web.Geography;

namespace BVCommerce
{

    partial class BVModules_Controls_AddressBilling : BVSoftware.Commerce.Content.BVUserControl, MerchantTribe.Web.Validation.IValidatable
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
            string ns = this.billingcountryname.NamingContainer.ClientID;
            ns = ns.Replace("_", "$");
            LoadRegions(Page.Request[ns + "$" + this.billingcountryname.ID]);
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
            this.billingcountryname.Items.Clear();
            foreach (Country c in countries)
            {
                this.billingcountryname.Items.Add(new System.Web.UI.WebControls.ListItem(c.DisplayName, c.Bvin));
            }
            if (this.billingcountryname.Items.FindByValue(Country.UnitedStatesCountryBvin) != null)
            {
                this.billingcountryname.ClearSelection();
                this.billingcountryname.Items.FindByValue(Country.UnitedStatesCountryBvin).Selected = true;
            }
            LoadRegions();
        }

        private void LoadRegions()
        {
            if (this.billingcountryname.SelectedItem != null)
            {
                string countryBvin = this.billingcountryname.SelectedItem.Value;
                LoadRegions(countryBvin);
            }
        }

        private void LoadRegions(string countryBvin)
        {
            Country c = Country.FindByBvin(countryBvin);
            if ((c != null))
            {
                this.billingstate.Items.Clear();
                this.billingstate.Items.Add(new ListItem("- Make a Selection -", ""));
                foreach (Region r in c.Regions)
                {
                    this.billingstate.Items.Add(new System.Web.UI.WebControls.ListItem(r.Name, r.Abbreviation));
                }
            }
        }

        public System.Collections.Generic.List<MerchantTribe.Web.Validation.RuleViolation> GetRuleViolations()
        {
            List<MerchantTribe.Web.Validation.RuleViolation> violations = new List<MerchantTribe.Web.Validation.RuleViolation>();

            MerchantTribe.Web.Validation.ValidationHelper.Required("Billing Country Name", this.billingcountryname.SelectedValue, violations, "billingcountryname");
            MerchantTribe.Web.Validation.ValidationHelper.Required("Billing First Name", this.billingfirstname.Text, violations, "billingfirstname");
            MerchantTribe.Web.Validation.ValidationHelper.Required("Billing Last Name", this.billinglastname.Text, violations, "billinglastname");
            MerchantTribe.Web.Validation.ValidationHelper.Required("Billing Street", this.billingaddress.Text, violations, "billingaddress");
            MerchantTribe.Web.Validation.ValidationHelper.Required("Billing City", this.billingcity.Text, violations, "billingcity");
            MerchantTribe.Web.Validation.ValidationHelper.Required("Billing Postal Code", this.billingzip.Text, violations, "billingzip");
            
            MerchantTribe.Web.Validation.ValidationHelper.Required("Billing Region/State", 
                                                                this.billingstate.Items[this.billingstate.SelectedIndex].Value, 
                                                                violations, "billingstate");

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
            this.billingcountryname.CssClass = "";
            this.billingfirstname.CssClass = "";
            this.billinglastname.CssClass = "";
            this.billingcompany.CssClass = "";
            this.billingaddress.CssClass = "";
            this.billingstate.Attributes["class"] = "";
            this.billingcity.CssClass = "";
            this.billingzip.CssClass = "";

            // Tag controls with violations with CSS class
            foreach (MerchantTribe.Web.Validation.RuleViolation v in violations)
            {
                if (v.ControlName == "billingstate")
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
                this.billingaddressbvin.Value = a.Bvin;
                if (this.billingcountryname.Items.FindByValue(a.CountryBvin) != null)
                {
                    this.billingcountryname.ClearSelection();
                    this.billingcountryname.Items.FindByValue(a.CountryBvin).Selected = true;
                }
                this.LoadRegions(this.billingcountryname.SelectedValue);

                if (this.billingstate.Items.Count > 0)
                {
                    this.billingstate.SelectedIndex = 0;
                    if (this.billingstate.Items.FindByValue(a.RegionBvin) != null)
                    {
                        this.billingstate.Items.FindByValue(a.RegionBvin).Selected = true;
                    }
                }

                this.billingfirstname.Text = a.FirstName;
                this.billinglastname.Text = a.LastName;
                this.billingcompany.Text = a.Company;
                this.billingaddress.Text = a.Line1;
                this.billingcity.Text = a.City;
                this.billingzip.Text = a.PostalCode;
            }
        }

        public Address GetAsAddress()
        {
            Address a = new Address();
            if (this.billingcountryname.Items.Count > 0)
            {
                a.CountryBvin = billingcountryname.SelectedValue;
                a.CountryName = billingcountryname.SelectedItem.ToString();
            }
            else
            {
                a.CountryBvin = "";
                a.CountryName = "Unknown";
            }
            if (this.billingstate.Items.Count > 0)
            {
                a.RegionName = billingstate.Items[billingstate.SelectedIndex].Text;
                a.RegionBvin = billingstate.Items[billingstate.SelectedIndex].Value;
            }
            a.FirstName = this.billingfirstname.Text.Trim();
            a.LastName = this.billinglastname.Text.Trim();
            a.Company = this.billingcompany.Text.Trim();
            a.Line1 = this.billingaddress.Text.Trim();
            a.City = this.billingcity.Text.Trim();
            a.PostalCode = this.billingzip.Text.Trim();
            if (this.billingaddressbvin.Value != string.Empty)
            {
                a.Bvin = this.billingaddressbvin.Value;
            }
            return a;
        }

    }
}