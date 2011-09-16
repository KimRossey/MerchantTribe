using System;
using System.Web;
using System.Web.UI.WebControls;
using BVSoftware.Commerce.Contacts;
using BVSoftware.Commerce.Membership;
using System.Collections.ObjectModel;
using BVSoftware.Commerce;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace BVCommerce
{

    partial class BVAdmin_People_Vendors : BaseAdminPage
    {
        private int pageSize = 50;
        private int rowCount = 0;
        private int currentPage = 1;
        private string keyword = string.Empty;

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Vendors";
            this.CurrentTab = AdminTabType.People;
            ValidateCurrentUserHasPermission(BVSoftware.Commerce.Membership.SystemPermissions.PeopleView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                if ((Request.QueryString["page"] != null))
                {
                    int.TryParse(Request.QueryString["page"], out currentPage);
                    if ((currentPage < 1))
                    {
                        currentPage = 1;
                    }
                }
                if ((Request.QueryString["keyword"] != null))
                {
                    this.keyword = Request.QueryString["keyword"];
                    this.FilterField.Text = keyword;
                }
                LoadData();
                this.FilterField.Focus();
            }
        }

        private void LoadData()
        {
            List<VendorManufacturer> items = BVApp.ContactServices.Vendors.FindAllWithFilter(this.keyword, currentPage, pageSize, ref rowCount);
            this.lblResults.Text = rowCount.ToString() + " found";
            this.litPager1.Text = MerchantTribe.Web.Paging.RenderPagerWithLimits("Vendors.aspx?page={0}&keyword=" + System.Web.HttpUtility.UrlEncode(keyword), currentPage, rowCount, pageSize, 20);
            RenderItems(items);
            this.litPager2.Text = this.litPager1.Text;
        }

        protected void btnGo_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Vendors.aspx?page=1&keyword=" + System.Web.HttpUtility.UrlEncode(this.FilterField.Text.Trim()));
        }

        private void RenderItems(List<VendorManufacturer> items)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table width=\"100%\"><tr><th>Name</th><th>Email</th><th>&nbsp;</th><th>&nbsp;</th></tr>");

            foreach (VendorManufacturer vm in items)
            {
                RenderSingleItem(sb, vm);
            }

            sb.Append("</table>");
            this.litResults.Text = sb.ToString();
        }

        private void RenderSingleItem(StringBuilder sb, VendorManufacturer vm)
        {

            string destinationLink = "Vendors_edit.aspx?id=" + vm.Bvin + "&page=" + currentPage + "&keyword=" + System.Web.HttpUtility.UrlEncode(keyword);
            string deleteLink = destinationLink.Replace("_edit", "_delete");

            sb.Append("<tr><td><a href=\"" + destinationLink + "\">" + vm.DisplayName + "</a></td>");
            sb.Append("<td><a href=\"" + destinationLink + "\">" + vm.EmailAddress + "</a></td>");
            sb.Append("<td><a onclick=\"return window.confirm('Delete this item?');\" href=\"" + deleteLink + "\" class=\"btn\"><b>Delete</b></a></td>");
            sb.Append("<td><a href=\"" + destinationLink + "\" class=\"btn\"><b>Edit</b></a></td></tr>");
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Vendors_edit.aspx?id=0&page=" + currentPage + "&keyword=" + System.Web.HttpUtility.UrlEncode(keyword));
        }

    }
}