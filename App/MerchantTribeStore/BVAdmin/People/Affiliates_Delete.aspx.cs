﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BVCommerce.BVAdmin.People
{
    public partial class Affiliates_Delete : BaseAdminPage
    {
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Affiliates";
            this.CurrentTab = AdminTabType.People;
            ValidateCurrentUserHasPermission(MerchantTribe.Commerce.Membership.SystemPermissions.PeopleView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {

                int currentPage = 1;
                string keyword = string.Empty;
                string id = string.Empty;

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
                    keyword = Request.QueryString["keyword"];
                }

                if (Request.QueryString["id"] != null)
                {
                    string itemId = Request.QueryString["id"];
                    long temp = 0;
                    long.TryParse(itemId, out temp);
                    BVApp.ContactServices.Affiliates.Delete(temp);
                }

                Response.Redirect("Affiliates.aspx?page=" + currentPage.ToString() + "&keyword=" + System.Web.HttpUtility.UrlEncode(keyword));
            }
        }
    }
}