using System.Collections.ObjectModel;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Membership;
using System;

namespace BVCommerce
{

    partial class BVModules_Controls_AdminPanel : MerchantTribe.Commerce.Content.BVUserControl
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            this.pnlMain.Visible = false;


            if (IsAdmin())
            {
                this.pnlMain.Visible = true;
             
                this.litAdminLink.Text = "<a href=\""
                    + MyPage.MTApp.CurrentStore.RootUrlSecure() 
                    + "bvadmin\" style=\"display:block;float:left;font-size:13px;line-height:24px;padding:6px 0 0 0;margin:-6px 0 0 0;color:#333;text-decoration:none;\"><img src=\"" 
                    + Page.ResolveUrl("~/images/system/AdminPanelLogo.png") 
                    + "\" alt=\"bvcommerce\" /></a>";


                if (MyPage.MTApp.CurrentStore.Settings.StoreClosed)
                {

                    this.litAdminLink.Text += "<a href=\""
                    + MyPage.MTApp.CurrentStore.RootUrlSecure()
                    + "bvadmin/configuration/general.aspx\" style=\"display:block;float:left;font-size:13px;line-height:24px;padding:6px 0 0 0;margin:-6px 0 0 0;color:#900;text-decoration:none;\">"
                    + "*** STORE IS CLOSED, SHOPPERS CAN'T SEE THIS PAGE ***</a>";                    
                }

                this.litAdminLink.Text += "<a href=\""
                    + MyPage.MTApp.CurrentStore.RootUrlSecure()
                    + "bvadmin\" style=\"display:block;float:right;font-size:13px;line-height:24px;padding:6px 0 0 0;margin:-6px 0 0 0;color:#333;text-decoration:none;\">Go To Admin Dashboard</a>";
            }         
        }

        public bool IsAdmin()
        {

            Guid? tokenId = MerchantTribe.Web.Cookies.GetCookieGuid(WebAppSettings.CookieNameAuthenticationTokenAdmin(),
                    this.Page.Request.RequestContext.HttpContext, new EventLog());

            // no token, return
            if (!tokenId.HasValue) return false;

            if (MyPage.MTApp.AccountServices.IsTokenValidForStore(MyPage.MTApp.CurrentStore.Id, tokenId.Value))
            {
                return true;
            }

            return false;
        }


    }
}