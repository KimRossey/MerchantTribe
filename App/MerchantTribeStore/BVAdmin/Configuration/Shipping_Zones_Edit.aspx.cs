using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
using MerchantTribe.Web.Geography;
using System.Text;

namespace BVCommerce
{

    public partial class BVAdmin_Configuration_Shipping_Zones_Edit : BaseAdminPage
    {

        private Zone zone;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Shipping Zone";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            msg.ClearMessage();

            zone = new Zone();

            if (!Page.IsPostBack)
            {

                string id = Request.QueryString["id"];

                PopulateCountries();
                string homeCountry = WebAppSettings.ApplicationCountryBvin;
                lstCountry.SelectedValue = homeCountry;
                PopulateRegions(lstCountry.SelectedItem.Value);

                LoadZone(long.Parse(id));

            }
        }

        private void LoadZone(long id)
        {
            zone = BVApp.OrderServices.ShippingZones.Find(id);
            if (zone.StoreId == BVApp.CurrentStore.Id)
            {
                this.ZoneNameField.Text = zone.Name;
                LoadAreas(zone);
            }
            else
            {
                Response.Redirect("Shipping_Zones.aspx");
            }
        }

        void SetCountry(string Code)
        {
            foreach (System.Web.UI.WebControls.ListItem li in lstCountry.Items)
            {
                if (li.Value == Code)
                {
                    lstCountry.ClearSelection();

                    li.Selected = true;
                }
            }
            PopulateRegions(lstCountry.SelectedValue);
        }

        void PopulateCountries()
        {
            try
            {
                lstCountry.DataSource = BVApp.CurrentStore.Settings.FindActiveCountries();
                lstCountry.DataValueField = "IsoAlpha3";
                lstCountry.DataTextField = "DisplayName";
                lstCountry.DataBind();
            }
            catch (Exception Ex)
            {
                throw new ArgumentException("Error Loading Countries: " + Ex.Message);
            }
        }

        void SetRegion(string ID)
        {
            foreach (System.Web.UI.WebControls.ListItem li in lstState.Items)
            {
                if (li.Value == ID)
                {
                    lstState.ClearSelection();
                    li.Selected = true;
                }
            }
        }

        void PopulateRegions(string sID)
        {
            lstState.Items.Clear();
            try
            {
                lstState.DataSource = Country.FindByISOCode(sID).Regions;
                lstState.DataTextField = "name";
                lstState.DataValueField = "abbreviation";
                lstState.DataBind();

                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                li.Value = "";
                li.Text = "All States/Regions";
                lstState.Items.Insert(0, li);
                li = null;
            }

            catch (Exception Ex)
            {
                throw new ArgumentException(Ex.Message);
            }
        }

        protected void lstCountry_SelectedIndexChanged(object Sender, EventArgs E)
        {
            PopulateRegions(lstCountry.SelectedItem.Value);
        }

        protected void btnSaveChanges_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (Save())
            {
                Response.Redirect("Shipping_Zones.aspx");
            }
        }

        private bool Save()
        {
            string ids = Request.QueryString["id"];
            long id = long.Parse(ids);
            zone = BVApp.OrderServices.ShippingZones.Find(id);
            zone.Name = this.ZoneNameField.Text;
            return BVApp.OrderServices.ShippingZones.Update(zone);
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string zid = Request.QueryString["id"];
            long zoneId = long.Parse(zid);
            zone = BVApp.OrderServices.ShippingZones.Find(zoneId);

            ZoneArea a = new ZoneArea();
            a.CountryIsoAlpha3 = this.lstCountry.SelectedItem.Value;
            a.RegionAbbreviation = this.lstState.SelectedItem.Value;

            zone.Areas.Add(a);
            BVApp.OrderServices.ShippingZones.Update(zone);
            LoadZone(zone.Id);
        }

        private void LoadAreas(Zone z)
        {
            this.litAreas.Text = string.Empty;
            foreach (ZoneArea a in z.Areas)
            {
                RenderArea(a, z.Id);
            }
        }

        private void RenderArea(ZoneArea a, long zoneId)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<tr>");
            sb.Append("<td>" + a.CountryName + "</td>");
            sb.Append("<td>" + a.RegionAbbreviation + "</td>");
            sb.Append("<td><a href=\"Shipping_Zones_DeleteArea.aspx?id=" + zoneId.ToString() + "&country=" + a.CountryIsoAlpha3 + "&region=" + a.RegionAbbreviation + "\"><img src=\"../images/buttons/delete.png\" alt=\"Delete\" /></a></td>");
            sb.Append("</tr>");

            this.litAreas.Text += sb.ToString();
        }
    }
}