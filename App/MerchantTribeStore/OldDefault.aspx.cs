using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;

namespace MerchantTribeStore
{
	public partial class Default : BaseStorePage
	{
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            // Reset Last Category Session Variable
            SessionManager.CategoryLastId = string.Empty;
            this.Page.Title = MTApp.CurrentStore.Settings.FriendlyName;

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
                if (MTApp.CurrentStore.StoreName == "www")
                {
                    Server.Transfer("/signup/home.aspx");
                }
            }
        }
	}
}