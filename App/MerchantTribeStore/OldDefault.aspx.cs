using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVSoftware.Commerce;

namespace BVCommerce
{
	public partial class Default : BaseStorePage
	{
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            // Reset Last Category Session Variable
            SessionManager.CategoryLastId = string.Empty;
            this.Page.Title = BVApp.CurrentStore.Settings.FriendlyName;

        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.AddBodyClass("store-home-page");
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!WebAppSettings.IsIndividualMode)
            {
                if (BVApp.CurrentStore.StoreName == "www")
                {
                    Server.Transfer("/signup/home.aspx");
                }
            }
        }
	}
}