﻿using System;
using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce.Membership;
using BVSoftware.Commerce.Utilities;
using BVSoftware.Commerce;

namespace BVCommerce.BVAdmin.Catalog
{
    public partial class Categories_FinishedEditing : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Finished Editing";
            this.CurrentTab = AdminTabType.Catalog;
            ValidateCurrentUserHasPermission(SystemPermissions.CatalogView);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string bvin = Request.QueryString["id"];
            BVApp.CurrentRequestContext.IsEditMode = false;
            Response.Redirect("Categories_EditFlexPage.aspx?id=" + bvin);
        }
    }
}