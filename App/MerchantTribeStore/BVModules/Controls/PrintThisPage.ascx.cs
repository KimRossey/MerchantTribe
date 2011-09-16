using System.Web.UI;

namespace BVCommerce
{

    partial class BVModules_Controls_PrintThisPage : BVSoftware.Commerce.Content.BVUserControl
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                this.imgPrint.ImageUrl = MyPage.BVApp.ThemeManager().ButtonUrl("printthispage", Request.IsSecureConnection);
            }
        }
    }
}