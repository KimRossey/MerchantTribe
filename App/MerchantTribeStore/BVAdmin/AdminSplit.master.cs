using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;

namespace BVCommerce.BVAdmin
{
    public partial class AdminSplit : System.Web.UI.MasterPage
    {
        public string wallpaper = "BrownStripes.jpg";

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            string wall = SessionManager.GetCookieString("AdminWallpaper");
            if (wall != string.Empty)
            {
                wallpaper = wall;
            }

        }
    }
}