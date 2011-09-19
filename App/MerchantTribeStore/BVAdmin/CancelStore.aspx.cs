using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;

namespace BVCommerce
{

    public partial class BVAdmin_CancelStore : BaseAdminPage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            UserAccount u = GetCorrectUser();
            Store s = FindValidStoreForUser(u, long.Parse(Request.QueryString["id"]));
            if (s == null) Response.Redirect("/bvadmin/account.aspx");
            this.lblStoreName.Text = s.Settings.FriendlyName;
        }

        private Store FindValidStoreForUser(UserAccount u, long storeId)
        {
            return BVApp.AccountServices.FindStoreByIdForUser(storeId, u.Id);
        }

        private UserAccount GetCorrectUser()
        {
            UserAccount u = CurrentUser;

            if (u != null)
            {
                if (u.Status == UserAccountStatus.SuperUser)
                {
                    // don't use current user, get the owner of the store instead
                    List<UserAccount> users = BVApp.AccountServices.FindAdminUsersByStoreId(BVApp.CurrentStore.Id);
                    if (users != null)
                    {
                        if (users.Count > 0)
                        {
                            return users[0];
                        }
                    }
                }
            }

            return u;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            UserAccount u = GetCorrectUser();
            long storeId = 0;
            storeId = long.Parse(Request.QueryString["id"]);
            BVApp.AccountServices.CancelStore(storeId, u.Id);
            Response.Redirect("/bvadmin/account.aspx");
        }
    }
}