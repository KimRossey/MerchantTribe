using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BVCommerce.BVAdmin.People
{
    public partial class AdministratorsRemove : BaseAdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userId = Request.QueryString["id"];

            bool isOwner = BVApp.AccountServices.IsUserStoreOwner(BVApp.CurrentStore.Id, CurrentUser.Id);

            if (isOwner)
            {
                BVApp.AccountServices.RemoveUserFromStore(BVApp.CurrentStore.Id, long.Parse(userId));
            }

            Response.Redirect("Administrators.aspx");
        }
    }
}