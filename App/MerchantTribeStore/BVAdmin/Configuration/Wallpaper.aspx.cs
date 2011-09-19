using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;

namespace BVCommerce
{

    public partial class BVAdmin_Configuration_Wallpaper : BaseAdminPage
    {
        protected void btnBrown_Click(object sender, ImageClickEventArgs e)
        {
            SessionManager.SetCookieString("AdminWallpaper", "BrownStripes.jpg");
            Response.Redirect("wallpaper.aspx");
        }
        protected void btnNebula_Click(object sender, ImageClickEventArgs e)
        {
            SessionManager.SetCookieString("AdminWallpaper", "Nebula.jpg");
            Response.Redirect("wallpaper.aspx");
        }
        protected void btnLeather_Click(object sender, ImageClickEventArgs e)
        {
            SessionManager.SetCookieString("AdminWallpaper", "Leather.jpg");
            Response.Redirect("wallpaper.aspx");
        }
        protected void btnBlue_Click(object sender, ImageClickEventArgs e)
        {
            SessionManager.SetCookieString("AdminWallpaper", "BlueStripes.jpg");
            Response.Redirect("wallpaper.aspx");
        }
        protected void btnPink_Click(object sender, ImageClickEventArgs e)
        {
            SessionManager.SetCookieString("AdminWallpaper", "PinkStripes.jpg");
            Response.Redirect("wallpaper.aspx");
        }
        protected void btnPurple_Click(object sender, ImageClickEventArgs e)
        {
            SessionManager.SetCookieString("AdminWallpaper", "PurpleStripes.jpg");
            Response.Redirect("wallpaper.aspx");
        }
    }
}