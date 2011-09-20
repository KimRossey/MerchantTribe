using System;
using System.Web;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using System.Collections.Specialized;

namespace MerchantTribeStore
{

    partial class BVAdmin_Header : BVUserControl
    {
        public bool HideMenu { get; set; }

        protected internal string root = string.Empty;

        public AdminTabType SelectedTab
        {
            get
            {
                if (Session["ActiveAdminTab"] == null)
                {
                    return AdminTabType.Dashboard;
                }
                else
                {
                    return (AdminTabType)Session["ActiveAdminTab"];
                }
            }
            set { Session["ActiveAdminTab"] = value; }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            
            if (!HideMenu)
            {
                this.output.Text = Helpers.Html.AdminHeader(MyPage.MTApp.CurrentStore, SelectedTab);                
            }            

        }

    }
}